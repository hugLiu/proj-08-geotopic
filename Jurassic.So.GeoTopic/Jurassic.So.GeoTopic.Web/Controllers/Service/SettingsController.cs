using System.Web.Mvc;
using Jurassic.WebFrame;

namespace Jurassic.So.GeoTopic.Web.Controllers
{
    public class SettingsController : BaseController
    {
        private readonly string _apiPath = System.Configuration.ConfigurationManager.AppSettings["ApiServiceURL"] + System.Configuration.ConfigurationManager.AppSettings["ApiVersion"];

        /// <summary>
        /// 获取Api服务的域名
        /// </summary>
        /// <returns></returns>
        public JsonResult GetApiHost()
        {
            return Json(_apiPath,JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取模块配置信息
        /// </summary>
        /// <returns></returns>
        public JsonResult GetModule()
        {
            string content = System.IO.File.ReadAllText(Server.MapPath("~/App_Data/ModuleConfig.json"));
            return Json(content, JsonRequestBehavior.AllowGet);
        }
    }
}