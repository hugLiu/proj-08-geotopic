using System.Collections.Generic;
using Jurassic.So.SpiderTool.IService.ViewModel;

namespace Jurassic.So.SpiderTool.IService
{
    public interface IAdapterInfoService
    {
        /// <summary>
        /// 分页获取本地适配器信息
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        List<AdapterInfoModel> GetAdapterPageInfos(int pageIndex, int pageSize, ref int total);

        /// <summary>
        /// 更新适配器对应的分批爬取大小
        /// </summary>
        /// <param name="adapterId">适配器Id</param>
        /// <param name="spiderSize">分批大小</param>
        void UpdateSpiderSize(string adapterId, int spiderSize);

        /// <summary>
        /// 更新爬取范围选中状态
        /// </summary>
        /// <param name="adapterId">适配器Id</param>
        /// <param name="scopes">爬取范围集合</param>
        void UpdateSpiderScopeSelected(string adapterId, List<string> scopes);

        /// <summary>
        /// 更新爬取范围选中状态 全选
        /// </summary>
        /// <param name="adapterId"></param>
        void UpdateSpiderScopeSelectAll(string adapterId);

        /// <summary>
        /// 获取适配下具体的爬取范围，并实现爬取范围条件过滤
        /// </summary>
        /// <param name="adapterId">适配器ID</param>
        /// <param name="spiderScope">爬取范围</param>
        /// <returns></returns>
        List<SpiderScopeModel> GetSpiderScopeList(string adapterId, string spiderScope);
    }
}
