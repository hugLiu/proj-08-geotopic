using System.Threading.Tasks;
using System.Web.Http;
using Jurassic.So.Infrastructure;
using Jurassic.So.Business;
using Jurassic.PKS.WebAPI.Models.Adapter;
using Jurassic.PKS.WebAPI.Models;
using Jurassic.PKS.Service.Data;
using Jurassic.PKS.Service.Adapter;
using System.Web;
using System.Linq;
using System.Configuration;
using System.ComponentModel.DataAnnotations;
using Jurassic.PKS.Service;

namespace GTAPI.API.Version1
{
    /// <summary>适配器服务基类API</summary>
    public abstract class AdapterServiceBaseController : WebAPIController
    {
        /// <summary>数据服务</summary>
        protected IAdapter AdapterService { get; private set; }
        /// <summary>构造函数</summary>
        protected AdapterServiceBaseController(IAdapter adapterService)
        {
            this.AdapterService = adapterService;
        }
        /// <summary>获得服务信息</summary>
        protected override ServiceInfo GetServiceInfo()
        {
            return new ServiceInfo()
            {
                Description = "适配器服务主要用于从适配器源获得成果的元数据、内容项和数据项"
            };
        }
        /// <summary>获得服务能力数据</summary>
        protected override async Task<ServiceCapabilities> GetServiceCapabilities()
        {
            var adapter = await this.AdapterService.GetAdapterInfoAsync();
            return new AdapterServiceCapabilities() { Adapter = adapter };
        }
        /// <summary>分批或增量爬取某个域的成果的元数据集合</summary>
        /// <param name="request">爬取成果的元数据集合请求数据</param>
        /// <returns>爬取结果</returns>
        [HttpPost]
        public virtual async Task<SpiderResult> Spider(AdapterSpiderRequest request)
        {
            var result = await this.AdapterService.SpiderAsync(request.Scope, request.IncrementValue, request.Pager.GetPagerOrDefault());
            var metadatas = result.Metadatas.Select(e => e.ToIndex()).ToArray();
            result.Metadatas = new MetadataCollection(metadatas.Select(metadata => JsonMetadata.JsonTo<JsonMetadata>(metadata)));
            return result;
        }
        /// <summary>获取成果的内容项集合</summary>
        /// <param name="request">获取成果的内容项集合请求数据</param>
        /// <returns>成果的内容项集合</returns>
        [HttpGet]
        public async Task<DataSchemaCollection> Retrieve([FromUri]AdapterRetrieveRequest request)
        {
            return await this.AdapterService.RetrieveAsync(request.Scope, request.NatureKey);
        }
        /// <summary>获取成果的数据项</summary>
        /// <param name="request">获取成果的数据项请求数据</param>
        /// <returns>成果的数据项结果</returns>
        [HttpGet]
        public async Task<IHttpActionResult> GetData([FromUri]AdapterGetDataRequest request)
        {
            var result = await this.AdapterService.GetDataAsync(request.Ticket, request.GetPagerOrDefault());
            if (result == null)
            {
                AdapterExceptionCode.DataNotExist.ThrowUserFriendly("请求数据不存在！", "请求数据不存在！");
            }
            var mimeTypeAttribute = result.Format.ToMimeType();
            if (mimeTypeAttribute.IsStreamOutput)
            {
                if (!ApiStreamResult.CanConvert(result.Value))
                {
                    AdapterExceptionCode.InvalidDataFormat.ThrowUserFriendly("数据内容格式应是字节流或字节数组！", "无效的数据内容格式！");
                }
                return new ApiStreamResult(mimeTypeAttribute.Value, result.Value, this);
            }
            return new ApiTextResult(mimeTypeAttribute.Value, result.Value, this);
        }
    }
}
