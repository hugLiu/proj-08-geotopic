using Jurassic.PKS.Service.Adapter;
using Jurassic.PKS.WebAPI.Models.Data;
using Jurassic.So.Data;
using Jurassic.So.Data.Center;

namespace Jurassic.So.SpiderTool.IService
{
    public interface IWrapedDataService
	{
		/// <summary>
		/// 获取数据服务的基本信息
		/// </summary>
		/// <returns>数据服务的基本信息(JSON格式)</returns>
		string GetServiceInfo();

        /// <summary>
        /// 获得服务能力数据
        /// </summary>
        /// <returns></returns>
        string GetServiceCapabilities();

        /// <summary>
        /// 爬取获得信息项的各类索引信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
	    SpiderResult Spider(SpiderRequest request);
	}
}
