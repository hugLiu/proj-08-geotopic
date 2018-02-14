using Jurassic.AppCenter;
using Jurassic.So.GeoTopic.DataService;
using Jurassic.WebFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jurassic.So.GeoTopic.Web.Controllers
{
    /// <summary>
    /// GeoTopic主控制器
    /// </summary>
    [Authorize]
    public class GeoTopicController : BaseController
    {
        private IGeoTopicService GeoTopicService { get; }
        private IPageDetailsService PageDetailsService { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="geoTopicService"></param>
        /// <param name="pageDetailsService"></param>
        public GeoTopicController(IGeoTopicService geoTopicService, IPageDetailsService pageDetailsService)
        {
            GeoTopicService = geoTopicService;
            PageDetailsService = pageDetailsService;
        }

        /// <summary>
        /// GeoTopic主页面
        /// </summary>
        /// <returns></returns>
        /// 这里会从其他页面传入参数，包含树的和展示卡片的全局参数。
        /// 树的参数用于过滤树，树参数的形式是keyValue。
        /// 卡片的参数用于合并到卡片参数中，卡片参数的形式也是KeyValue，没有结构，用于直接替换参数。
        /// 有一个问题是如果是卡片参数没有的参数，那么应该放在什么地方。
        public ActionResult Index(string param)
        {
            //{
            //    "topic":{
            //    },
            //    "kmd":{
            //        "Author":"张三"
            //    }
            //}
            //param = "{\"topic\":{\"oil\":\"测试\"},\"kmd\":{\"Author\":\"张三\"}}";
            //----------------------------------------------------
            ViewBag.TopicParm = HttpUtility.UrlEncode(param);
            //topicParm.Add("oil", "测试");//code为oil，title是石油
            //ViewBag.TopicParm = HttpUtility.UrlEncode(JsonConvert.SerializeObject(topicParm));
            //ViewBag.CardParm = cardParm;
            //ViewBag.Topics = HttpUtility.UrlEncode(JsonConvert.SerializeObject(GeoTopicService.GetTopics(topicParm,userId)));
            return View();
        }

        public ActionResult Index2(string param)
        {
            ViewBag.TopicParm = HttpUtility.UrlDecode(param);
            return View();
        }

        /// <summary>
        /// 比较组件测试页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Compare()
        {
            return View();
        }
        /// <summary>
        /// 成果列表测试页面
        /// </summary>
        /// <returns></returns>
        public ActionResult PTList()
        {
            return View();
        }
        /// <summary>
        /// 临近对象推荐组件测试页面
        /// </summary>
        /// <returns></returns>
        public ActionResult RcmdBORecommend()
        {
            return View();
        }

        /// <summary>
        /// 获取卡片信息
        /// </summary>
        /// <param name="id">topicId</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetCards(string id)
        {
            var topicId = Convert.ToInt32(id);
            var data = PageDetailsService.GetGeoTCards(topicId).OrderBy(o => o.OrderId).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取知识树
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetTopics(Dictionary<string, string> topicIndex, string[] topicCode, string[] topicTitle,
            string userId = null)
        {
            //2016.10.18   Luke.Lu
            //由于在框架中会默认给字典类型加入当前Controllers和Action的名称，所以手动去掉这两个值。
            //这个地方的问题待确认，标记占位。
            if (topicIndex != null)
            {
                topicIndex.Remove("controller");
                topicIndex.Remove("action");
            }

            //var tParm = new Dictionary<string, string>();
            if (userId == null)
                userId = User.Identity.GetUserId();
            return Json(GeoTopicService.GetTopics(topicIndex, topicCode, topicTitle, userId).Select(o => new
            {
                id = o.Id,
                parentId = o.PId,
                text = o.Title,
                value = o.Id,
                state = new
                {
                    opened = true
                }
            }).ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Test()
        {

            return View();
        }

        /// <summary>
        /// 推荐Tab页测试页面
        /// </summary>
        /// <returns></returns>
        public ActionResult RcmdTabs()
        {
            return View();
        }

        public ActionResult SearchIndex()
        {
            ViewBag.IsUser = !string.IsNullOrWhiteSpace(User.Identity.GetUserId());
            ViewBag.UserName = User.Identity.GetUserName();
            return View();
        }
    }
}