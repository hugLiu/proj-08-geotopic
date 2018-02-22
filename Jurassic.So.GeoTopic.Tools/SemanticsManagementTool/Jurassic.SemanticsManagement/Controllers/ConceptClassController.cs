using System.Collections.Generic;
using Jurassic.Semantics.IService;
using Jurassic.Semantics.IService.ViewModel;
using Jurassic.WebFrame;
using Newtonsoft.Json;
using System;
using System.Web.Mvc;

namespace Jurassic.SemanticsManagement.Controllers
{
    /// <summary>
    /// 概念类维护
    /// </summary>
    public class ConceptClassController : BaseController
    {
        public IConceptClassService _conceptClassService { get; set; }

        /// <summary>
        /// 使用依赖注入自动注入服务参数，此处构造函数的参数类型
        /// 没有用接口而用的实际的类，则不需在global.asax文件中写注入逻辑
        /// </summary>
        /// <param name="conceptClassService"></param>
        public ConceptClassController(IConceptClassService conceptClassService)
        {
            _conceptClassService = conceptClassService;
        }

        /// <summary>
        /// 概念类维护界面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取概念类数据
        /// </summary>
        /// <returns>返回获得的概念类数据</returns>
        public JsonResult GetData()
        {
            var list = _conceptClassService.GetConceptClassList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="submitData"></param>
        /// <returns></returns>
        public JsonResult EditGetDataResult(string submitData)
        {
            try
            {
                var ccModel = JsonConvert.DeserializeObject<ConceptClassmodel>(submitData);
                var ccsmodel = new ConceptClassmodel
                {
                    CC = ccModel.CC,
                    CCCode = ccModel.CCCode,
                    Source = ccModel.Source,
                    Tag = ccModel.Tag,
                    Type = ccModel.Type
                };
                _conceptClassService.Edit(ccModel.CCCode, ccsmodel);
                return Json("修改成功!!!", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json("修改成功失败" + e.Message, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult OperateData()
        {
            return View();
        }

        /// <summary>
        /// 更新已存在的概念类的信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult EditResult(String model)
        {
            if (model == null) return null;
            var conmodel = JsonConvert.DeserializeObject<ConceptClassmodel>(model);
            ViewData["model"] = conmodel;
            return View();
        }

        /// <summary>
        /// 根据 CCCode 删除已经存在的概念类
        /// </summary>
        /// <param name="cccode"></param>
        /// <returns></returns>
        public ActionResult Delete(string cccode)
        {
            var data = new Dictionary<string, string>();
            try
            {
                _conceptClassService.Delete(cccode);
                data.Add("State", "success");
                data.Add("Text", "数据删除成功!!!");
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
        /// 添加新的概念类
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult Add(string model)
        {
            var data = new Dictionary<string, string>();
            var newClassmodel = JsonConvert.DeserializeObject<ConceptClassmodel>(model);
            try
            {
                _conceptClassService.Add(newClassmodel);
                data.Add("State", "success");
                data.Add("Text", "数据添加成功!!!");
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
