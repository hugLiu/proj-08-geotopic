using Jurassic.AppCenter;
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
    /// 叙词维护
    /// </summary>
    public class GlossaryManageController : BaseController
    {
        private IGlossaryManageService Service { get; set; }
        private List<TermTreeModel> _list = null;

        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="service"></param>
        public GlossaryManageController(IGlossaryManageService service)
        {
            Service = service;
        }

        public ActionResult Index()
        {
            return View();
        }

        #region 1.显示树数据+Treeselect联动展示每个根节点的子节点+GetTree(string termGuid)
        public JsonResult GetCcTermResult(string strccterm)
        {
            strccterm = Server.UrlDecode(strccterm);
            _list = Service.GetTermTree(strccterm);
            return Json(_list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 2.显示不同概念类的来源+GetSourceListOfCC(string strccterm)
        /// <summary>
        /// 获得不同概念类的来源列表
        /// </summary>
        /// <param name="strccterm"></param>
        /// <returns></returns>
        public JsonResult GetSourceListOfCc(string strccterm)
        {
            var list = Service.GetSourceList(strccterm);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获得某个叙词的来源列表
        /// </summary>
        /// <param name="nodeId">叙词ID</param>
        /// <returns></returns>
        public JsonResult GetSourceListById(string nodeId)
        {
            var newList = Service.GetSourceListService(nodeId);
            return Json(newList, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 3.得到译文表数据+GetTermTraslations(Guid termGuid)
        /// <summary>
        /// 获得某个叙词的译文数据
        /// </summary>
        /// <param name="termClassId"></param>
        /// <returns></returns>
        public JsonResult GetTermTraslations(string termClassId)
        {
            var list = new List<TermTranslationModel>();
            try
            {
                list = Service.GetTermTraslations(Guid.Parse(termClassId));
            }
            catch (Exception e)
            {
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 4.得到别名表数据+GetTermAlias(Guid termClassId, string langauageCn)

        /// <summary>
        /// 获得某个叙词的别名列表
        /// </summary>
        /// <param name="termClassId"></param>
        /// <returns></returns>
        public JsonResult GetTermAlias(string termClassId)
        {
            var list = new List<SemanticsModel>();
            try
            {
                list = Service.GetTermAlias(Guid.Parse(termClassId));
            }
            catch (Exception)
            {
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 5.删除+DeleteCcTermByid(string model)
        public JsonResult DeleteCcTermByid(string id)
        {
            var termClassId = Guid.Parse(id);
            Service.DeleteCcTermByIdService(termClassId);
            return Json("", JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 9.添加子节点+AddChildNode(Guid termGuid)
        /// <summary>
        /// 添加子节点
        /// </summary>
        /// <param name="termGuid">需要添加</param>
        /// <param name="model"></param>
        /// <param name="textterm"></param>
        /// <returns></returns>
        public JsonResult AddChildNode(string termGuid, string model, string textterm)
        {
            var newList = JsonConvert.DeserializeObject<CcTermModel>(model);
            CcTermModel newmodel = new CcTermModel()
            {
                Term = newList.Term,
                CCCode = newList.CCCode,
                TermClassID = Guid.NewGuid(),
                CreatedBy = User.Identity.GetUserName(),//正式测试的时候使用
                CreatedDate = DateTime.Now,
                Description = newList.Description,
                LastUpdatedBy = User.Identity.GetUserName(), //正式测试的时候使用             
                LastUpdatedDate = DateTime.Now,
                OrderIndex = newList.OrderIndex,
                PathTerm = newList.PathTerm,
                Source = newList.Source,
                Remark = newList.Remark,
            };
            Service.AddChildrenNode(Guid.Parse(termGuid), newmodel, textterm);
            _list = Service.GetTermTree(textterm);
            return Json(_list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 11.编辑节点+EditTermById(Guid termGuid, string term)
        public JsonResult EditTermById(Guid termGuid, string term, string path)
        {
            Service.EditTreeById(termGuid, term, path);
            return Json("", JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 12.保存描述+SavsMs(string strMs, Guid termGuid)
        public JsonResult SavsMs(string strMs, Guid termGuid)
        {
            Service.SaveMs(strMs, termGuid);
            return Json("", JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 13.添加来源+GettermModel(string termSourceModel)
        public JsonResult GettermModel(string termSourceModel, string text)
        {
            var oldTermModel = JsonConvert.DeserializeObject<TermSourceModel>(termSourceModel);
            TermSourceModel newTermModel = new TermSourceModel()
            {
                ConceptClassId = oldTermModel.ConceptClassId,
                Source = oldTermModel.Source,
                CreateDate = DateTime.Now,
            };
            Service.AddSource(newTermModel, text);
            return Json("", JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 14.保存数据来源+SaveSourceResult(string source, Guid termGuid)
        public JsonResult SaveSourceResult(string source, Guid termGuid)
        {
            Service.SaveSource(source, termGuid);
            return Json("", JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 15.删除数据来源+DeleteBySourceName(string source)
        /// <summary>
        /// 根据数据来源名称删除来源
        /// </summary>
        /// <param name="source">来源名称</param>
        /// <returns></returns>
        public JsonResult DeleteBySourceName(string source)
        {
            Service.DeleteTermSourcrRowByName(source);
            return Json("", JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 16.保存译文+SavetotherNameResult(string model)
        public JsonResult SavetotherNameResult(string model, string Translation)
        {
            string userName = User.Identity.GetUserName();
            var newmodel = JsonConvert.DeserializeObject<TermTranslationModel>(model);
            newmodel.CreatedDate = DateTime.Now;
            newmodel.CreatedBy = userName;
            //newmodel.CreatedBy = "hemm";
            newmodel.LastUpdatedBy = userName;
            //newmodel.LastUpdatedBy = "hemm";
            newmodel.LastUpdatedDate = DateTime.Now;
            Service.SaveOtherName(newmodel);
            return Json("", JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 17.删除译文表数据+DeleteRowResult(string tuid)
        public JsonResult DeleteRowResult(string tuid, string tran)
        {
            Service.DeleteRow(Guid.Parse(tuid), tran);
            return Json("", JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 20.拖拽节点
        public JsonResult DragNodeResult(string sdModel, string dragmodel, string textValue)
        {
            var newSemantics = JsonConvert.DeserializeObject<SemanticsModel>(sdModel);
            SemanticsModel neseModel = new SemanticsModel()
            {
                CreatedBy = newSemantics.CreatedBy,
                CreatedDate = newSemantics.CreatedDate,
                FTerm = newSemantics.FTerm,
                FTermClassId = newSemantics.FTermClassId,
                LastUpdatedBy = User.Identity.GetUserName(),
                LastUpdatedDate = DateTime.Now,
                LTerm = newSemantics.LTerm,
                LTermClassId = newSemantics.LTermClassId,
                OrderIndex = newSemantics.OrderIndex,
                SR = newSemantics.SR,
                Remark = newSemantics.Remark,
            };
            var newCcterm = JsonConvert.DeserializeObject<CcTermModel>(dragmodel);
            CcTermModel newTermModel = new CcTermModel()
            {
                CCCode = newCcterm.CCCode,
                CreatedBy = newCcterm.CreatedBy,
                CreatedDate = newCcterm.CreatedDate,
                Description = newCcterm.Description,
                LastUpdatedBy = User.Identity.GetUserName(),
                LastUpdatedDate = DateTime.Now,
                OrderIndex = newCcterm.OrderIndex,
                PathTerm = newCcterm.PathTerm,
                Term = newCcterm.Term,
                Source = newCcterm.Source,
                Remark = newCcterm.Remark,
                TermClassID = newCcterm.TermClassID,
            };
            Service.DragNodeService(neseModel, newTermModel, textValue);
            return Json("", JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 21.排序

        public JsonResult RefreshResult(string nodeId, string parentName, int newOrderIndex)
        {
            Service.SequenceService(Guid.Parse(nodeId), parentName, newOrderIndex);
            Service.UpdatePathTerm(Guid.Parse(nodeId));
            return Json("", JsonRequestBehavior.AllowGet);
        }
        #endregion

        /// <summary>
        /// 添加别名
        /// </summary>
        /// <param name="semanticsModel"></param>
        /// <returns></returns>
        public JsonResult AddAlias(string semanticsModel)
        {
            var model = JsonConvert.DeserializeObject<SemanticsModel>(semanticsModel);
            Service.AddAlias(model);
            return Json("", JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 删除别名
        /// </summary>
        /// <param name="semanticsModel"></param>
        /// <returns></returns>
        public JsonResult DeleteAlias(string semanticsModel)
        {
            var model = JsonConvert.DeserializeObject<SemanticsModel>(semanticsModel);
            Service.DeleteAlias(model);
            return Json("", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获得概念类下拉选项
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllConceptClass()
        {
            var model = new List<ConceptClassModel>();
            var allconceprclass = Service.GetAllConceptClass();
            foreach (var conceptlist in allconceprclass)
            {
                model.Add(new ConceptClassModel(conceptlist.Key, conceptlist.Value));
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }

    /// <summary>
    /// 构造的实体模型
    /// 用作下拉显示
    /// </summary>
    public class ConceptClassModel
    {
        /// <summary>
        /// id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 文本展示
        /// </summary>
        public string Text { get; set; }

        public ConceptClassModel(string id, string text)
        {
            Id = id;
            Text = text;
        }
    }
}

