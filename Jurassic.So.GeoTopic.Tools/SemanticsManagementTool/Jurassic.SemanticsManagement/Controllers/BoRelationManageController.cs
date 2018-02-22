using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jurassic.Semantics.IService;
using Jurassic.Semantics.IService.ViewModel;
using Newtonsoft.Json.Linq;

namespace Jurassic.SemanticsManagement.Controllers
{
    /// <summary>
    /// 业务对象关系维护
    /// </summary>
    public class BoRelationManageController : Controller
    {
        private IBoRelationManageService _service;
        /// <summary>
        /// 服务注入
        /// </summary>
        /// <param name="service"></param>
        public BoRelationManageController(IBoRelationManageService service)
        {
            _service = service;
        }

        /// <summary>
        /// 业务对象关系维护
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获得业务对象关系类型
        /// </summary>
        /// <returns></returns>
        public JsonResult GetBoRelationType()
        {
            var result = _service.GetBoRelationType();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据对象1，和对象2的对象类型获得 
        /// </summary>
        /// <returns></returns>
        public JsonResult GetId2ById1AndBot2(string id1, string bot2)
        {
            var result = _service.GetId2ById1AndBot2(id1, bot2);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加业务对象关系
        /// </summary>
        /// <param name="bor"></param>
        public JsonResult AddBor(string bor)
        {
            var data = new Dictionary<string, string>();

            try
            {
                var json = Server.UrlDecode(bor);
                var tokens = JArray.Parse(json);
                foreach (var token in tokens)
                {
                    if (token.HasValues)
                    {
                        _service.AddBoRelation(JsonToBor(token.ToString()));
                    }
                }
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

        /// <summary>
        /// 添加业务对象关系
        /// </summary>
        /// <param name="bor"></param>
        public JsonResult UpdateBor(string bor)
        {
            var data = new Dictionary<string, string>();

            try
            {
                var json = Server.UrlDecode(bor);
                var tokens = JArray.Parse(json);
                foreach (var token in tokens)
                {
                    if (token.HasValues)
                    {
                        _service.UpdateBoRelation(JsonToBor(token.ToString()));
                    }
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
        /// 删除业务对象关系
        /// </summary>
        /// <param name="bor"></param>
        public JsonResult DeleteBor(string bor)
        {
            var data = new Dictionary<string, string>();

            try
            {
                var json = Server.UrlDecode(bor);
                _service.DeleteBoRelation(JsonToBor(json));
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

        private BoRelationModel JsonToBor(string json)
        {
            var model = new BoRelationModel();

            var token = JObject.Parse(json);

            if (token.HasValues)
            {
                var id1 = token["ID1"].ToString();
                var id2 = token["ID2"].ToString();
                var bot1 = token["BOT1"].ToString();
                var bot2 = token["BOT2"].ToString();
                var type = token["RelTypeCode"].ToString();
                model = new BoRelationModel()
                {
                    ID1 = Guid.Parse(id1),
                    ID2 = Guid.Parse(id2),
                    BOT1 = bot1,
                    BOT2 = bot2,
                    RelTypeCode = type
                };
            }
            return model;
        }

    }
}
