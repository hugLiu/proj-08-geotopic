using Jurassic.So.SpiderTool.IService.ViewModel;
using System;
using System.Collections.Generic;

namespace Jurassic.So.SpiderTool.IService
{
    public interface ITaskLogInfoService
    {
        /// <summary>
        /// 清空所有的记录
        /// </summary>
        void ClearTaskLog();

        /// <summary>
        /// 获取日志详情
        /// </summary>
        /// <param name="adapterTaskLogIdentity"></param>
        /// <param name="spiderScope"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        List<TaskLogInfoModel> GetTaskLogDetail(string adapterTaskLogIdentity, string spiderScope, int state);

        /// <summary>
        /// 获取某一爬取范围的所有爬取日志
        /// </summary>
        /// <param name="adaterId"></param>
        /// <param name="spiderScope"></param>
        /// <returns></returns>
        List<TaskLogInfoModel> GetTaskLogByScope(string adaterId, string spiderScope, int state);

            /// <summary>
        /// 日志的查询
        /// </summary>
        /// <param name="adapterName">适配器名称</param>
        /// <param name="startDate">开始执行日期</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <returns></returns>
        List<TaskLogIndexModel> GetAdapterTaskLog(string adapterName, ref int totalCount, DateTime? startDate = null,
            int pageIndex = 1, int pageSize = int.MaxValue);

    }
}
