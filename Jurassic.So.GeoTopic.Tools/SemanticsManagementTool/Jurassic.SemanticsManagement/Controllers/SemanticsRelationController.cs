using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Jurassic.AppCenter;
using Jurassic.Semantics.IService.ViewModel;
using Jurassic.Semantics.Service;
using Jurassic.WebFrame;
using Newtonsoft.Json.Linq;

namespace Jurassic.SemanticsManagement.Controllers
{
    /// <summary>
    /// 语义关系维护
    /// </summary>
    public class SemanticsRelationController : BaseController
    {
        private SemanticsManageService _service;

        /// <summary>
        /// 语义关系维护视图页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {

            return View();
        }

        /// <summary>
        /// 根据概念类code获得树
        /// </summary>
        /// <param name="cc"></param>
        /// <param name="term"></param>
        /// <param name="sr"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult CCTrees(string cc, string term, string sr, string id)
        {
            ViewBag.cc = cc;
            ViewBag.sr = sr;
            ViewBag.term = term;
            ViewBag.selectdId = id;
            return View();
        }

        /// <summary>
        /// 服务注入
        /// </summary>
        /// <param name="service"></param>
        public SemanticsRelationController(SemanticsManageService service)
        {
            _service = service;
        }

        /// <summary>
        /// 根据叙词和正向语义关系来获得对应概念类的叙词列表
        /// </summary>
        /// <param name="termid">叙词ID</param>
        /// <param name="sr">语义关系</param>
        /// <returns>返回对应的叙词信息</returns>
        public JsonResult GetSemantics(string termid, string sr)
        {
            return Json(_service.GetSemantics(termid, sr), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据叙词和反向语义关系来获得对应概念类的叙词列表
        /// </summary>
        /// <param name="term">叙词</param>
        /// <param name="sr">语义关系</param>
        /// <returns>返回对应的叙词信息</returns>
        public JsonResult GetReverseSemantics(string term, string sr)
        {
            return Json(_service.GetReverseSemantics(term, sr), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据概念类获得所有叙词并以树结构展示
        /// </summary>
        /// <param name="cc">概念类ccode</param>
        /// <returns></returns>
        public JsonResult GetTermTrees(string cc)
        {
            var result = _service.GetTermTrees(cc);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获得需要维护的概念类的列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetNeedManageCc()
        {
            var dics = _service.GetNeedManageCC();
            var results = new List<Model>();
            foreach (var dic in dics)
            {
                results.Add(new Model(dic.Key, dic.Value));
            }
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获得指定概念类需要维护的关系列表
        /// </summary>
        /// <param name="cc">概念类ccode</param>
        /// <returns></returns>
        public JsonResult GetRelationOfCc(string cc)
        {
            var results = _service.GetRelationOfCC(cc);
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加新的语义关系到语义表中（重复的不再插入）
        /// </summary>
        /// <param name="s"></param>
        public void SaveSemantics(string s)
        {
            _service.SaveSemantics(JsonToSemantics(s));
        }

        /// <summary>
        /// 删除语义关系
        /// </summary>
        /// <param name="s"></param>
        public void DeleteSemantics(string s)
        {
            _service.DeleteSemantics(JsonToSemantics(s));
        }

        private List<SemanticsModel> JsonToSemantics(string json)
        {
            var semantics = new List<SemanticsModel>();
            var userName = User.Identity.GetUserName();
            var words = Server.UrlDecode(json);
            var tokens = JArray.Parse(words);
            if (tokens.HasValues)
            {
                var i = 0;
                foreach (var token in tokens)
                {
                    var fTermClassId = token["FTermClassId"].ToString();
                    var sr = token["SR"].ToString();
                    var lTermClassId = token["LTermClassId"].ToString();
                    var fTerm = token["FTerm"].ToString();
                    var lTerm = token["LTerm"].ToString();
                    var model = new SemanticsModel()
                    {
                        FTermClassId = Guid.Parse(fTermClassId),
                        LTermClassId = Guid.Parse(lTermClassId),
                        FTerm = fTerm,
                        LTerm = lTerm,
                        SR = sr,
                        CreatedBy = userName,
                        CreatedDate = DateTime.Now,
                        LastUpdatedBy = userName,
                        LastUpdatedDate = DateTime.Now,
                        OrderIndex = i++
                    };
                    semantics.Add(model);
                }
            }
            return semantics;
        }
    }
}

/// <summary>
/// 视图展示模型
/// </summary>
public class Model
{
    /// <summary>
    /// id
    /// </summary>
    public string Id { get; set; }
    /// <summary>
    /// 文本展示
    /// </summary>
    public string Text { get; set; }

    public Model(string id, string text)
    {
        Id = id;
        Text = text;
    }

}

