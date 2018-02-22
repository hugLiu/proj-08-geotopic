using System;
using System.Threading.Tasks;
using Jurassic.PKS.Service;
using Jurassic.PKS.Service.Adapter;
using Jurassic.PKS.WebAPI.Models.Adapter;
using Jurassic.So.Business;
using Jurassic.So.Infrastructure;
using Newtonsoft.Json.Linq;

namespace Jurassic.So.SpiderTool.Service
{
    public class WrapedAdapterService : AdapterBase
    {
        /// <summary>构造函数</summary>
        static WrapedAdapterService()
        {
            s_HttpClient = new HttpClientWrapper();
            //配置
            s_HttpClient.Instance.Timeout = TimeSpan.FromSeconds(60);
        }
        /// <summary>构造函数</summary>
        public WrapedAdapterService(string url)
        {
            this.Url = url.Trim().TrimEnd('/');
        }

        /// <summary>Http客户端包装器</summary>
        private static HttpClientWrapper s_HttpClient { get; set; }
        /// <summary>服务URL</summary>
        private string Url { get; set; }

        /// <summary>获得适配器服务能力信息URL</summary>
        private string Url_GetServiceCapabilities
        {
            get { return this.Url + "/GetCapabilities"; }
        }
        /// <summary>适配器信息</summary>
        public override AdapterInfo GetAdapterInfo()
        {
            return GetAdapterInfoAsync().Result;
        }
        /// <summary>获得适配器信息</summary>
        public override async Task<AdapterInfo> GetAdapterInfoAsync()
        {
            var result = await s_HttpClient.GetAsync<AdapterServiceCapabilities>(this.Url_GetServiceCapabilities).ConfigureAwait(false);
            return result.Adapter;
        }

        /// <summary>爬取URL</summary>
        private string Url_Spider
        {
            get { return this.Url + "/Spider"; }
        }
        /// <summary>分批或增量爬取某个适配器域的成果的元数据集合</summary>
        /// <param name="scope">某个适配器域</param>
        /// <param name="incrementValue">增量值，允许为空，如果有值必须是符合该域的增量类型的值</param>
        /// <param name="pager">分页参数，允许为null，为null表示爬取全部，否则只爬取某页数据</param>
        /// <returns>爬取结果</returns>
        public virtual async Task<SpiderResult> SpiderAsync(string scope, string incrementValue, Pager pager)
        {
            var request = new AdapterSpiderRequest();
            request.Scope = scope;
            request.IncrementValue = incrementValue;
            request.Pager = pager;
            return await s_HttpClient.PostAsync<SpiderResult>(this.Url_Spider, request.ToJson()).ConfigureAwait(false);
        }

        /// <summary>获取内容项URL</summary>
        private string Url_Retrieve
        {
            get { return this.Url + "/Retrieve"; }
        }
        /// <summary>根据域和成果键获取成果的内容项集合</summary>
        /// <param name="scope">某个适配器域</param>
        /// <param name="natureKey">成果键</param>
        /// <returns>成果的内容项集合</returns>
        public override async Task<DataSchemaCollection> RetrieveAsync(string scope, string natureKey)
        {
            var request = new AdapterRetrieveRequest();
            request.Scope = scope;
            request.NatureKey = natureKey;
            var queryParams = JObject.FromObject(request).JsonToDictionary();
            var result = await s_HttpClient.GetAsync<DataSchema[]>(this.Url_Retrieve, queryParams).ConfigureAwait(false);
            return new DataSchemaCollection(result);
        }

        /// <summary>获取数据项URL</summary>
        private string Url_GetData
        {
            get { return this.Url + "/GetData"; }
        }
        /// <summary>根据数据项票据获取成果的数据项</summary>
        /// <param name="ticket">成果的数据项票据</param>
        /// <param name="pager">分页参数</param>
        /// <returns>成果的数据项结果</returns>
        public override async Task<DataResult> GetDataAsync(string ticket, Pager pager)
        {
            var request = new AdapterGetDataRequest();
            request.Ticket = ticket;
            if (pager != null)
            {
                request.From = pager.From;
                request.Size = pager.Size;
            }
            var queryParams = JObject.FromObject(request).JsonToDictionary();
            return await s_HttpClient.GetAsync<DataResult>(this.Url_GetData, queryParams).ConfigureAwait(false);
        }
    }
}
