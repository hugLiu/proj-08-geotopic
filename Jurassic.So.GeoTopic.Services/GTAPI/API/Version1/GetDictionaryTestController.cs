using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Jurassic.So.Infrastructure;

namespace GTAPI.API.Version1
{
    /// <summary>
    /// 搜索服务API
    /// </summary>
    public class GetDictionaryTestController : WebAPIController
    {
        [HttpHead, HttpGet, HttpPost]
        public async Task<HttpResponseMessage> GetDictionary(string path)
        {
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            var content = File.ReadAllText(path);
            response.Content = new StringContent(content, Encoding.UTF8);
            response.Headers.Age = TimeSpan.FromHours(1);
            response.Headers.ETag = EntityTagHeaderValue.Parse($"\"{content.ToMD5()}\"");
            return response;
        }
        public List<string> LoadFromTextFile(String fileName)
        {
            var dict = new List<string>();
            using (var sr = new StreamReader(fileName, Encoding.UTF8))
            {
                while (!sr.EndOfStream)
                {
                    dict.Add(sr.ReadLine());
                }
            }
            return dict;
        }
    }
}
