using System;
using System.Configuration;
using System.Threading.Tasks;
using Jurassic.So.Business;
using Jurassic.So.SpiderTool.IService;
using Jurassic.PKS.WebAPI.Models.Data;
using Jurassic.PKS.Service.Adapter;
using Jurassic.PKS.Service;

namespace Jurassic.So.SpiderTool.Service
{
    public class WrapedDataService : IWrapedDataService
	{
        /// <summary>构造函数</summary>
        static WrapedDataService()
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

        public string GetServiceCapabilities()
        {
            return string.Empty;
        }

        public SpiderResult Spider(SpiderRequest request)
        {
            return SpiderAsync(request).Result;
        }

        public virtual async Task<SpiderResult> SpiderAsync(SpiderRequest request)
        {
            string api = ConfigurationManager.AppSettings["Spider"];
            var result = await s_HttpClient.PostAsync<string>(api, request.ToJson()).ConfigureAwait(false);
            return JsonMetadata.JsonTo<SpiderResult>(result);
        }
    }
}
