using Jurassic.AppCenter;
using Jurassic.AppCenter.Config;
using Jurassic.Semantics.IService;
using Jurassic.SemanticsManagement.HelpClass;
using Jurassic.WebFrame;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Jurassic.SemanticsManagement.Controllers
{
    /// <summary>
    /// 关键词维护
    /// </summary>
    public class KeyWordsManagerController : BaseController
    {
        private IKeyWordsManageService Service;

        /// <summary>
        /// 服务注入
        /// </summary>
        /// <param name="service"></param>
        public KeyWordsManagerController(IKeyWordsManageService service)
        {
            Service = service;
        }

        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// .加载数据BP和PT（以树的结构展示）按照orderindex排序，同时判断记录关键词录入状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="show"></param>
        /// <returns></returns>
        public JsonResult GetBPAndPtTreeResult(string id, bool show)
        {
            return Json(Service.GetBPPTTree(id, show), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 将关键词保存到数据库指定位置
        /// </summary>
        /// <param name="id"></param>
        /// <param name="words"></param>
        public void AddKeyWords(string id, string words)
        {
            var userName = User.Identity.GetUserName();
            words = Server.UrlDecode(words);
            var keywords = new Dictionary<string, int>();

            var tokens = JArray.Parse(words);
            if (tokens.HasValues)
            {
                foreach (var token in tokens)
                {
                    var word = token["text"].ToString();
                    var order = Convert.ToInt32(token["id"].ToString());
                    //去除重复项
                    if (keywords.ContainsKey(word)) continue;
                    keywords.Add(word, order);
                }
            }
            Service.AddKeyWords(id, userName, keywords);
        }

        public void UpdateKeyWords(string id, string words)
        {
            var userName = User.Identity.GetUserName();
            words = Server.UrlDecode(words);
            var keywords = new Dictionary<string, int>();

            var tokens = JArray.Parse(words);
            if (tokens.HasValues)
            {
                foreach (var token in tokens)
                {
                    var word = token["text"].ToString();
                    var order = Convert.ToInt32(token["id"].ToString());
                    //去除重复项
                    if (keywords.ContainsKey(word)) continue;
                    keywords.Add(word, order);
                }
            }

            Service.UpdateKeyWords(id, userName, keywords);
        }
        /// <summary>
        /// 根据id获得关键词的列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetKeyWords(string id)
        {
            return Json(Service.GetKeyWords(id), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除关键词
        /// </summary>
        /// <param name="id">叙词id</param>
        /// <param name="words">关键词名称</param>
        public void DeleteKeyWord(string id, string words)
        {
            var keywords = new List<string>();
            words = Server.UrlDecode(words);
            var tokens = JArray.Parse(words);
            if (tokens.HasValues)
            {
                foreach (var token in tokens)
                {
                    var word = token["text"].ToString();
                    keywords.Add(word);
                }
            }
            Service.DeleteKeyWords(id, keywords);
        }

        /// <summary>
        ///调用语义服务获得分词结果
        /// </summary>
        /// <param name="sentence">短语</param>
        /// <param name="method">分词方法名称</param>
        /// <returns></returns>
        public JsonResult Segment(string sentence, string method)
        {
            var config = new ConfigManager();
            string baseUrl = config.GetAppSettings("IP");
            string name = config.GetAppSettings("Name");
            string key = config.GetAppSettings("PassWord");

            var token = JObject.Parse(WebRequestUtil.GetToken(baseUrl + "token", name, key));
            var accessToken = token["access_token"].ToString();
            var param = new Dictionary<string, string> { { "Sentence", sentence }, { "Method", method } };
            var result = WebRequestUtil.PostHttpClientStr(baseUrl + "API/SemanticsService/Segment", param, accessToken).Result;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
