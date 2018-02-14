using Jurassic.PKS.Service.Data;
using Jurassic.So.Business;
using Jurassic.So.Web;
using System;
using System.Collections.Generic;
using System.Net;

namespace GTAPI.API
{
    /// <summary>WEB异常映射</summary>
    public class DataWebExceptionMap : IWebExceptionMap
    {
        /// <summary>服务名称</summary>
        public string ServiceName { get; } = "DataService";
        /// <summary>配置</summary>
        public IDictionary<string, WebExceptionModel> Config()
        {
            var mappers = new Dictionary<string, WebExceptionModel>();
            mappers.Add(DataExceptionCode.AdapterNotExist.ToString(),
                new WebExceptionModel()
                {
                    StatusCode = HttpStatusCode.Forbidden,
                    ReasonPhrase = "Adapter Not Exist"
                });
            mappers.Add(DataExceptionCode.AdapterScopeNotExist.ToString(),
                new WebExceptionModel()
                {
                    StatusCode = HttpStatusCode.Forbidden,
                    ReasonPhrase = "Adapter Scope Not Exist"
                });
            mappers.Add(DataExceptionCode.InvalidDataFormat.ToString(),
                new WebExceptionModel()
                {
                    StatusCode = HttpStatusCode.Forbidden,
                    ReasonPhrase = "Invalid Data Content Format"
                });
            return mappers;
        }
    }
}
