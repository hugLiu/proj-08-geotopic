using Jurassic.So.SpiderTool.IService.Processers;

namespace Jurassic.So.SpiderTool.IService
{
    public interface ITaskService
    {
        /// <summary>
        /// 不带进度条的任务执行
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="adapterId">适配器Id</param>
        void TaskRun(string userName, string adapterId);

        /// <summary>
        /// 带进度条的任务执行
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="adapterId">适配器Id</param>
        void ProgressTaskRun(string userName, string adapterId);

        /// <summary>
        /// 带进度条的任务执行所有爬取范围
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="adapterId">适配器Id</param>
        void ProgressTaskRunAll(string userName, string adapterId);

        /// <summary>
        /// 带进度条执行指定爬取范围的任务
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="adapterId">适配器Id</param>
        /// <param name="scope">爬取范围</param>
        void ProgressTaskRunByScope(string userName, string adapterId, string scope);

        /// <summary>
        /// 带进度条执行指定状态的任务
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="adapterId">适配器Id</param>
        /// <param name="status">进程状态</param>
        void ProgressTaskRunByStatus(string userName, string adapterId, ProcessStatus status);
    }
}
