using Jurassic.Semantics.Service;
using System.Web.Mvc;
using Jurassic.WebFrame;

namespace Jurassic.SemanticsManagement.Controllers
{
    /// <summary>
    /// 语义同步
    /// </summary>
    public class SemanticsSyncController : BaseController
    {
        private readonly SyncToElasticSearch _service = new SyncToElasticSearch();
        /// <summary>
        /// 语义同步操作页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 重新索引 分类叙词语义部分信息
        /// </summary>
        /// <returns></returns>
        public JsonResult ReIndexGlossary()
        {
            var result = _service.ReIndexGlossary();
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 重新索引 成果类型上下文信息
        /// </summary>
        /// <returns></returns>
        public JsonResult ReIndexPtContext()
        {
            var result = _service.ReIndexPtContext();
            return Json(result,JsonRequestBehavior.AllowGet);
        }

    }
}
