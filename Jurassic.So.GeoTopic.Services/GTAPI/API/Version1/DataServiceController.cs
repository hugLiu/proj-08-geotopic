using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Jurassic.So.Infrastructure;
using Jurassic.So.Business;
using Jurassic.PKS.Service.Data;
using Jurassic.PKS.WebAPI.Models.Data;
using Jurassic.PKS.Service.Adapter;
using Jurassic.PKS.WebAPI.Models;
using Jurassic.PKS.Service;

namespace GTAPI.API.Version1
{
    /// <summary>数据服务API</summary>
    public class DataServiceController : WebAPIController
    {
        /// <summary>数据服务</summary>
        private IData DataService { get; set; }
        /// <summary>构造函数</summary>
        public DataServiceController(IData dataService)
        {
            this.DataService = dataService;
        }
        /// <summary>获得服务信息</summary>
        protected override ServiceInfo GetServiceInfo()
        {
            return new ServiceInfo()
            {
                Description = "数据服务主要用于从已注册的多个适配器源获得成果的元数据、内容项和数据项"
            };
        }
        /// <summary>获得服务能力数据</summary>
        protected override async Task<ServiceCapabilities> GetServiceCapabilities()
        {
            var adapters = await this.DataService.GetAdapterInfosAsync();
            return new DataServiceCapabilities() { Adapters = adapters };
        }
        /// <summary>分批或增量爬取某个适配器的某个域的成果的元数据集合</summary>
        /// <param name="request">爬取成果的元数据集合请求数据</param>
        /// <returns>爬取结果</returns>
        [HttpPost]
        public async Task<SpiderResult> Spider(SpiderRequest request)
        {
            return await this.DataService.SpiderAsync(request.AdapterId, request.Scope, request.Pager.GetPagerOrDefault(), request.IncrementValue);
        }
        /// <summary>获取成果的内容项集合</summary>
        /// <param name="request">获取成果的内容项集合请求数据</param>
        /// <returns>成果的内容项集合</returns>
        [HttpGet]
        public async Task<DataSchemaCollection> Retrieve([FromUri]RetrieveRequest request)
        {
            var url = HttpUtility.UrlDecode(request.Url);
            return await this.DataService.RetrieveAsync(url);
        }
        /// <summary>获取成果的数据项</summary>
        /// <param name="request">获取成果的数据项请求数据</param>
        /// <returns>成果的数据项结果</returns>
        [HttpGet]
        public async Task<IHttpActionResult> GetData([FromUri]GetDataRequest request)
        {
            var url = HttpUtility.UrlDecode(request.Url);
            var result = await this.DataService.GetDataAsync(url, request.Ticket, request.GetPagerOrDefault());
            var mimeTypeAttribute = result.Format.ToMimeType();
            if (mimeTypeAttribute.IsStreamOutput)
            {
                if (!ApiStreamResult.CanConvert(result.Value))
                {
                    DataExceptionCode.InvalidDataFormat.ThrowUserFriendly("数据内容格式应是字节流或字节数组！", "无效的数据内容格式！");
                }
                return new ApiStreamResult(mimeTypeAttribute.Value, result.Value, this);
            }
            return new ApiTextResult(mimeTypeAttribute.Value, result.Value, this);
        }
    }
}
