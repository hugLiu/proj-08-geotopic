using Jurassic.AppCenter;
using Jurassic.Semantics.IService;
using Jurassic.Semantics.IService.ViewModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms.VisualStyles;

namespace Jurassic.SemanticsManagement.Controllers
{
    /// <summary>
    /// 业务对象信息维护
    /// </summary>
    public class BoInfoManageController : Controller
    {
        private IBoManageService _service;

        /// <summary>
        /// 服务注入
        /// </summary>
        /// <param name="service"></param>
        public BoInfoManageController(IBoManageService service)
        {
            _service = service;
        }

        /// <summary>
        /// 业务对象信息维护页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 弹出框，业务对象基础信息维护
        /// </summary>
        /// <returns></returns>
        public ActionResult BoinfoWindow(string operation, string bot, string pid)
        {
            ViewBag.action = operation;
            ViewBag.bot = bot;
            ViewBag.pid = pid;

            return View();
        }

        /// <summary>
        /// 获得指定bot的业务对象的根节点信息
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public JsonResult GetBoRootByBot(string bot,string key)
        {
            var id = string.Empty;

            //分页
            var pageIndex = Convert.ToInt32(Request["pageIndex"]);
            var pageSize = Convert.ToInt32(Request["pageSize"]);

            //获得根节点节点
            var result = _service.GetBoByBot(bot, id,key, pageIndex, pageSize);
            var totalCount = _service.GetCountByBotAndId(bot,key, id);

            var hs = new Hashtable();
            hs["data"] = result;
            hs["total"] = totalCount;

            return Json(hs, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获得指定bot的所有业务对象信息
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public JsonResult GetBoByBot(string bot, string value)
        {

            //分页
            var pageIndex = Convert.ToInt32(Request["pageIndex"]);
            var pageSize = Convert.ToInt32(Request["pageSize"]);

            //获得根节点节点
            var result = _service.GetByBot(bot, value, pageIndex, pageSize);
            var totalCount = _service.GetCountByBot(bot, value);

            var hs = new Hashtable();
            hs["data"] = result;
            hs["total"] = totalCount;

            return Json(hs, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据id获得业务对象信息
        /// </summary>
        /// <returns></returns>
        public JsonResult GetBoById(string id)
        {
            if (id == null) throw new ArgumentNullException("id");
            var rootbo = _service.GetBoById(id);
            return Json(rootbo, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 获得指定bot的业务对象信息
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetBoByBotAndRootId(string bot, string id)
        {
            if (String.IsNullOrEmpty(id)) id = null;

            //获得下一级节点
            var result = _service.GetBoByBot(bot, id);

            //判断节点，是否有子节点。如果有，则处理isLeaf和expanded。
            for (int i = 0, l = result.Count; i < l; i++)
            {
                var node = result[i];
                node.isLeaf = true;
                node.expanded = true;
                var nodeId = node.ID.ToString();

                //只获得节点下的子节点
                var result2 = _service.GetBoByBot(bot, nodeId);

                if (result2.Count > 0)
                {
                    node.isLeaf = false;
                    node.expanded = false;
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除业务对象
        /// </summary>
        /// <param name="boinfo"></param>
        /// <returns></returns>
        public JsonResult DeleteBo(string boinfo)
        {
            var data = new Dictionary<string, string>();

            var boinfos = Server.UrlDecode(boinfo);
            var tokens = JArray.Parse(boinfos);

            try
            {
                foreach (var token in tokens)
                {
                    if (token.HasValues)
                    {
                        var id = token["ID"].ToString();
                        var alias = token["BOAlias"].ToString();
                        _service.DeleteBo(id, alias);
                    }
                }
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
        /// 更新业务对象类型
        /// </summary>
        /// <returns></returns>
        public JsonResult UpdateBo(string bo)
        {

            var data = new Dictionary<string, string>();
            try
            {
                _service.UpdateBo(JsonToBoBaseInfoModel(bo));
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
        /// <returns></returns>
        public JsonResult InsertBo(string bo)
        {
            var data = new Dictionary<string, string>();
            try
            {
                _service.AddBo(JsonToBoBaseInfoModel(bo));
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
        /// json 转化为实体
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        private BoBaseInfoModel JsonToBoBaseInfoModel(string json)
        {
            var model = new BoBaseInfoModel();
            var userName = User.Identity.GetUserName();
            var words = Server.UrlDecode(json);
            var token = JObject.Parse(words);

            if (token.HasValues)
            {
                var id = token["ID"].ToString();
                var pid = token["PID"].ToString();
                var bot = token["BOT"].ToString();
                var sid = token["SID"].ToString();
                var name = token["Name"].ToString();
                var boalias = token["Alias"].ToString();
                var location = token["SpaceLocationArea"].ToString();
                var orderIndex = token["OrderIndex"].ToString();
                var remark = token["Remark"].ToString();
                var sourceId = token["SourceID"].ToString();
                var sourceDb = token["SourceDB"].ToString();
                var sourceTable = token["SourceTable"].ToString();

                model = new BoBaseInfoModel()
                {
                    BOT = bot,
                    SID = sid,
                    Name = name,
                    Alias = boalias,
                    SpaceLocationArea = location,
                    Remark = remark,
                    SourceID = sourceId,
                    SourceDB = sourceDb,
                    SourceTable = sourceTable,
                    CreatedBy = userName,
                    CreatedDate = DateTime.Now,
                    LastUpdatedBy = userName,
                    LastUpdatedDate = DateTime.Now,
                    OrderIndex = Convert.ToInt32(orderIndex)
                };

                if (string.IsNullOrEmpty(id))
                {
                    model.ID = Guid.NewGuid();
                }
                else
                {
                    model.ID = Guid.Parse(id);
                }
                if (string.IsNullOrEmpty(pid))
                {
                    model.PID = null;
                }
                else
                {
                    model.PID = Guid.Parse(pid);
                }
            }
            return model;
        }

    }
}
