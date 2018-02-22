using Ninject;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using Jurassic.So.SpiderTool.IService;
using Jurassic.So.SpiderTool.IService.ViewModel;
using Jurassic.So.SpiderTool.Service.Job;
using Jurassic.So.SpiderTool.Service.Util;

namespace Jurassic.So.SpiderTool.Service
{
    /// <summary>
    /// 数据爬取任务执行调度入口
    /// </summary>
    public class SpiderSchedule
    {
        private IScheduler _scheduler = null;
        [Inject]
        public IPlanService PlanService { get; set; }

        /// <summary>
        /// 将所有计划加入到调度，并启动调度器
        /// </summary>
        public void SchedulePlans()
        {
            ISchedulerFactory sf = new StdSchedulerFactory();
            _scheduler = sf.GetScheduler();

            List<SchedulePlan> plans = PlanService.GetPlans(false);
            foreach (SchedulePlan p in plans)
            {
                ScheduleJob(p);
            }
            if (!_scheduler.IsStarted)
            {
                _scheduler.Start();
            }
        }
        /// <summary>
        /// 更新调度器中的计划
        /// </summary>
        /// <param name="p"></param>
        public void UpdatePlanSchedule(SchedulePlan p)
        {
            if (_scheduler == null)
            {
                return;
            }
            JobKey jobkey = new JobKey(Constants.Plan + p.Id, p.PlanName);
            IList<IJobExecutionContext> jobs = _scheduler.GetCurrentlyExecutingJobs();
            if (_scheduler.CheckExists(jobkey))
            {
                _scheduler.DeleteJob(jobkey);
            }
            ScheduleJob(p);
        }

        /// <summary>
        /// 删除调度器的计划
        /// </summary>
        /// <param name="p"></param>
        public void DeletePlanFromSchedule(SchedulePlan p)
        {
            if (_scheduler == null)
            {
                return;
            }
            JobKey jobkey = new JobKey(Constants.Plan + p.Id, p.PlanName);
            if (_scheduler.CheckExists(jobkey))
            {
                _scheduler.DeleteJob(jobkey);
            }
        }

        /// <summary>
        /// 将计划加入到调度器
        /// </summary>
        /// <param name="p">计划实体</param>0
        private void ScheduleJob(SchedulePlan p)
        {
            if (PlanUtil.IsPlanCanBeSchedule(p))
            {
                try
                {
                    JobDataMap map = new JobDataMap();
                    map.Add(Constants.Plan, p);

                    IJobDetail detail = JobBuilder
                                .Create()
                                .OfType(typeof(SpiderJob))
                                .WithDescription(p.Remark)
                                .WithIdentity(new JobKey(Constants.Plan + p.Id, p.PlanName))
                                .UsingJobData(map)
                                .Build();
                    ITrigger trigger = ScheduleContext.GetContext()
                        .GetTriggerProcessor(p)
                        .CreateCronTrigger();

                    if (trigger != null)
                    {
                        _scheduler.ScheduleJob(detail, trigger);
                        DateTime date = trigger.GetNextFireTimeUtc().GetValueOrDefault().AddHours(8).DateTime;
                        p.NextRunDate = date;
                        PlanService.UpdatePlan(p);
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteError("将计划加入到调度器出错", ex);
                }

            }
        }
        public void CloseScheduler()
        {
            if (_scheduler != null && _scheduler.IsStarted
                && !_scheduler.IsShutdown)
            {
                _scheduler.Shutdown(false);
            }
        }
    }
}
