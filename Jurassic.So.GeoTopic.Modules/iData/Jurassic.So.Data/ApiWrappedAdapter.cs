using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Jurassic.PKS.Service;
using Jurassic.PKS.Service.Adapter;
using Jurassic.PKS.WebAPI.Models.Adapter;
using Jurassic.So.Business;
using Jurassic.So.Infrastructure;
using Newtonsoft.Json.Linq;

namespace Jurassic.So.Data.Center
{
    /// <summary>API包装适配器服务</summary>
    public class ApiWrappedAdapter
    {
        /// <summary>构造函数</summary>
        static ApiWrappedAdapter()
        {
            HttpClient = new HttpClientWrapper();
            //配置
            HttpClient.Instance.Timeout = TimeSpan.FromSeconds(60);
        }
        /// <summary>构造函数</summary>
        public ApiWrappedAdapter(string url)
        {
            this.ServiceUrl = url.Trim().TrimEnd('/');
        }
        /// <summary>Http客户端包装器</summary>
        public static HttpClientWrapper HttpClient { get; private set; }
        /// <summary>服务URL</summary>
        private string ServiceUrl { get; set; }
        /// <summary>获得适配器服务能力信息URL</summary>
        public string Url_GetCapabilities
        {
            get { return this.ServiceUrl + "/GetCapabilities"; }
        }
        /// <summary>适配器信息</summary>
        public AdapterServiceCapabilities GetCapabilities()
        {
            return GetCapabilitiesAsync().Result;
        }
        /// <summary>获得适配器信息</summary>
        public async Task<AdapterServiceCapabilities> GetCapabilitiesAsync()
        {
            return await HttpClient.GetAsync<AdapterServiceCapabilities>(this.Url_GetCapabilities).ConfigureAwait(false);
        }
        /// <summary>爬取URL</summary>
        public string Url_Spider
        {
            get { return this.ServiceUrl + "/Spider"; }
        }
        public SpiderResult Spider(AdapterSpiderRequest request)
        {
            return SpiderAsync(request).Result;
        }
        /// <summary>分批或增量爬取某个适配器域的成果的元数据集合</summary>
        public async Task<SpiderResult> SpiderAsync(AdapterSpiderRequest request)
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
        public DataSchemaCollection Retrieve(AdapterRetrieveRequest request)
        {
            return RetrieveAsync(request).Result;
        }
        /// <summary>根据域和成果键获取成果的内容项集合</summary>
        public async Task<DataSchemaCollection> RetrieveAsync(AdapterRetrieveRequest request)
        {
            var queryParams = JObject.FromObject(request).JsonToDictionary();
            var result = await HttpClient.GetAsync<ApiDataSchema[]>(this.Url_Retrieve, queryParams).ConfigureAwait(false);
            return new DataSchemaCollection(result.Select(e => e.ToDataSchema()));
        }
        /// <summary>获取数据项URL</summary>
        public string Url_GetData
        {
            get { return this.ServiceUrl + "/GetData"; }
        }
        /// <summary>根据数据项票据获取成果的数据项</summary>
        public DataResult GetData(AdapterGetDataRequest request)
        {
            return GetDataAsync(request).Result;
        }
        /// <summary>根据数据项票据获取成果的数据项</summary>
        public async Task<DataResult> GetDataAsync(AdapterGetDataRequest request)
        {
            var queryParams = JObject.FromObject(request).JsonToDictionary();
            return await HttpClient.GetAsync<DataResult>(this.Url_GetData, queryParams).ConfigureAwait(false);
        }

        #region 成果的内容项
        /// <summary>成果的内容项</summary>
        [Serializable]
        [DataContract]
        private class ApiDataSchema
        {
            /// <summary>名称</summary>
            [DataMember(Name = "name")]
            public string Name { get; set; }
            /// <summary>票据</summary>
            [DataMember(Name = "ticket")]
            public string Ticket { get; set; }
            /// <summary>是否主成果</summary>
            [DataMember(Name = "major")]
            public bool Major { get; set; }
            /// <summary>格式</summary>
            [DataMember(Name = "format")]
            public string Format { get; set; }
            /// <summary>总数，-1表示总数未知</summary>
            [DataMember(Name = "total")]
            public int Total { get; set; }
            /// <summary>单位</summary>
            [DataMember(Name = "unit")]
            public string Unit { get; set; }
            /// <summary>生成JSON串</summary>
            public DataSchema ToDataSchema()
            {
                var result = new DataSchema();
                result.Name = this.Name;
                result.Ticket = this.Ticket;
                result.Major = this.Major;
                var format = this.Format.ToDataFormat();
                if (format == DataFormat.Unknown)
                {
                    format = this.Format.ToDataFormatFromMime();
                }
                result.Format = format;
                result.Total = this.Total;
                result.Unit = this.Unit;
                return result;
            }
            /// <summary>生成JSON串</summary>
            public override string ToString()
            {
                return this.ToJson();
            }
        }
        #endregion
    }
}
