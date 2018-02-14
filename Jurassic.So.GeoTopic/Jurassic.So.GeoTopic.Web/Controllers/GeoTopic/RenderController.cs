using System;
using System.Web;
using System.Web.Mvc;
using Jurassic.WebFrame;

namespace Jurassic.So.GeoTopic.Web.Controllers
{
    /// <summary>
    /// GeoTopic体系外部使用SoUI和GtUI模块的统一访问接口。
    /// 目前考虑是公开访问，不需要登陆。
    /// 所有对外可以访问的页面入口都放到这里。逻辑方法可在各自控制器中。
    /// </summary>
    public class RenderController : BaseController
    {
        private readonly string _submitapi =
            System.Configuration.ConfigurationManager.AppSettings["SubmissionServiceUrl"];

        private readonly string _upload = System.Configuration.ConfigurationManager.AppSettings["Upload"];

        private readonly string _apiPath = System.Configuration.ConfigurationManager.AppSettings["ApiServiceURL"] +
                                           System.Configuration.ConfigurationManager.AppSettings["ApiVersion"];

        private readonly string _getMetadataDefinition =
            System.Configuration.ConfigurationManager.AppSettings["GetMetadataDefinition"];

        private readonly string _searchService = System.Configuration.ConfigurationManager.AppSettings["SearchService"];

        /// <summary>
        /// 详情展示页面
        /// </summary>
        /// <param name="iiid"></param>
        /// <returns></returns>
        ///测试iiid :46F066E4BCB2210223CAADEB60B0D7CE
        public ActionResult PTDetail(string iiid)
        {
            ViewBag.iiid = iiid;
            return View();
        }

        /// <summary>
        /// 列表展示页面
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public ActionResult PtList(string param)
        {
            ViewBag.Param = HttpUtility.UrlEncode(param);
            return View();
        }

        /// <summary>
        /// 展示主页面
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public ActionResult Topic(string param)
        {
            return RedirectToAction("Index", "GeoTopic", new {param});
        }

        /// <summary>
        /// 整体成果展示页面
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        public ActionResult Card(string topicId)
        {
            ViewBag.CardId = Guid.NewGuid().ToString();
            ViewBag.TopicId = HttpUtility.UrlEncode(topicId);
            return View();
        }

        /// <summary>
        /// 单个成果展示，用于Url显示.
        /// 用于简单使用只需要param就可以，但是现在PtRender的逻辑是走的这边，所以要支持其他参数
        /// </summary>
        /// <returns></returns>
        public ActionResult Pt(string param, string global = null, int heights = 0)
        {
            ViewBag.Param = HttpUtility.UrlEncode(param);
            ViewBag.global = HttpUtility.UrlEncode(global);
            ViewBag.heights = heights;
            return View();
        }

        /// <summary>
        /// 展示单个文件的预览
        /// </summary>
        /// <returns></returns>
        public ActionResult PtUrl(string param, int infoIndex, int fileIndex,string globle)
        {
            ViewBag.Param = HttpUtility.UrlEncode(param);
            ViewBag.Globle = HttpUtility.UrlEncode(globle);
            ViewBag.infoIndex = infoIndex;
            ViewBag.fileIndex = fileIndex;
            return View();
        }

        /// <summary>
        /// 最新入库成果展示
        /// </summary>
        /// <returns></returns>
        public ActionResult NewEstPt()
        {
            return View();
        }

        /// <summary>
        /// 成果收藏
        /// </summary>
        /// <returns></returns>
        public ActionResult Favorite(int count)
        {
            ViewBag.count = count;
            return View();
        }

        /// <summary>
        /// 下载历史
        /// </summary>
        /// <returns></returns>
        public ActionResult DownloadHistory(int count)
        {
            ViewBag.count = count;
            return View();
        }

        /// <summary>
        /// 成果提交
        /// </summary>
        /// <returns></returns>
        public ActionResult Submission(string param)
        {
            ViewBag.Metadata = _apiPath + _searchService + _getMetadataDefinition;
            ViewBag.Upload = _submitapi + _upload;
            ViewBag.Param = HttpUtility.UrlEncode(param);
            return View();
        }

        /// <summary>
        /// 评论
        /// </summary>
        /// <returns></returns>
        public ActionResult Remarks(string scoap, string key)
        {
            ViewBag.scoap = scoap;
            ViewBag.key = key;
            return View();
        }
    }
}
