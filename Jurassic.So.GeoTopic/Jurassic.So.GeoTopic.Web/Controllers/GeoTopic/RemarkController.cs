using Jurassic.So.GeoTopic.DataService.Models;
using Jurassic.So.GeoTopic.DataService.Service;
using Jurassic.WebFrame;
using System;
using System.Web.Mvc;

namespace Jurassic.So.GeoTopic.Web.Controllers
{
    public class RemarkController : BaseController
    {
        public IRemarkService IRemark { get; set; }

        public RemarkController(IRemarkService remark)
        {
            IRemark = remark;
        }

        /// <summary>
        /// 获取用户信息的方法
        /// </summary>
        /// <returns></returns>
        public JsonResult GetUserInfo()
        {
            //.net获取用户信息的方法
            //var user = User.Identity.Name;
            //框架在BaseControlleer的对象
           // var users = CurrentUser.Id;
            var user = new UserProfileModel
            {
                UserId = Convert.ToInt32(CurrentUser.Id),
                UserName = CurrentUser.Name
            };
            return Json(user,JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取评论列表
        /// </summary>
        /// <param name="scoap"></param>
        /// <param name="naturekey"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public JsonResult GetRemarkList(string scoap, string naturekey, string index,string size,string filter)
        {  
            var data = IRemark.QueryRemark(scoap, naturekey, Convert.ToInt32(index),Convert.ToInt32(size),filter,Convert.ToInt32(CurrentUser.Id));
            return Json(data,JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除评论
        /// </summary>
        public void DeleteRemark(string Id)
        {
            IRemark.DeleteRemark(Convert.ToInt32(Id));
        }
        /// <summary>
        /// 发表评论
        /// </summary>
        /// <param name="model"></param>
        public void AddRemark(RemarkModel model)
        {
            IRemark.AddRemark(model);
        }

        /// <summary>
        /// 点赞
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <param name="praised"></param>
        public void PraiseRemark(string id, string userId, string praised)
        {
            IRemark.PraiseRemark(Convert.ToInt32(id),Convert.ToInt32(userId),Convert.ToBoolean(praised));
        }
    }
}