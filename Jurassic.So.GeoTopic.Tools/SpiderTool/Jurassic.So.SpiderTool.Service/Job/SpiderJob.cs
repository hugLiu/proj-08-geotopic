using Quartz;
using Jurassic.CommonModels;
using Jurassic.Log4;
using Jurassic.AppCenter.Logs;
using Jurassic.So.SpiderTool.IService;
using Jurassic.So.SpiderTool.IService.ViewModel;
using Ninject;

namespace Jurassic.So.SpiderTool.Service.Job
{
    /// <summary>
    /// A sample job that just prints info on console for demostration purposes.
    /// </summary>
    public class SpiderJob : IJob
    {
        readonly IJLog _log = new JLogManager().GetLogger("ScheduleLogger");

        /// <summary>
        /// 任务开始执行
        /// </summary>
        /// <param name="context">Job执行上下文</param>
        public void Execute(IJobExecutionContext context)
        {
            SchedulePlan plan = context.JobDetail.JobDataMap.Get(Constants.Plan) as SchedulePlan;
            ITaskService taskService = SiteManager.Kernel.Get<ITaskService>();
            IPlanService planService = SiteManager.Kernel.Get<IPlanService>();
            taskService.TaskRun(Constants.DefaultUser, plan.AdapterId.ToString());
            plan.NextRunDate = context.Trigger.GetNextFireTimeUtc().Value.AddHours(8).DateTime;
            planService.UpdatePlan(plan);
            _log.Write(new JLogInfo()
            {
                Message = $"适配器{plan.AdapterId + plan.PlanName}运行，下次运行时间为：{plan.NextRunDate}"
            }, JLogType.Info, null);
        }
    }
}