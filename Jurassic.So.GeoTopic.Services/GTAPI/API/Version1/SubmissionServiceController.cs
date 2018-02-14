using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using GTAPI.API.Models;
using Jurassic.PKS.Service.Submission;
using Jurassic.PKS.WebAPI.Models;
using Jurassic.PKS.WebAPI.Submission;
using Jurassic.So.Infrastructure;
using Jurassic.PKS.Service;

namespace GTAPI.API.Version1
{
    /// <summary>成果提交服务API </summary>
    public class SubmissionServiceController : WebAPIController
    {
        /// <summary>成果提交服务</summary>
        private ISubmission SubmissionService { get; set; }
        /// <summary>构造函数</summary>
        public SubmissionServiceController(ISubmission submissionService)
        {
            this.SubmissionService = submissionService;
        }
        /// <summary>获得服务信息</summary>
        protected override ServiceInfo GetServiceInfo()
        {
            return new ServiceInfo()
            {
                Description = "成果提交服务主要用于上传文件或提交成果文件及元数据到目标系统中"
            };
        }

        /// <summary>
        /// 上传一个成果文件到目标系统中
        /// </summary>
        /// <param name="file">待上传的文件</param>
        /// <returns>文件ID</returns>
        [HttpPost]
        public async Task<object> Upload()
        {
            var result = string.Empty;
            if (!Request.Content.IsMimeMultipartContent())
            {
                SubmissionExceptionCodes.MissingMultipartData.ThrowUserFriendly("不是multipart/form-data格式的数据！",
                    "应该使用HTML Form表单multipart/form-data格式或C#的MultipartFormDataContent类来准备请求体！");
            }
            var provider = new MultipartFormDataMemoryStreamProvider();
            try
            {
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);
                if (provider.FileContents.Count <= 0)
                {
                    SubmissionExceptionCodes.UploadingFileFailed.ThrowUserFriendly("上传文件失败！", "请确保有文件上传！");
                }
                // This illustrates how to get the file names.
                var fileContent = provider.FileContents.First();
                var inputStream = await fileContent.ReadAsStreamAsync();
                var fileName = fileContent.Headers.ContentDisposition.FileName;
                if (fileName.EndsWith("\""))
                {
                    fileName = fileName.Substring(1, fileName.Length - 2);
                }
                if (fileName.Contains("\\"))
                {
                    fileName = Path.GetFileName(fileName);
                }

                result = await this.SubmissionService.UploadAsync(fileName, inputStream);
            }
            catch (Exception ex)
            {
                SubmissionExceptionCodes.OperationProcessingFailed.ThrowUserFriendly("发生不可预期的异常！", ex.Message);
            }
            return Json(new { fileID = result });
        }

        /// <summary>
        /// 提交成果的文件集和元数据集到目标系统中
        /// </summary>
        /// <param name="info">提交信息</param>
        /// <returns>成果ID</returns>
        [HttpPost]
        public async Task<object> Submit(SubmissionInfoRequest request)
        {
            if (request == null)
                SubmissionExceptionCodes.MissingParameterValue.ThrowUserFriendly("缺请求参数！", "请求参数为null！");
            var info = request.MapTo<SubmissionInfo>();
            return Json(new { naturekey = await this.SubmissionService.SubmitAsync(info)});
        }
    }
}