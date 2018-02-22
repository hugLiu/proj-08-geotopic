using System;
using Jurassic.So.SpiderTool.IService;
using Jurassic.So.SpiderTool.Service.Processer;
using System.Linq;
using Jurassic.So.Data.Entities;
using Jurassic.So.SpiderTool.IService.Processers;
using Jurassic.So.SpiderTool.Service.Util;

namespace Jurassic.So.SpiderTool.Service
{
    public class TaskService : ITaskService
    {
        public DataServiceDBContext _context { get; set; }

        public TaskService()
        {
            _context = new DataServiceDBContext();
        }

        /// <summary>
        /// 不带进度条的任务执行
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="adapterId"></param>
        public void TaskRun(string userName, string adapterId)
        {            
            var adapterInfo = _context.GT_AdapterInfo
                                    .Include("GT_SpiderScope")
                                    .FirstOrDefault(t => t.Id.ToString() == adapterId);
            if (adapterInfo == null) return;

            ProcesserFactory fac = new ProcesserFactory();
            SpiderTaskProcesser spiderTask = new SpiderTaskProcesser();
            fac.TaskFinished += ScheduleFinished;
            fac.Register(adapterId, spiderTask);
            foreach (var spiderScope in adapterInfo
                                    .GT_SpiderScope)
            {
                spiderTask.SpiderPercent.Add(spiderScope.SpiderScope, 0);
                spiderTask.ExecuteStatus.Add(spiderScope.SpiderScope, ProcessStatus.Preparing);
                spiderTask.ExecuteTimes.Add(spiderScope.SpiderScope, 0);
                spiderTask.ExecuteStartTime.Add(spiderScope.SpiderScope, "");
                spiderTask.ExecuteDuration.Add(spiderScope.SpiderScope, "");
                fac.Add(adapterId, new SpiderParam(userName, spiderScope));
            }
            fac.Start(adapterId);
        }

        private void ScheduleFinished(object sender, EventArgs e)
        {
            var adapterId =  sender as string;
            if (adapterId == null) return;
            var planInfo = _context.GT_PlanInfo
                .FirstOrDefault(t => t.AdapterId.ToString() == adapterId);
            if (planInfo == null) return;
            try
            {
                planInfo.RunTimes += 1;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Log.WriteError("更新任务计划执行次数出错", ex);
                throw ex;
            }           
        }

        /// <summary>
        /// 带进度条的任务执行
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="adapterId"></param>
        public void ProgressTaskRun(string userName, string adapterId)
        {
            var adapterInfo = _context.GT_AdapterInfo
                .Include("GT_SpiderScope")
                .FirstOrDefault(t => t.Id.ToString() == adapterId);
            if (adapterInfo == null) return;

            SpiderTaskProcesser spiderTask = new SpiderTaskProcesser();
            SignalRProcesserFactory.Instance.Register(adapterId, spiderTask);
            foreach (var spiderScope in adapterInfo
                .GT_SpiderScope
                .Where(t => t.AsTask != null && t.AsTask.Value))
            {
                spiderTask.SpiderPercent.Add(spiderScope.SpiderScope, 0);
                spiderTask.ExecuteStatus.Add(spiderScope.SpiderScope, ProcessStatus.Preparing);
                spiderTask.ExecuteTimes.Add(spiderScope.SpiderScope, 0);
                spiderTask.ExecuteStartTime.Add(spiderScope.SpiderScope, "");
                spiderTask.ExecuteDuration.Add(spiderScope.SpiderScope, "");
                SignalRProcesserFactory.Instance.Add(adapterId, new SpiderParam(userName, spiderScope));               
            }
            SignalRProcesserFactory.Instance.Start(adapterId);
        }

        /// <summary>
        /// 带进度条的任务执行所有爬取范围
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="adapterId">适配器Id</param>
        public void ProgressTaskRunAll(string userName, string adapterId)
        {
            var adapterInfo = _context.GT_AdapterInfo
                .Include("GT_SpiderScope")
                .FirstOrDefault(t => t.Id.ToString() == adapterId);
            if (adapterInfo == null) return;

            SpiderTaskProcesser spiderTask = new SpiderTaskProcesser();
            SignalRProcesserFactory.Instance.Register(adapterId, spiderTask);
            foreach (var spiderScope in adapterInfo
                .GT_SpiderScope)
            {
                spiderTask.SpiderPercent.Add(spiderScope.SpiderScope, 0);
                spiderTask.ExecuteStatus.Add(spiderScope.SpiderScope, ProcessStatus.Preparing);
                spiderTask.ExecuteTimes.Add(spiderScope.SpiderScope, 0);
                spiderTask.ExecuteStartTime.Add(spiderScope.SpiderScope, "");
                spiderTask.ExecuteDuration.Add(spiderScope.SpiderScope, "");
                SignalRProcesserFactory.Instance.Add(adapterId, new SpiderParam(userName, spiderScope));
            }
            SignalRProcesserFactory.Instance.Start(adapterId);
        }

        /// <summary>
        /// 带进度条执行指定爬取范围的任务
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="adapterId"></param>
        /// <param name="scope"></param>
        public void ProgressTaskRunByScope(string userName, string adapterId, string scope)
        {
            var spiderScope =
                _context.GT_SpiderScope.FirstOrDefault(t => t.AdapterId.ToString() == adapterId && t.SpiderScope == scope);
            if (spiderScope == null) return;

            SpiderTaskProcesser spiderTask = new SpiderTaskProcesser();
            spiderTask.SpiderPercent.Add(spiderScope.SpiderScope, 0);
            spiderTask.ExecuteStatus.Add(spiderScope.SpiderScope, ProcessStatus.Preparing);
            spiderTask.ExecuteTimes.Add(spiderScope.SpiderScope, 0);
            spiderTask.ExecuteStartTime.Add(spiderScope.SpiderScope, "");
            spiderTask.ExecuteDuration.Add(spiderScope.SpiderScope, "");
            SignalRProcesserFactory.Instance.Register(adapterId + scope, spiderTask);
            SignalRProcesserFactory.Instance.Add(adapterId + scope, new SpiderParam(userName, spiderScope));
            SignalRProcesserFactory.Instance.Start(adapterId + scope);
        }

        public void ProgressTaskRunByStatus(string userName, string adapterId, ProcessStatus processStatus)
        {
            var adapterInfo = _context.GT_AdapterInfo
                .Include("GT_SpiderScope")
                .FirstOrDefault(t => t.Id.ToString() == adapterId);
            if (adapterInfo == null) return;

            SpiderTaskProcesser spiderTask = new SpiderTaskProcesser();
            SignalRProcesserFactory.Instance.Register(adapterId, spiderTask);
            foreach (var spiderScope in adapterInfo
                .GT_SpiderScope
                .Where(t => t.LastSpiderStatus == (int?)processStatus))
            {
                spiderTask.SpiderPercent.Add(spiderScope.SpiderScope, 0);
                spiderTask.ExecuteStatus.Add(spiderScope.SpiderScope, ProcessStatus.Preparing);
                spiderTask.ExecuteTimes.Add(spiderScope.SpiderScope, 0);
                spiderTask.ExecuteStartTime.Add(spiderScope.SpiderScope, "");
                spiderTask.ExecuteDuration.Add(spiderScope.SpiderScope, "");
                SignalRProcesserFactory.Instance.Add(adapterId, new SpiderParam(userName, spiderScope));
            }
            SignalRProcesserFactory.Instance.Start(adapterId);
        }


    }
}
