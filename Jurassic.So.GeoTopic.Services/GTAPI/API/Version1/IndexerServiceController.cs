using System.Threading.Tasks;
using System.Web.Http;
using Jurassic.So.Infrastructure;
using Jurassic.PKS.Service.Index;
using Jurassic.PKS.WebAPI.Models;
using Jurassic.PKS.WebAPI.Models.Index;
using Jurassic.PKS.Service;

namespace GTAPI.API.Version1
{
    /// <summary>索引服务API</summary>
    public class IndexerServiceController : WebAPIController
    {
        /// <summary>索引服务</summary>
        private IIndexer IndexerService { get; set; }
        /// <summary>构造函数</summary>
        public IndexerServiceController(IIndexer indexerService)
        {
            this.IndexerService = indexerService;
        }
        /// <summary>获得服务信息</summary>
        protected override ServiceInfo GetServiceInfo()
        {
            return new ServiceInfo()
            {
                Description = "索引服务主要用于将元数据推送到元数据库中"
            };
        }
        /// <summary>批量保存/更新/删除索引信息</summary>
        /// <param name="request">索引操作信息请求数据</param>
        /// <returns>索引操作结果</returns>
        [HttpPost]
        public async Task<IndexResult> SendIndex(IndexInfoRequest request)
        {
            return await this.IndexerService.SendIndexAsync(request.MapTo<IndexInfo>());
        }
    }
}
