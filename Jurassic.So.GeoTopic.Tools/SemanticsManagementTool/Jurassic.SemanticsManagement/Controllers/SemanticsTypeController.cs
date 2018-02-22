using Jurassic.Semantics.IService;
using Jurassic.Semantics.IService.ViewModel;
using Jurassic.WebFrame;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Jurassic.SemanticsManagement.Controllers
{
    /// <summary>
    /// 语义关系类型维护
    /// </summary>
    public class SemanticsTypeController : BaseController
    {

        /// <summary>
        /// 语义关系类型维护服务
        /// </summary>
        public ISemanticsTypeService SemanticsTypeService { get; set; }
        /// <summary>
        /// 使用依赖注入自动注入服务参数，此处构造函数的参数类型
        /// 没有用接口而用的实际的类，则不需在global.asax文件中写注入逻辑
        /// </summary>
        /// <param name="semanticsTypeService"></param>
        public SemanticsTypeController(ISemanticsTypeService semanticsTypeService)
        {
            SemanticsTypeService = semanticsTypeService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetData()
        {
            List<SemanticsTypemodel> list = SemanticsTypeService.GetSemanticsTypeList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        public ActionResult OperateData()
        {
            return View();
        }


        [HttpPost]
        public JsonResult Add(string model)
        {
            try
            {
                var newSemanticsType = JsonConvert.DeserializeObject<SemanticsTypemodel>(model);
                SemanticsTypeService.Add(newSemanticsType);
                return Json(JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json("添加失败！" + e.Message, JsonRequestBehavior.AllowGet);
            }


        }

        [HttpPost]
        public JsonResult EditGetDataResult(string submitData)
        {
            try
            {
                var seModel = JsonConvert.DeserializeObject<SemanticsTypemodel>(submitData);
                SemanticsTypeService.Edit(seModel.SR, seModel);
                return Json(JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json("修改失败!" + e.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EdictResult(string model)
        {
            var conmodel = JsonConvert.DeserializeObject<SemanticsTypemodel>(model);
            ViewData["model"] = conmodel;
            return View();
        }


        [HttpPost]
        public ActionResult Delete(string sr)
        {
            try
            {
                SemanticsTypeService.Delete(sr);
                Json("删除成功！", JsonRequestBehavior.AllowGet);
                return RedirectToAction("index", "SemanticsType");
            }
            catch (Exception e)
            {
                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
