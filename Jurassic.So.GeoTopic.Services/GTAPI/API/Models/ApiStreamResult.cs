using Jurassic.So.Infrastructure;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace GTAPI.API
{
    /// <summary>自定义流结果</summary>
    public class ApiStreamResult : ApiActionResult
    {
        /// <summary>构造函数</summary>
        public ApiStreamResult(string mimeType, object content, ApiController controller) : base(mimeType, content, controller) { }
        /// <summary>是否能转换为输出</summary>
        public static bool CanConvert(object content)
        {
            return content is Stream || content is byte[];
        }
        /// <summary>执行</summary>
        protected override HttpResponseMessage Execute()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                var stream = BuildContent();
                stream.Position = 0;
                response.Content = new StreamContent(stream);
                //response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = fileName };
                response.Content.Headers.ContentType = new MediaTypeHeaderValue(this.MimeType);
                response.Content.Headers.ContentLength = stream.Length;
                response.RequestMessage = this.Request;
            }
            catch
            {
                response.Dispose();
                throw;
            }
            return response;
        }
        /// <summary>生成内容</summary>
        protected Stream BuildContent()
        {
            var stream = this.Content.As<Stream>();
            if (stream == null)
            {
                var content = this.Content.As<byte[]>();
                stream = new MemoryStream(content);
            }
            return stream;
        }
    }
}
