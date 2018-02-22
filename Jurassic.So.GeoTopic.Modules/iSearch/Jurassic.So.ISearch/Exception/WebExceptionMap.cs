using System.Collections.Generic;
using System.Net;
using Jurassic.So.Web;

namespace Jurassic.So.Search
{
    /// <summary>WEB异常映射</summary>
    public class WebExceptionMap : IWebExceptionMap
    {
        /// <summary>服务名称</summary>
        public string ServiceName { get; } = "SearchService";
        /// <summary>配置</summary>
        public IDictionary<string, WebExceptionModel> Config()
        {
            var mappers = new Dictionary<string, WebExceptionModel>
            {
                {
                    SearchExceptionCodes.InvalidPagerParameter.ToString(),
                    new WebExceptionModel()
                    {
                        StatusCode = HttpStatusCode.Forbidden,
                        ReasonPhrase = "Invalid Pager Value"
                    }
                },
                {
                SearchExceptionCodes.DataConvertFaild.ToString(),
                    new WebExceptionModel()
                    {
                        StatusCode = HttpStatusCode.Forbidden,
                        ReasonPhrase = "Data conversion failed"
                    }
                },
                {
                SearchExceptionCodes.InvalidEsRequest.ToString(),
                    new WebExceptionModel()
                    {
                        StatusCode = HttpStatusCode.Forbidden,
                        ReasonPhrase = "Invalid ES Request"
                    }
                }
        };
            return mappers;
        }
    }
}
