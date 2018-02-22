using Jurassic.PKS.Service.Index;
using Jurassic.PKS.WebAPI.Models.Index;

namespace Jurassic.So.SpiderTool.IService
{
    public interface IWrapedIndexerService
    {
        /// <summary>
        /// 获得服务信息
        /// </summary>
        /// <returns></returns>
        string GetServiceInfo();

        /// <summary>
        /// 保存/更新/删除索引信息
        /// </summary>
        /// <param name="request">索引操作信息请求数据</param>
        /// <returns>索引操作结果</returns>
        IndexResult SendIndex(IndexInfoRequest request);
    }
}
