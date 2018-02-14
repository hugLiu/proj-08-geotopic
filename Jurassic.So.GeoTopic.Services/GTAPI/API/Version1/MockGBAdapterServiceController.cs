using System.Threading.Tasks;
using System.Web.Http;
using Jurassic.So.Infrastructure;
using Jurassic.So.Business;
using Jurassic.PKS.WebAPI.Models.Adapter;
using Jurassic.PKS.WebAPI.Models;
using Jurassic.PKS.Service.Data;
using Jurassic.PKS.Service.Adapter;
using System.Web;
using System.Configuration;
using System.ComponentModel.DataAnnotations;
using Jurassic.PKS.Service;

namespace GTAPI.API.Version1
{
    /// <summary>MockGB适配器服务API</summary>
    public class MockGBAdapterServiceController : AdapterServiceBaseController
    {
        /// <summary>构造函数</summary>
        public MockGBAdapterServiceController(IAdapter adapterService) : base(adapterService) { }
        /// <summary>分批或增量爬取某个域的成果的元数据集合</summary>
        /// <param name="request">爬取成果的元数据集合请求数据</param>
        /// <returns>爬取结果</returns>
        [HttpPost]
        public override async Task<SpiderResult> Spider(AdapterSpiderRequest request)
        {
            return await this.AdapterService.SpiderAsync(request.Scope, request.IncrementValue, request.Pager.GetPagerOrDefault());
        }
    }
}
