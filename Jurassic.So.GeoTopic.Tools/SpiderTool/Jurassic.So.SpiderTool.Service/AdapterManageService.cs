using Jurassic.So.SpiderTool.IService;
using Jurassic.So.SpiderTool.IService.ViewModel;
using Jurassic.So.SpiderTool.Service.Util;
using Jurassic.So.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Jurassic.PKS.Service.Adapter;

namespace Jurassic.So.SpiderTool.Service
{
    public class AdapterManageService : IAdapterManageService
    {
        private DataServiceDBContext _context { get; set; }
        private DataNodeDBContext _dataNodeDbContext { get; set; }
        private string _parentId { get; set; }

        public AdapterManageService()
        {
            _context = new DataServiceDBContext();
            _dataNodeDbContext = new DataNodeDBContext();
            _parentId =
                _dataNodeDbContext.API_DATA_NODE_INFO.FirstOrDefault(d => d.DataParentID == "undefined")?.DataID;
        }        

        /// <summary>
        /// 查询适配器信息
        /// </summary>
        /// <param name="adapterName">适配器名称</param>
        /// <param name="status">工作状态</param>
        /// <param name="hangup">挂起状态</param>
        /// <returns></returns>
        public AdapterInfoModelPager GetAdapterInfoPageList(string adapterName, string status, string hangup, int pageIndex, int pageSize)
        {
            var data = _context.GT_AdapterInfo.Select(AutoMapper.Mapper.Map<GT_AdapterInfo, AdapterInfoModel>);
            if (!string.IsNullOrEmpty(adapterName))
            {
                data = data.Where(t => t.AdapterName.Contains(adapterName.Trim()));                       
            }            
            if(!string.IsNullOrEmpty(status))
            {
                data = data.Where(t => t.Status.Value == int.Parse(status));
            }
            if(!string.IsNullOrEmpty(hangup))
            {
                data = data.Where(t => t.Hangup.Value == int.Parse(hangup)); 
            }            
            AdapterInfoModelPager pageData = new AdapterInfoModelPager();
            pageData.total = data.Count();
            pageData.data = data.OrderByDescending(t => t.CreateDate)
                                .Skip(pageSize * (pageIndex))
                                .Take(pageSize).ToList();
            return pageData;            
        }

        /// <summary>
        /// 获取适配下具体的数据类型，并实现数据类型条件过滤
        /// </summary>
        /// <param name="adapterId">适配器ID</param>
        /// <param name="spiderScope">数据类型</param>
        /// <returns></returns>
        public List<SpiderScopeModel> GetDataTypeList(string adapterId, string spiderScope)
        {
            if (string.IsNullOrWhiteSpace(adapterId))
                return null;
            List<GT_SpiderScope> dataTypesList = new List<GT_SpiderScope>();
            if (!string.IsNullOrWhiteSpace(spiderScope))//当传入的数据类型参数不为空值
            {
                spiderScope = spiderScope.Trim();
                dataTypesList =
                    _context.GT_SpiderScope.Where(p => p.AdapterId.ToString() == adapterId && p.SpiderScope.Contains(spiderScope) && p.IsInvalid.Value == false).ToList();
            }
            else
            {
                dataTypesList = _context.GT_SpiderScope.Where(p => p.AdapterId.ToString() == adapterId && p.IsInvalid.Value == false).ToList();
            }
            return dataTypesList.Select(entity => new SpiderScopeModel
            {
                AdapterId = entity.AdapterId,
                SpiderScope = entity.SpiderScope,
                IncrementType = entity.IncrementType,
                Priority = entity.Priority ?? 0
            }).ToList();
        }

        /// <summary>
        /// 挂起或取消挂起适配器
        /// </summary>
        /// <param name="adapterId">适配器ID</param>
        /// <param name="userName">更新人</param>
        public void HangupAdapter(string adapterId, string userName)
        {
            if (!string.IsNullOrEmpty(adapterId))
            {
                var item = _context.GT_AdapterInfo.FirstOrDefault(t => t.Id.ToString() == adapterId);
                if (item?.Hangup != null)
                {
                    item.Hangup = item.Hangup.Value == 1 ? 0 : 1;
                    item.UpdatedDate = DateTime.Now;
                    item.UpdatedBy = userName;
                }
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// 注册适配器
        /// </summary>
        /// <param name="adapterInfoCom">适配器信息</param>
        /// <param name="adapterSpiderScopesCom">适配器数据类型</param>
        /// <param name="ds_AI"></param>
        /// <returns>1:成功,0:失败,-1:重复的适配器名称,-2:重复的适配器数据类型,-3:重复的适配器Id</returns>
        public int RegisterAdapter(AdapterInfo adapterInfoCom, GT_AdapterInfo ds_AI, List<Scope> adapterSpiderScopesCom, string userName)
        {
            try
            {
                var adapterTemp = _context.GT_AdapterInfo.FirstOrDefault(t => t.AdapterName.ToString() == adapterInfoCom.Id);
                if (adapterTemp != null)
                {
                    return -1;
                }
                var adapterSpiderScopeTemp = (from t in adapterSpiderScopesCom
                                           group t by t into g
                                           where g.Count() >= 2
                                           select g.Key).ToList();
                if (adapterSpiderScopeTemp.Count > 0)
                {
                    Log.WriteError("注册适配器包爬取范围发生重复", adapterSpiderScopeTemp.FirstOrDefault().Name);
                    return -2;
                }
                //加载适配器信息
                var adapterEntry = AutoMapper.Mapper.Map<AdapterInfo, GT_AdapterInfo>(adapterInfoCom);                
                adapterEntry.AdapterURL = ds_AI.AdapterURL;
                adapterEntry.ClassName = ds_AI.ClassName;
                adapterEntry.ConfigAddress = ds_AI.ConfigAddress;
	            adapterEntry.InvokeType = ds_AI.InvokeType;
                adapterEntry.Hangup = 0;
                adapterEntry.Status = 1;
                adapterEntry.IsInvalid = false;
                adapterEntry.OrderIndex = 1;
                adapterEntry.CreatedDate = DateTime.Now;
                adapterEntry.CreatedBy = userName;
                _context.GT_AdapterInfo.Add(adapterEntry);

                //加载适配器数据类型
                foreach (var item in adapterSpiderScopesCom)
                {
                    GT_SpiderScope adapterDtEntry = new GT_SpiderScope
                    {
                        AdapterId = adapterEntry.Id,
                        SpiderScope = item.Name,
                        IncrementType = item.IncrementType.ToString(),
                        Priority = 1,
                        CreatedDate = DateTime.Now,
                        IsInvalid = false
                    };
                    _context.GT_SpiderScope.Add(adapterDtEntry);
                }
                _context.SaveChanges();

                //注册时将信息加入数据节点权限控制 管理部框架数据库表 API_DATA_NODE_INFO
                API_DATA_NODE_INFO dataNodeInfo = new API_DATA_NODE_INFO();
                try
                {
                    dataNodeInfo.DataID = Guid.NewGuid().ToString();
                    dataNodeInfo.DataParentID = _parentId;
                    dataNodeInfo.DataNodeName = adapterEntry.AdapterName;
                    dataNodeInfo.DataNodeID = adapterEntry.Id.ToString();
                    dataNodeInfo.Memo = "NULL";
                    dataNodeInfo.IsValid = 1;
                    dataNodeInfo.CreatedDate = DateTime.Now;
                    dataNodeInfo.CreatedBy = "pmis";
                    _dataNodeDbContext.API_DATA_NODE_INFO.Add(dataNodeInfo);
                    _dataNodeDbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    //TODO: 失败时需要回滚
                    Log.WriteError("加入服务管理-数据节点时发生错误", ex);
                    return 1;
                }
                return 1;
            }
            catch (Exception ex)
            {
                Log.WriteError("注册适配器包发生错误", ex);
                return 0;
            }
        }

        /// <summary>
        /// 反注册适配器
        /// </summary>
        /// <param name="adapterId">适配器ID</param>
        /// <returns></returns>
        public string UnRegisterAdapter(string adapterId)
        {
            var adapterEntry = _context.GT_AdapterInfo
                .FirstOrDefault(t => t.Id.ToString() == adapterId);
            if (adapterEntry == null) return "-1";

            //删除适配器对应的爬取范围           
            var spiderScopes = _context.GT_SpiderScope
                .Where(t => t.AdapterId.ToString() == adapterId)
                .ToList();
            foreach (var spiderScope in spiderScopes)
                _context.GT_SpiderScope.Remove(spiderScope);

            //删除适配器对应的任务计划
            var plan = _context.GT_PlanInfo
                .FirstOrDefault(t => t.AdapterId.ToString() == adapterId);
            if (plan != null)
                _context.GT_PlanInfo.Remove(plan);

            //更新执行日志
            var taskLogs = _context.GT_TaskLogInfo
                .Where(t => t.AdapterId.ToString() == adapterId)
                .ToList();
            foreach (var log in taskLogs)
                log.AdapterName += "(已反注册)";
            
            //删除适配器信息
            _context.GT_AdapterInfo.Remove(adapterEntry);
            //保存修改
            _context.SaveChanges();

            // 反注册时同时去掉已注册的数据节点授权
            var dataNodes = _dataNodeDbContext.API_DATA_NODE_INFO
                                              .Where(t => t.DataNodeID == adapterId);
            if (dataNodes.Any())
            {
                var apiDataNodeInfo = dataNodes.FirstOrDefault();
                if (apiDataNodeInfo != null)
                {
                    var dataId = apiDataNodeInfo.DataID;
                    // 去掉数据节点授权相关信息
                    var dataRelations = _dataNodeDbContext.API_DATA_RELATION.Where(t => t.DataID == dataId);
                    if (dataRelations.Any())
                    {
                        foreach (var dataRelation in dataRelations)
                        {
                            _dataNodeDbContext.API_DATA_RELATION.Remove(dataRelation);
                        }
                    }
                }
                _dataNodeDbContext.API_DATA_NODE_INFO.Remove(dataNodes.FirstOrDefault());
                _dataNodeDbContext.SaveChanges();
            }
            return adapterEntry?.AdapterURL;
        }

        /// <summary>
        /// 获得重复的DataType列表
        /// </summary>
        /// <returns></returns>
        public List<SpiderScopeModel> GetDataTypeDuplicateList()
        {
            var dataTypeDuplicateList = (from t in _context.GT_SpiderScope
                         group t by t.SpiderScope into g
                         where g.Count() >= 2
                         select g.Key).ToList();

            List<SpiderScopeModel> data = new List<SpiderScopeModel>();
            foreach (var item in dataTypeDuplicateList)
            {
                SpiderScopeModel newitem = new SpiderScopeModel {SpiderScope = item};
                data.Add(newitem);
            }
            return data;
        }

        /// <summary>
        /// 通过选择的数据类型获得适配器列表
        /// </summary>
        /// <param name="adapterSpiderScope">数据类型</param>
        /// <returns></returns>
        public List<SpiderScopeModel> GetAdapterInfoListFromDataType(string adapterSpiderScope)
        {
            var data = (from t1 in _context.GT_SpiderScope
                       join t2 in _context.GT_AdapterInfo on t1.AdapterId equals t2.Id
                       select new SpiderScopeModel{
                           AdapterId = t1.AdapterId,
                           AdapterName = t2.AdapterName,
                           SpiderScope = t1.SpiderScope,
                           Priority = t1.Priority.Value
                       })                       
                       .Where(t => t.SpiderScope == adapterSpiderScope)
                       .OrderBy(t => t.Priority)
                       .ToList();
            for (int i = 0; i < data.Count; i++)
            {                
                var adapterId = data[i].AdapterId;
                var dataType = data[i].SpiderScope;
                var entry = _context.GT_SpiderScope
                    .FirstOrDefault(t => t.AdapterId == adapterId && t.SpiderScope == dataType);
                if (entry != null) entry.Priority = i + 1;
            }
            _context.SaveChanges();
            return data;                        
        }

        /// <summary>
        /// 交换相同数据类型适配器的优先级
        /// </summary>
        /// <param name="spiderScope">数据类型</param>
        /// <param name="adapterId1">适配器1ID</param>
        /// <param name="adapterId2">适配器2ID</param>
        public void ExchangeAdapterTypePriority(string spiderScope, string adapterId1, string adapterId2)
        {
            var exchangePriority = 0;
            var entry1 =_context.GT_SpiderScope
                .FirstOrDefault(t => t.AdapterId.ToString() == adapterId1 && t.SpiderScope == spiderScope);
            var entry2 = _context.GT_SpiderScope
                .FirstOrDefault(t => t.AdapterId.ToString() == adapterId2 && t.SpiderScope == spiderScope);
            if (entry1?.Priority != null)
            {
                exchangePriority = entry1.Priority.Value;
                if (entry2?.Priority != null) entry1.Priority = entry2.Priority.Value;
            }
            if (entry2 != null) entry2.Priority = exchangePriority;
            _context.SaveChanges();
        }
    }
}
