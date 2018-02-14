using System.Collections.Generic;
using System.Net;
using Jurassic.So.Web;
using Jurassic.PKS.Service.Search;

namespace GTAPI.API
{
    /// <summary>WEB异常映射</summary>
    public class SearchWebExceptionMap : IWebExceptionMap
    {
        /// <summary>服务名称</summary>
        public string ServiceName { get; } = "SearchService";
        /// <summary>配置</summary>
        public IDictionary<string, WebExceptionModel> Config()
        {
            var mappers = new Dictionary<string, WebExceptionModel>
            {
                {
                SearchExceptionCodes.DataConvertFaild.ToString(),
                    new WebExceptionModel()
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        ReasonPhrase = "Data conversion failed"
                    }
                },
                {
                SearchExceptionCodes.InvalidEsRequest.ToString(),
                    new WebExceptionModel()
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        ReasonPhrase = "Invalid elasticsearch request"
                    }
                },
                {
                SearchExceptionCodes.EsProcessFaild.ToString(),
                    new WebExceptionModel()
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        ReasonPhrase = "Elasticsearch processing faild"
                    }
                },
                {
                SearchExceptionCodes.MongoProcessFaild.ToString(),
                    new WebExceptionModel()
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        ReasonPhrase = "Mongo processing faild"
                    }
                }
        };
            return mappers;
        }
    }
}
