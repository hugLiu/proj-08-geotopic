using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.PKS.Service;
using Jurassic.So.GeoTopic.DataService.Tool;
using Jurassic.So.Infrastructure.Util;
using Jurassic.WebFrame.Providers;
using Newtonsoft.Json;


namespace Jurassic.So.GeoTopic.DataService
{
    public class WrapedDataSearchService: IWrapedDataSearchService
    {

        private  Dictionary<string, string> UserTokens = new Dictionary<string, string>();

        // GET: /WebService/
        private readonly string _apiPath = System.Configuration.ConfigurationManager.AppSettings["ApiServiceURL"] + System.Configuration.ConfigurationManager.AppSettings["ApiVersion"];

        //搜索服务
        private readonly string _match = System.Configuration.ConfigurationManager.AppSettings["Match"];
        private readonly string _getMetadataDefinition = System.Configuration.ConfigurationManager.AppSettings["GetMetadataDefinition"];
        private readonly string _dataService = System.Configuration.ConfigurationManager.AppSettings["DataService"];
        private readonly string _getData = System.Configuration.ConfigurationManager.AppSettings["GetData"];
        private readonly string _searchService = System.Configuration.ConfigurationManager.AppSettings["SearchService"];
        private readonly string _retrieve = System.Configuration.ConfigurationManager.AppSettings["Retrieve"];

        private  string GetToken()
        {
            string issUser = System.Configuration.ConfigurationManager.AppSettings["ISSUser"];
            string issKey = System.Configuration.ConfigurationManager.AppSettings["ISSKey"];
            string apiUrl = System.Configuration.ConfigurationManager.AppSettings["ApiServiceURL"];
            var token = AuthorizeComm.GetAccessTokenHeaders(apiUrl, issUser, issKey);
            return token.access_token;
        }

        /// <summary>
        /// WebApp中获取Token 的统一方法
        /// </summary>
        /// <returns></returns>
        public  string GetTokenService()
        {
            string issUser = System.Configuration.ConfigurationManager.AppSettings["ISSUser"];
            if (!UserTokens.ContainsKey(issUser))
                UserTokens.Add(issUser, GetToken());
            return UserTokens[issUser];
        }

       /// <summary>
       /// 调用搜索WebAPI，获取元数据集合项
       /// </summary>
        public MetadataDefinitionCollection GetMetadataDefinition()
        {
            MetadataDefinitionCollection metadataDefDic=new MetadataDefinitionCollection();
           try
           {
                var task = WebRequestUtil.PostJson(_apiPath + _searchService + _getMetadataDefinition, string.Empty);
               //if (task.Result == null)
               //    return metadataDefDic;
                metadataDefDic = JsonUtil.JsonToObject(task.Result, typeof(MetadataDefinitionCollection)) as MetadataDefinitionCollection;
            }
            catch (Exception ex)
           {
           }
            return metadataDefDic;
        }
    }
}
