using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using Jurassic.AppCenter;
using Jurassic.Semantics.IService;
using Newtonsoft.Json.Linq;

namespace Jurassic.SemanticsManagement.Controllers
{
    public class BOTManagerController : Controller
    {
        private IBOTManageService Service;

        /// <summary>
        /// 服务注入
        /// </summary>
        /// <param name="service"></param>
        public BOTManagerController(IBOTManageService service)
        {
            Service = service;
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetBOTTreeResult(string id)
        {
            return Json(Service.GetBotbyId(id), JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteBOT(string id)
        {
            var data = new Dictionary<string, string>();
            try
            {
                Service.DeleteBot(id);
                data.Add("State","success");
                data.Add("Text", "数据删除成功!!!");
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                data.Add("State", "error");
                data.Add("Text",e.Message );
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 更新业务对象类型
        /// </summary>
        /// <param name="id"></param>
        /// <param name="bot"></param>
        /// <returns></returns>
        public JsonResult UpdateBot(string bot)
        {
            var userName = User.Identity.GetUserName();
            bot = Server.UrlDecode(bot);
            var tokens = JArray.Parse(bot);
            string _bot;

            var data = new Dictionary<string, string>();
            try
            {
                foreach (var token in tokens)
                {
                    var id = token["TypeID"].ToString();
                    _bot = token["BOT"].ToString();
                    var typeName = token["TypeName"].ToString();
                    var description = token["Description"].ToString();
                    var remark = token["Remark"].ToString();
                    var orderIndex = Convert.ToInt32(token["OrderIndex"].ToString());

                    Service.UpdateBot(id, userName, _bot, typeName, orderIndex, description, remark); 
                }
                data.Add("State", "success");
                data.Add("Text", "数据更新成功!!!");
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                data.Add("State", "error");
                data.Add("Text", e.Message);
                return Json(data, JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// 插入业务对象类型
        /// </summary>
        /// <param name="bot"></param>
        /// <returns></returns>
        public JsonResult InsertBot(string bot)
        {
            var userName = User.Identity.GetUserName();
            bot = Server.UrlDecode(bot);
            var tokens = JObject.Parse(bot);

            var _bot = tokens["BOT"].ToString();
            var typeName = tokens["TypeName"].ToString();
            var orderIndex = Convert.ToInt32(tokens["OrderIndex"].ToString());
            var description = tokens["Description"].ToString();
            var remark = tokens["Remark"].ToString();

            var data = new Dictionary<string, string>();

            try
            {
                Service.InsertBot(_bot, userName, typeName, orderIndex, description, remark);
                data.Add("State", "success");
                data.Add("Text", "数据插入成功!!!");
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                data.Add("State", "error");
                data.Add("Text", e.Message);
                return Json(data, JsonRequestBehavior.AllowGet);
            }

        }

    }
}
