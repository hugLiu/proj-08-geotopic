using System;
using System.Threading.Tasks;
using Jurassic.PKS.Service;
using Jurassic.PKS.Service.Adapter;
using Jurassic.PKS.WebAPI.Models.Data;
using Jurassic.So.Business;
using Jurassic.So.Infrastructure;
using Newtonsoft.Json.Linq;

namespace Jurassic.So.Data.Center
{
    /// <summary>API包装数据服务</summary>
    public class ApiWrappedDataService
    {
        /// <summary>构造函数</summary>
        static ApiWrappedDataService()
        {
            HttpClient = new HttpClientWrapper();
            //配置
            HttpClient.Instance.Timeout = TimeSpan.FromSeconds(60);
        }
        /// <summary>构造函数</summary>
        public ApiWrappedDataService(string url)
        {
            this.ServiceUrl = url.Trim().TrimEnd('/');
        }
        /// <summary>Http客户端包装器</summary>
        public static HttpClientWrapper HttpClient { get; private set; }
        /// <summary>服务URL</summary>
        private string ServiceUrl { get; set; }
        /// <summary>获得服务能力信息URL</summary>
        public string Url_GetCapabilities
        {
            get { return this.ServiceUrl + "/GetCapabilities"; }
        }
        /// <summary>获得服务能力信息</summary>
        public DataServiceCapabilities GetCapabilities()
        {
            return GetCapabilitiesAsync().Result;
        }
        /// <summary>获得服务能力信息</summary>
        public async Task<DataServiceCapabilities> GetCapabilitiesAsync()
        {
            return await HttpClient.GetAsync<DataServiceCapabilities>(this.Url_GetCapabilities).ConfigureAwait(false);
        }
        /// <summary>爬取URL</summary>
        public string Url_Spider
        {
            get { return this.ServiceUrl + "/Spider"; }
        }
        /// <summary>分批或增量爬取某个适配器域的成果的元数据集合</summary>
        public SpiderResult Spider(SpiderRequest request)
        {
            return SpiderAsync(request).Result;
        }
        /// <summary>分批或增量爬取某个适配器域的成果的元数据集合</summary>
        public async Task<SpiderResult> SpiderAsync(SpiderRequest request)
        {
            var result = await HttpClient.PostAsync<string>(this.Url_Spider, request.ToJson()).ConfigureAwait(false);
            return JsonMetadata.JsonTo<SpiderResult>(result);
        }
        /// <summary>获取内容项URL</summary>
        public string Url_Retrieve
        {
            get { return this.ServiceUrl + "/Retrieve"; }
        }
        /// <summary>根据域和成果键获取成果的内容项集合</summary>
        public DataSchemaCollection Retrieve(RetrieveRequest request)
        {
            return RetrieveAsync(request).Result;
        }
        /// <summary>根据域和成果键获取成果的内容项集合</summary>
        public async Task<DataSchemaCollection> RetrieveAsync(RetrieveRequest request)
        {
            var queryParams = JObject.FromObject(request).JsonToDictionary();
            return await HttpClient.GetAsync<DataSchemaCollection>(this.Url_Retrieve, queryParams).ConfigureAwait(false);
        }
        /// <summary>获取数据项URL</summary>
        public string Url_GetData
        {
            get { return this.ServiceUrl + "/GetData"; }
        }
        /// <summary>根据数据项票据获取成果的数据项</summary>
        public DataResult GetData(GetDataRequest request)
        {
            return GetDataAsync(request).Result;
        }
        /// <summary>根据数据项票据获取成果的数据项</summary>
        public async Task<DataResult> GetDataAsync(GetDataRequest request)
        {
            var queryParams = JObject.FromObject(request).JsonToDictionary();
            return await HttpClient.GetAsync<DataResult>(this.Url_GetData, queryParams).ConfigureAwait(false);
        }
    }
}
