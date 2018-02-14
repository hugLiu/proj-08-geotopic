using System.Web.Mvc;
using Jurassic.So.GeoTopic.Web.Utility;
using Jurassic.WebFrame;

namespace Jurassic.So.GeoTopic.Web.Controllers
{
    /// <summary>
    /// 界面中token授权管理
    /// </summary>
    [Authorize]
    public class TokenController: BaseController
    {
        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        public JsonResult GetToken()
        {
            return Json(TokenManmge.GetTokenService(), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 清除token
        /// </summary>
        /// <param name="token"></param>
        public void CleanToken(string token)
        {
            TokenManmge.CleanToken(token);
        }
    }
}