using System;
using System.Configuration;
using System.Threading.Tasks;
using Jurassic.PKS.Service;
using Jurassic.PKS.Service.Index;
using Jurassic.PKS.WebAPI.Models.Index;
using Jurassic.So.Business;
using Jurassic.So.SpiderTool.IService;

namespace Jurassic.So.SpiderTool.Service
{
    class WrapedIndexerService : IWrapedIndexerService
    {
        /// <summary>构造函数</summary>
        static WrapedIndexerService()
        {
            s_HttpClient = new HttpClientWrapper();
            //配置
            s_HttpClient.Instance.Timeout = TimeSpan.FromSeconds(60);
        }

        /// <summary>Http客户端包装器</summary>
        private static HttpClientWrapper s_HttpClient { get; set; }

        public string GetServiceInfo()
        {
            return string.Empty;
        }

        public IndexResult SendIndex(IndexInfoRequest request)
        {
            return SendIndexAsync(request).Result;
        }

        public async Task<IndexResult> SendIndexAsync(IndexInfoRequest request)
        {
            string api = ConfigurationManager.AppSettings["SendIndex"];
            return await s_HttpClient.PostAsync<IndexResult>(api, request.ToJson()).ConfigureAwait(false);
        }
    }
}
