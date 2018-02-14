using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Jurassic.PKS.Service.Submission;
using Jurassic.So.Web;

namespace GTAPI.API
{
    /// <summary>WEB异常映射</summary>
    public class SubmissionWebExceptionMap : IWebExceptionMap
    {
        /// <summary>服务名称</summary>
        public string ServiceName { get; } = "SubmissionService";
        /// <summary>配置</summary>
        public IDictionary<string, WebExceptionModel> Config()
        {
            var mappers = new Dictionary<string, WebExceptionModel>();
            mappers.Add(SubmissionExceptionCodes.MissingMultipartData.ToString(),
                new WebExceptionModel()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ReasonPhrase = "Invalid Format:Not A 'multipart/form-data' Format"
                });
            mappers.Add(SubmissionExceptionCodes.MissingParameterValue.ToString(),
                new WebExceptionModel()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ReasonPhrase = "Missing Parameter Value"
                });
            mappers.Add(SubmissionExceptionCodes.OperationProcessingFailed.ToString(),
                new WebExceptionModel()
                {
                    StatusCode = HttpStatusCode.Forbidden,
                    ReasonPhrase = "Operation Processing Failed"
                });
            mappers.Add(SubmissionExceptionCodes.ParameterParsingFailed.ToString(),
                new WebExceptionModel()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ReasonPhrase = "Parameter Parsing Failed"
                });
            mappers.Add(SubmissionExceptionCodes.SubmissionFailed.ToString(),
                new WebExceptionModel()
                {
                    StatusCode = HttpStatusCode.Forbidden,
                    ReasonPhrase = "Submission Failed"
                });
            mappers.Add(SubmissionExceptionCodes.UploadingFileFailed.ToString(),
                new WebExceptionModel()
                {
                    StatusCode = HttpStatusCode.Forbidden,
                    ReasonPhrase = "Uploading File Failed"
                });
            return mappers;
        }
    }
}