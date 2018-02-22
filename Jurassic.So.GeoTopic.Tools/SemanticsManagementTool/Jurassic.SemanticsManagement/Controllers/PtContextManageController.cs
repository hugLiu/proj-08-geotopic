using Jurassic.AppCenter;
using Jurassic.Semantics.IService.ViewModel;
using Jurassic.Semantics.Service;
using Jurassic.WebFrame;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace Jurassic.SemanticsManagement.Controllers
{
    /// <summary>
    /// 成果类型上下文维护
    /// </summary>
    public class PtContextManageController : BaseController
    {
        private readonly PtContextManageService _service;

        /// <summary>
        /// 服务注入
        /// </summary>
        /// <param name="service">pt上下文服务</param>
        public PtContextManageController(PtContextManageService service)
        {
            _service = service;
        }

        /// <summary>
        /// 成果类型上下文维护
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 概念类树弹出窗口
        /// </summary>
        /// <param name="cc">选中单元格所属的概念类</param>
        /// <param name="sr">选中单元格和成果类型的关系类型</param>
        /// <param name="ptid">选中单元格对应的成果类型id</param>
        /// <returns></returns>
        public ActionResult TermTree(string cc, string sr, string ptid)
        {
            ViewBag.cc = cc;
            ViewBag.sr = sr.Trim();
            ViewBag.ptid = ptid;
            return View();
        }
        /// <summary>
        /// 获得与PT有关系的关系类型
        /// </summary>
        /// <returns></returns>
        public JsonResult GetPtRelations()
        {
            var relations = _service.GetPtRelations();
            return Json(relations, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获得和pt
        /// </summary>
        /// <param name="sr"></param>
        /// <param name="ptid"></param>
        /// <returns></returns>
        public JsonResult GetPtRelationsOfCc(string ptid, string sr)
        {
            var relations = _service.GetPtRelationsOfCc( ptid, sr);
            return Json(relations, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptid"></param>
        /// <param name="sr"></param>
        /// <param name="term"></param>
        public void DeletePtSemantics(string ptid,string sr, string term)
        {
            _service.DeletePtSemantics(ptid,sr, term);
        }

        /// <summary>
        /// 获得不同概念类树
        /// </summary>
        /// <param name="cc"></param>
        /// <returns></returns>
        public JsonResult GetTermTree(string cc)
        {
            var data = _service.GetTermTree(cc);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获得pt上下文中某种概念类叙词列表（用于过滤）
        /// </summary>
        /// <param name="pt">成果类型过滤</param>
        /// <param name="field">字段名称</param>
        /// <returns></returns>
        public JsonResult GetTermGroup(string pt, string field)
        {
            //这里需要加成果类型的过滤
            var result = new List<ModelBase>();
            if (!string.IsNullOrEmpty(field))
            {
                var data = _service.GetTermGroup(pt, field);
                var i = 0;
                var enumerable = data as object[] ?? data.Cast<object>().ToArray();
                foreach (var o in enumerable)
                {
                    var term = JObject.Parse(JsonHelper.ToJson(o));
                    var id = i++.ToString(CultureInfo.InvariantCulture);
                    var text = term[field] ?? "空白";
                    result.AddRange(new[] { new ModelBase() { id = id, text = text.ToString() } });
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 单列 过滤pt上下文
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="filterItems"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public JsonResult FilterPtContext(string pt, string filterItems, string field)
        {
            var pageIndex = Convert.ToInt32(Request["pageIndex"]);
            var pageSize = Convert.ToInt32(Request["pageSize"]);
            var data = _service.FilterPtContext(pt, filterItems, field, pageIndex, pageSize);
            var totalCount = _service.GetFilterPtContextCount(pt, filterItems, field);

            var hs = new Hashtable();
            hs["data"] = data;
            hs["total"] = totalCount;
            return Json(hs, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///保存修改的PT上下文关系
        /// </summary>
        /// <param name="ptId">修改对应的PTID</param>
        /// <param name="pt">修改对应的pt</param>
        /// <param name="sr">修改的语义关系类型</param>
        /// <param name="ccId">叙词id</param>
        /// <param name="ccTerm">叙词</param>
        /// <returns></returns>
        public void SavePtContext(string ptId, string pt, string sr, string ccId, string ccTerm)
        {
            var userName = User.Identity.GetUserName();
            _service.SavePtContext(ptId, pt, sr, ccId, ccTerm, userName);
        }
    }
}
