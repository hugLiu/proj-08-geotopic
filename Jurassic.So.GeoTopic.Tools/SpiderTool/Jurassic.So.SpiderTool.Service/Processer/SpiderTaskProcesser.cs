using Jurassic.PKS.Service;
using Jurassic.PKS.Service.Adapter;
using Jurassic.PKS.Service.Index;
using Jurassic.PKS.WebAPI.Models.Data;
using Jurassic.PKS.WebAPI.Models.Index;
using Jurassic.So.Data.Entities;
using Jurassic.So.SpiderTool.IService;
using Jurassic.So.SpiderTool.IService.Processers;
using Jurassic.So.SpiderTool.Service.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jurassic.So.SpiderTool.Service.Processer
{
    /// <summary>
    /// 数据爬取任务执行方法，主要需要完成以下任务
    /// 1. 按照爬取范围从数据源中爬取索引数据
    /// 2. 调用索引服务发送索引数据
    /// 5. 记录日志
    /// </summary>
    public class SpiderTaskProcesser : ProcesserBase
    {
        #region 属性
        /// <summary>
        /// 爬取范围各自的进度
        /// </summary>
        public Dictionary<string, int> SpiderPercent { get; set; } = new Dictionary<string, int>();
        /// <summary>
        /// 当前执行状态
        /// </summary>
        public Dictionary<string, ProcessStatus> ExecuteStatus { get; set; } = new Dictionary<string, ProcessStatus>();
        public Dictionary<string, int> ExecuteTimes { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, string> ExecuteStartTime { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> ExecuteDuration { get; set; } = new Dictionary<string, string>();
        /// <summary>
        /// 任务开始执行的时间
        /// </summary>
        public string ProcessStartTime { get; set; }
        private readonly DateTime _processStartTime;
        public string ProcessTimeCost { get; set; } = "00:00:00";
        #endregion

        private readonly IWrapedDataService _wrapedDataService = null;
        private readonly IWrapedIndexerService _wrapedIndexerService = null;
        private  DataServiceDBContext _context = null;

        public  SpiderTaskProcesser()
        {
            _processStartTime = DateTime.Now;
            ProcessStartTime = _processStartTime.ToString("yyyy-MM-dd hh:mm:ss");
            _wrapedDataService = new ServiceFactory().NewWrapedDataService();
            _wrapedIndexerService = new ServiceFactory().NewWrapedIndexerService();
            _context = new DataServiceDBContext();
        }

        /// <summary>
        /// 任务处理核心
        /// </summary>
        /// <param name="item">任务参数</param>
        public override void Process(object item)
         {          
            var spiderParam = (SpiderParam)item;
            var scopeId = 0;
            var scope = "";
            var markCount = 0;
            var startTime = DateTime.Now;
            var taskLogId = Guid.NewGuid();
            var success = true;
            var errorMsg = string.Empty;
            var recondCount = 0;

            try
            {
                var adapterName = spiderParam.GtSpiderScope.GT_AdapterInfo.AdapterName;
                scopeId = spiderParam.GtSpiderScope.Id;
                scope = spiderParam.GtSpiderScope.SpiderScope;
                var incrementValue = spiderParam.GtSpiderScope.IncrementValue;
                var spiderSize = spiderParam.GtSpiderScope.GT_AdapterInfo.SpiderSize ?? 20;
                var executeTimes = spiderParam.GtSpiderScope.ExecuteTimes;
                if (executeTimes != null)
                    ExecuteTimes[scope] = (int) executeTimes;
                ExecuteStartTime[scope] = startTime.ToString("yyyy-MM-dd hh:mm:ss");
                ExecuteStatus[scope] = ProcessStatus.Running;

                //从数据服务爬取数据 调用数据服务WEB API的Spider接口实现
                var request = new SpiderRequest
                {
                    AdapterId = adapterName,
                    Scope = scope,
                    IncrementValue = incrementValue,
                    Pager = new Pager(0, 1) //先取一条 目的是获得total
                };

                var data = _wrapedDataService.Spider(request);
                int total = data.Total;
                double incrementPercent = spiderSize*100.0/total;
                double scopePercent = 0;
                //分批爬取
                while (markCount < total)
                {
                    //触发进度广播
                    this.FireProgressChanged();
                    request.Pager.Size = spiderSize;
                    request.Pager.From = markCount;
                    recondCount += SpiderBySize(spiderParam, request);
                    markCount += spiderSize;
                    scopePercent += incrementPercent;
                    SpiderPercent[scope] = Convert.ToInt16(Math.Round(scopePercent));
                    if (scopePercent > 100)
                        SpiderPercent[scope] = 100;
                    ExecuteDuration[scope] = (DateTime.Now - startTime).ToString().Substring(0, 8);
                    ProcessTimeCost = (DateTime.Now - _processStartTime).ToString().Substring(0, 8);
                }
                SpiderPercent[scope] = 100;
                ExecuteStatus[scope] = ProcessStatus.Successed;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                if (ex.InnerException?.Message != null)
                    errorMsg = ex.InnerException.Message;
                success = false;
                ExecuteStatus[scope] = ProcessStatus.Failed;
                Log.WriteError("爬取任务失败,爬取范围：" + scope, ex);
            }
            finally
            {
                var endTime = DateTime.Now;
                ExecuteDuration[scope] = (endTime - startTime).ToString().Substring(0, 8);
                ProcessTimeCost = (endTime - _processStartTime).ToString().Substring(0, 8);
                UpdateExecuteTimes(scopeId, scope);
                //触发进度广播
                this.FireProgressChanged();
                WriteTaskLog(spiderParam, taskLogId, success, recondCount, errorMsg, startTime, endTime);
            }
        }

        /// <summary>
        /// 按照SpiderSize分批爬取数据
        /// </summary>
        /// <param name="spiderParam"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        private int SpiderBySize(SpiderParam spiderParam, SpiderRequest request)
        {
            int recondCount = 0;
            string maxValue = "";
            try
            {
                var data = _wrapedDataService.Spider(request);
                maxValue = data.MaxValue;
                var metadatas = data.Metadatas;
                if (metadatas != null)
                {
                    recondCount = SaveIndexData(metadatas.ToArray());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                UpdateIncrementValue(spiderParam.GtSpiderScope, maxValue);
            }           
            return recondCount;
        }

        /// <summary>
        /// 保存爬取数据到索引库(调用索引服务)
        /// </summary>
        /// <param name="metadatas">元数据集</param>
        /// <returns></returns>
        private int SaveIndexData(object[] metadatas)
        {
            try
            {
                var indexInfoRequest = new IndexInfoRequest
                {
                    Action = IndexAction.Save,
                    Metadatas = metadatas
                };
                var indexResult = _wrapedIndexerService.SendIndex(indexInfoRequest);
                return indexResult?.IIIds?.Count ?? 0;
            }
            catch (Exception ex)
            {                
                throw ex;
            }            
        }

        /// <summary>
        /// 更新执行次数和最后一次执行状态
        /// </summary>
        /// <param name="scopeId"></param>
        /// <param name="scope"></param>
        private void UpdateExecuteTimes(int scopeId , string scope)
        {
            try
            {
                var gtSpiderScope = _context.GT_SpiderScope
                .FirstOrDefault(t => t.Id == scopeId);
                if (gtSpiderScope == null) return;
                if (gtSpiderScope.ExecuteTimes != null) gtSpiderScope.ExecuteTimes += 1;
                else gtSpiderScope.ExecuteTimes = 1;
                ExecuteTimes[scope] = (int)gtSpiderScope.ExecuteTimes;
                gtSpiderScope.LastSpiderStatus = (int?)ExecuteStatus[scope];
                _context.SaveChanges();
            }
            catch (Exception ex)
            {               
                throw ex;
            }            
        }

        /// <summary>
        /// 记录任务执行日志
        /// </summary>
        /// <param name="spiderParam"></param>
        /// <param name="tasklogId"></param>
        /// <param name="success"></param>
        /// <param name="recordCount"></param>
        /// <param name="errorMsg"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        private void WriteTaskLog(SpiderParam spiderParam, Guid tasklogId, bool success, int recordCount, string errorMsg, DateTime? startDate, DateTime? endDate)
        {
            var log = new GT_TaskLogInfo
            {
                TaskLogInfoId = tasklogId,
                SpiderScopeId = spiderParam.GtSpiderScope.Id,
                AdapterId = spiderParam.GtSpiderScope.GT_AdapterInfo.Id,
                AdapterName = spiderParam.GtSpiderScope.GT_AdapterInfo.AdapterName,
                DataSourceName = spiderParam.GtSpiderScope.GT_AdapterInfo.DataSourceName,
                AdapterTaskLogIdentity = this.TaskInstanceId,
                SpiderScope = spiderParam.GtSpiderScope.SpiderScope,
                Performer = spiderParam.User,
                Success = success,
                RecordCount = recordCount,
                FailturReason = errorMsg,
                StartDate = startDate,
                EndDate = endDate,
                CreatedDate = DateTime.Now,
                IsInvalid = false,
                Deleted = false
            };
            _context.GT_TaskLogInfo.Add(log);
            _context.SaveChanges();
        }

        /// <summary>
        /// 更新增量值
        /// </summary>
        /// <param name="spiderScope"></param>
        /// <param name="maxValue">增量值</param>
        private void UpdateIncrementValue(GT_SpiderScope spiderScope, string  maxValue)
        {
            try
            {
                var gtSpiderScope = _context.GT_SpiderScope.FirstOrDefault(t => t.Id == spiderScope.Id);
                if (gtSpiderScope == null) return;

                if (gtSpiderScope.IncrementType == IncrementType.Date.ToString() ||
                    gtSpiderScope.IncrementType == IncrementType.ID.ToString())
                {
                    gtSpiderScope.IncrementValue = maxValue;
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Log.WriteError("更新增量值出错", ex);
                throw ex;
            }
            
        }
    }
}
