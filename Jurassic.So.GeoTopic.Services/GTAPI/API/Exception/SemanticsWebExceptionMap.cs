using System;
using System.Collections.Generic;
using System.Net;
using Jurassic.PKS.Service.Semantics;
using Jurassic.So.Web;

namespace GTAPI.API
{
    /// <summary>WEB异常映射</summary>
    public class SemanticsWebExceptionMap : IWebExceptionMap
    {
        public string ServiceName { get; } = "SemanticsService";


        public IDictionary<string, WebExceptionModel> Config()
        {
            var mappers = new Dictionary<string, WebExceptionModel>();
            mappers.Add(SemanticsExceptionCode.OperationProcessingFailed.ToString(),
                new WebExceptionModel
                {
                    StatusCode = HttpStatusCode.Forbidden,
                    ReasonPhrase = "Operation Processing Failed"
                });
            mappers.Add(SemanticsExceptionCode.InvalidEnumValue.ToString(),
                new WebExceptionModel
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ReasonPhrase = "Invalid Enum Value"
                });
            mappers.Add(SemanticsExceptionCode.MissingParameterValue.ToString(),
                new WebExceptionModel
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ReasonPhrase = "Missing Parameter Value"
                });
            return mappers;
        }
    }
}