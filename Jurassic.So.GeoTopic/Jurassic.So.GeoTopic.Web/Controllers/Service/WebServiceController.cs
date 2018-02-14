using Jurassic.PKS.Service;
using Jurassic.So.Business;
using Jurassic.So.GeoTopic.Web.Utility;
using Jurassic.So.Infrastructure;
using Jurassic.So.Infrastructure.Util;
using Jurassic.WebFrame;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Jurassic.So.GeoTopic.Web.Controllers
{
    /// <summary>
    /// 获取外部服务的总控制器
    /// </summary>
    [Authorize]
    public class WebServiceController : BaseController
    {
        // GET: /WebService/
        private readonly string _apiPath = System.Configuration.ConfigurationManager.AppSettings["ApiServiceURL"] 
            + System.Configuration.ConfigurationManager.AppSettings["ApiVersion"];

        //搜索服务
        private readonly string _search = System.Configuration.ConfigurationManager.AppSettings["Search"];
        private readonly string _match = System.Configuration.ConfigurationManager.AppSettings["Match"];
        private readonly string _getMetadataDefinition = System.Configuration.ConfigurationManager.AppSettings["GetMetadataDefinition"];
        private readonly string _dataService = System.Configuration.ConfigurationManager.AppSettings["DataService"];
        private readonly string _getData = System.Configuration.ConfigurationManager.AppSettings["GetData"];
        private readonly string _searchService = System.Configuration.ConfigurationManager.AppSettings["SearchService"];
        private readonly string _retrieve = System.Configuration.ConfigurationManager.AppSettings["Retrieve"];
        private readonly string _submitapi = System.Configuration.ConfigurationManager.AppSettings["SubmissionServiceUrl"];
        private readonly string _submit = System.Configuration.ConfigurationManager.AppSettings["Submit"];
        private readonly string _upload = System.Configuration.ConfigurationManager.AppSettings["Upload"];
        private readonly string _getMetaData = System.Configuration.ConfigurationManager.AppSettings["GetMetaData"];

        /// <summary>
        /// 前端使用的通用方法，只负责调用不负责参数。get参数的post方法。
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PostFromBody()
        {
            string pram = Request.Form["pram"];
            string url = Request.Form["url"];
            string userToken = TokenManmge.GetTokenService();
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                  {"", pram}
                });
            return Json(WebRequestUtil.PostHttpClientStr(url, content, userToken).GetAwaiter().GetResult(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 前端使用的通用方法，只负责调用不负责参数。post
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Post()
        {
            string pram = Request.Form["pram"];
            string url = Request.Form["url"];
            string userToken = TokenManmge.GetTokenService();
            //return Json(WebRequestUtil.PostHttpClientStr(url, pram, userToken).GetAwaiter().GetResult(), JsonRequestBehavior.AllowGet);
            return null;
        }

        /// <summary>
        /// 前端使用的通用方法，只负责调用不负责参数。get
        /// </summary>
        /// <param name="pram"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult Get(string pram, string url)
        {
            var parmOdj = JsonConvert.DeserializeObject<Dictionary<string, string>>(pram);
            string userToken = TokenManmge.GetTokenService();
            return Json(WebRequestUtil.GetHttpClientStr(url, parmOdj, userToken).GetAwaiter().GetResult(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parmOdj"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public JsonResult Get(Dictionary<string, string> parmOdj, string url)
        {
            //var parmOdj = JsonConvert.DeserializeObject<Dictionary<string, string>>(pram);
            string userToken = TokenManmge.GetTokenService();
            return Json(WebRequestUtil.GetHttpClientStr(url, parmOdj, userToken).GetAwaiter().GetResult(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMetadataDefinition()
        {
            string userToken = TokenManmge.GetTokenService();
            JsonResult json = Json(WebRequestUtil.PostHttpClientStr(_apiPath + _searchService +_getMetadataDefinition, "", userToken).GetAwaiter().GetResult(), JsonRequestBehavior.AllowGet);
            return json;
        }

        public ActionResult GetDataEntity(string urlstr, string ticket, string name=null)
        {
            string userToken = TokenManmge.GetTokenService();
            using (var client = new HttpClientWrapper())
            {
                var url = _apiPath + _dataService + _getData + "?url=" + urlstr + "&ticket=" + ticket;
                //var result = client.Get(url, null, userToken);
                var response = client.Get<HttpResponseMessage>(url, null, userToken);
                var mediaType = response.Content.Headers.ContentType.MediaType;
                var result = response.Content.ReadAsStreamAsync().Result;
                string fileName = null;
                if(name != null)
                    fileName = HttpUtility.UrlEncode(name, Encoding.GetEncoding("UTF-8"));
                return File(result.As<Stream>(), mediaType, fileName);
            }
            //return Json(WebRequestUtil.GetHttpClientStr(_apiPath + _dataService + _getData, parmOdj, userToken).GetAwaiter().GetResult(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRetrieve(Dictionary<string, string> parmOdj)
        {
            string userToken = TokenManmge.GetTokenService();
            return Json(WebRequestUtil.GetHttpClientStr(_apiPath + _dataService + _retrieve, parmOdj, userToken).GetAwaiter().GetResult(), JsonRequestBehavior.AllowGet);
        }

        public ContentResult GetMatch(string pramsJson)
        {
            string userToken = TokenManmge.GetTokenService();
            var result = WebRequestUtil.PostHttpClientStr(_apiPath + _searchService + _match, pramsJson, userToken).Result;
            return Content(result, MimeTypeConst.JSON);
            //return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ContentResult GetSearch(string pramsJson)
        {
            string userToken = TokenManmge.GetTokenService();
            var result = WebRequestUtil.PostHttpClientStr(_apiPath + _searchService + _search, pramsJson, userToken).Result;
            return Content(result, MimeTypeConst.JSON);
            //return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ContentResult PostSubmit(string pramsJson)
        {
            string userToken = TokenManmge.GetTokenService();
            var result = WebRequestUtil.PostHttpClientStr(_submitapi + _submit, pramsJson, userToken).Result; 
            return Content(result, MimeTypeConst.JSON);
            //return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ContentResult GetMateData(Dictionary<string, string> prams)
        {
            string userToken = TokenManmge.GetTokenService();
            var result = WebRequestUtil.GetHttpClientStr(_apiPath+ _searchService+ _getMetaData, prams, userToken).Result;
            return Content(result, MimeTypeConst.JSON);
        }
    }
}
