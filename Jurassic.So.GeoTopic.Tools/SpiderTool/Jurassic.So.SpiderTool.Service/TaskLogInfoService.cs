using Jurassic.So.SpiderTool.IService;
using Jurassic.So.SpiderTool.IService.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Jurassic.So.Data.Entities;
using Jurassic.So.Infrastructure;

namespace Jurassic.So.SpiderTool.Service
{
    public class TaskLogInfoService : ITaskLogInfoService
    {
        private DataServiceDBContext _context { get; set; }

        public TaskLogInfoService()
        {
            _context = new DataServiceDBContext();
        }

        public void ClearTaskLog()
        {
            _context.GT_TaskLogInfo.RemoveRange(_context.GT_TaskLogInfo);
            _context.SaveChanges();
        }

        /// <summary>
        /// 获取数据类型的详细日志信息  
        /// </summary>
        /// <param name="adapterTaskLogIdentity"></param>
        /// <param name="spiderScope"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public List<TaskLogInfoModel> GetTaskLogDetail(string adapterTaskLogIdentity, string spiderScope, int state)
        {
            var data = _context
                .GT_TaskLogInfo
                .OrderByDescending(t => t.StartDate)
                .Where(t => t.AdapterTaskLogIdentity == adapterTaskLogIdentity && t.Deleted == false)
                .Select(t => new TaskLogInfoModel
                {
                    AdapterId = t.AdapterId.ToString(),
                    SpiderScope = t.SpiderScope,
                    StartDate = t.StartDate,
                    EndDate = t.EndDate,
                    FailturReason = t.FailturReason,
                    FailureTime = t.FailureTime,
                    IsInvalid = t.IsInvalid,
                    OrderIndex = t.OrderIndex,
                    Performer = t.Performer,
                    RecordCount = t.RecordCount,
                    Success = t.Success
                });

            if (!string.IsNullOrEmpty(spiderScope))
            {
                data = data.Where(t => t.SpiderScope.Contains(spiderScope));
            }
            switch (state)
            {
                case 2:
                    data = data.Where(t => t.Success.Value);
                    break;
                case 3:
                    data = data.Where(t => !t.Success.Value);
                    break;
            }
            return data.ToList();
        }

        public List<TaskLogInfoModel> GetTaskLogByScope(string adaterId, string spiderScope, int state)
        {
            var data = _context
                .GT_TaskLogInfo
                .Where(t => t.AdapterId.ToString() ==adaterId && t.SpiderScope == spiderScope && t.Deleted==false)
                .OrderByDescending(t => t.StartDate)
                .Select(t => new TaskLogInfoModel
                {
                    AdapterId = t.AdapterId.ToString(),
                    SpiderScope = t.SpiderScope,
                    StartDate = t.StartDate,
                    EndDate = t.EndDate,
                    FailturReason = t.FailturReason,
                    FailureTime = t.FailureTime,
                    IsInvalid = t.IsInvalid,
                    OrderIndex = t.OrderIndex,
                    Performer = t.Performer,
                    RecordCount = t.RecordCount,
                    Success = t.Success
                });


            switch (state)
            {
                case 2:
                    data = data.Where(t => t.Success.Value);
                    break;
                case 3:
                    data = data.Where(t => !t.Success.Value);
                    break;
            }
            return data.ToList();
        }

        /// <summary>
        /// 获取日志信息
        /// </summary>
        /// <param name="adapterName"></param>
        /// <param name="startDate"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<TaskLogIndexModel> GetAdapterTaskLog(string adapterName, ref int totalCount,
            DateTime? startDate = null, int pageIndex = 1, int pageSize = int.MaxValue)
        {
            var groupList = _context.GT_TaskLogInfo
                .GroupBy(t => t.AdapterTaskLogIdentity)
                .Select(g =>
                    new
                    {
                        AdapterTaskLogIdentity = g.Key,
                        TaskSuccessCount = g.Count(t => t.Success.Value),
                        TaskCount = g.Count(),
                        g.FirstOrDefault().Performer,
                        StartDate = g.Min(p => p.StartDate),
                        EndDate = g.Max(p => p.EndDate)
                    }
                );

            var distinctList = _context.GT_TaskLogInfo
                .Select(t =>
                    new
                    {
                        t.AdapterId,
                        t.AdapterTaskLogIdentity,
                        t.AdapterName,
                        t.DataSourceName
                    }
                )
                .OrderByDescending(t => t.AdapterTaskLogIdentity)
                .Distinct();

            var taskLogIndexModels = distinctList.Join(groupList,
                d => d.AdapterTaskLogIdentity,
                g => g.AdapterTaskLogIdentity,
                (d, g) => new {d, g}
                )
                .OrderByDescending(t => t.g.StartDate)
                .Select(r => new TaskLogIndexModel
                {
                    AdapterId = (int) r.d.AdapterId,
                    AdapterTaskLogIdentity = r.d.AdapterTaskLogIdentity,
                    AdapterName = r.d.AdapterName,
                    DataSourceName = r.d.DataSourceName,
                    Performer = r.g.Performer,
                    TaskSuccessCount = r.g.TaskSuccessCount,
                    TaskCount = r.g.TaskCount,
                    StartDate = r.g.StartDate,
                    EndDate = r.g.EndDate
                }
                );

            if (!string.IsNullOrEmpty(adapterName))
            {
                taskLogIndexModels = taskLogIndexModels.Where(t => t.AdapterName.Contains(adapterName));
            }
            if (startDate != null)
            {
                taskLogIndexModels = taskLogIndexModels.Where(t => t.StartDate == startDate);
            }
            totalCount = taskLogIndexModels.Count();
            var list = taskLogIndexModels.Skip(pageIndex*pageSize).Take(pageSize).ToList();
            return list;
        }        
    }
}
