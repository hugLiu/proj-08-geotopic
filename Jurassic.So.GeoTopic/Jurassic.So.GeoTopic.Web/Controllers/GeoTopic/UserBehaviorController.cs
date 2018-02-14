using Jurassic.AppCenter;
using Jurassic.So.GeoTopic.DataService.Models;
using Jurassic.So.GeoTopic.DataService.Service;
using Jurassic.WebFrame;
using System;
using System.Web.Mvc;
using Jurassic.So.Infrastructure;
using Newtonsoft.Json.Linq;

namespace Jurassic.So.GeoTopic.Web.Controllers.GeoTopic
{
    /// <summary>
    /// 对UserBehavior表的操作
    /// </summary>
    public class UserBehaviorController : GTBaseController
    {
        private IUserBehaviorService UserBehavior { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userBehavior"></param>
        public UserBehaviorController(IUserBehaviorService userBehavior)
        {
            UserBehavior = userBehavior;
        }

        // GET: UserBehavior
        /// <summary>
        /// Index 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>获得某类某页行为列表</summary>
        /// <returns></returns>
        public ActionResult GetList(string type, int from, int size)
        {
            var userId = CurrentUserId.ToInt32();
            var values = UserBehavior.GetList(userId, type, from, size);
            return Json(values, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 通过用户Id获取用户的下载历史
        /// </summary>
        /// <returns></returns>
        public JsonResult GetDownloadMessByUserId(int count)
        {
            var userId = base.CurrentUserId.ToInt32();
            var downloadMess = UserBehavior.GetDownloadMessByUserId(userId, count);
            return Json(downloadMess, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 通过用户Id获取用户的收藏信息
        /// </summary>
        /// <returns></returns>
        public JsonResult GetFavoriteMessByUserId(int count)
        {
            var userIdStr = User.Identity.GetUserId();
            var userId = int.Parse(userIdStr);
            var favoriteMess = UserBehavior.GetFavoriteMessByUserId(userId, count);
            return Json(favoriteMess, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 通过用户Id获取用户的收藏信息
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllFavoriteMessByUserId()
        {
            var userIdStr = User.Identity.GetUserId();
            var userId = int.Parse(userIdStr);
            var favoriteMess = UserBehavior.GetFavoriteMessByUserId(userId);
            return Json(favoriteMess, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取用户操作信息
        /// </summary>
        /// <param name="count"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public JsonResult GetOptionMessByUserId(int count, string type)
        {
            int userId = Convert.ToInt32(CurrentUser.Id);
            var list = UserBehavior.QueryOptionMessByUserId(userId, count, type);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 添加一条收藏信息
        /// </summary>
        /// <returns></returns>
        public JsonResult AddFavoriteMess(string data)
        {
            var result = JObject.Parse(data);
            var model = new UserBehaviorModel
            {
                BehaviorType = result["BehaviorType"].ToString(),
                Type = result["Type"].ToString(),
                Title = result["Title"].ToString(),
                Content = result["Content"].ToString(),
            };
            AddUserBehavior(model);
            return Json(model.Id, JsonRequestBehavior.AllowGet);
        }
        /// <summary>删除用户行为日志</summary>
        /// <param name="id">日志Id</param>
        public JsonResult Remove(int id)
        {
            UserBehavior.DeleteOptionMessById(id, this.CurrentUser.Name);
            return Json(id, JsonRequestBehavior.AllowGet);
        }
    }
}