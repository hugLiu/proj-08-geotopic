using Jurassic.PKS.Service.Index;
using Jurassic.So.Business;
using Jurassic.So.Web;
using System;
using System.Collections.Generic;
using System.Net;

namespace GTAPI.API
{
    /// <summary>WEB异常映射</summary>
    public class IndexWebExceptionMap : IWebExceptionMap
    {
        /// <summary>服务名称</summary>
        public string ServiceName { get; } = "IndexerService";
        /// <summary>配置</summary>
        public IDictionary<string, WebExceptionModel> Config()
        {
            var mappers = new Dictionary<string, WebExceptionModel>();
            mappers.Add(MetadataExceptionCodes.UrlNotExist.ToString(),
                new WebExceptionModel()
                {
                    StatusCode = HttpStatusCode.Forbidden,
                    ReasonPhrase = "Metadata Url Not Exist"
                });
            mappers.Add(IndexExceptionCodes.SavingIndexFailed.ToString(),
                new WebExceptionModel()
                {
                    StatusCode = HttpStatusCode.Forbidden,
                    ReasonPhrase = "Saving index failed"
                });
            mappers.Add(IndexExceptionCodes.UpdatingIndexFailed.ToString(),
                new WebExceptionModel()
                {
                    StatusCode = HttpStatusCode.Forbidden,
                    ReasonPhrase = "Updating index failed"
                });
            mappers.Add(IndexExceptionCodes.DeletingIndexFailed.ToString(),
                new WebExceptionModel()
                {
                    StatusCode = HttpStatusCode.Forbidden,
                    ReasonPhrase = "Deleting index failed"
                });
            return mappers;
        }
    }
}
