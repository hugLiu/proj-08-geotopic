using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jurassic.So.GeoTopic.DataService;
using Jurassic.So.GeoTopic.DataService.Models;
using Jurassic.So.GeoTopic.DataService.Service;
using Jurassic.So.GeoTopic.DataService.Tool;
using Jurassic.WebFrame;

namespace Jurassic.So.GeoTopic.Web.Controllers
{
    [Authorize]
    public partial class PageDetailsController : BaseController
    {
        public IPageDetailsService pageDetailsService { get; set; }
        public IImportParamsService importParamsService { get; set; }

        public IWrapedDataSearchService wrapedDataSearchService { get; set; }

        //public IImportParamsService importParamsService=new ImportParamsService();



        public PageDetailsController(
            IPageDetailsService service,
            IImportParamsService importService,
            IWrapedDataSearchService searchService)
        {
            pageDetailsService = service;
            importParamsService = importService;
            wrapedDataSearchService = searchService;
        }

        /// <summary>
        /// 页面详细主页面
        /// </summary>
        /// <returns></returns>
        public ActionResult PageDetails()
        {
            return View();
        }

        /// <summary>
        /// 加载主题树信息
        /// </summary>
        /// <returns></returns>
        public JsonResult GetKTopicModels()
        {
            return Json(pageDetailsService.GetGeoKTopics(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取主题索引
        /// </summary>
        /// <param name="TopicId">主题Id</param>
        /// <returns></returns>
        public JsonResult GetTIndexs(string TopicId)
        {
            if (string.IsNullOrEmpty(TopicId))
                return null;
            return Json(pageDetailsService.GetTIndexs(TopicId), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取主题索引属性集合
        /// </summary>
        /// <returns></returns>
        public JsonResult GetDictIndexs()
        {
            return Json(pageDetailsService.GetDictIndexs(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 更新知识主题信息
        /// </summary>
        /// <param name="nodeParams"></param>
        public void SaveKTopic(string nodeParams)
        {
            nodeParams = HttpUtility.UrlDecode(nodeParams);
            List<KTopicModel> models = JsonUtil.JsonToObject(nodeParams, typeof(List<KTopicModel>)) as List<KTopicModel>;
            if (models != null && models.Count > 0)
            {
                pageDetailsService.UpdateGeoKTopics(models);
            }
        }

        /// <summary>
        /// 保存新的知识主题
        /// </summary>
        /// <param name="nodeParams">知识主题数据</param>
        /// <returns></returns>
        public int SaveNewNode(string nodeParams)
        {
            nodeParams = HttpUtility.UrlDecode(nodeParams);
            int id = 0;
            List<KTopicModel> models = JsonUtil.JsonToObject(nodeParams, typeof(List<KTopicModel>)) as List<KTopicModel>;
            if (models != null && models.Count == 1)
            {
                id = pageDetailsService.AddNewNodeToGeoKTopics(models.First());
            }
            return id;
        }

        /// <summary>
        /// 更新主题节点对应索引属性
        /// </summary>
        /// <param name="nodeParams"></param>
        public void SaveTIndex(string nodeParams)
        {
            nodeParams = HttpUtility.UrlDecode(nodeParams);
            List<TIndexModel> models = JsonUtil.JsonToObject(nodeParams, typeof(List<TIndexModel>)) as List<TIndexModel>;
            if (models != null && models.Count > 0)
            {
                pageDetailsService.UpdateGeoTIndexs(models);
            }
        }


        /// <summary>
        /// 保存导入主题属性数据模型
        /// </summary>
        /// <param name="paramStr"></param>
        public void SaveImportTindex(string paramStr)
        {
            paramStr = HttpUtility.UrlDecode(paramStr);
            List<ImportTIndexModel> models = JsonUtil.JsonToObject(paramStr, typeof (List<ImportTIndexModel>)) as List<ImportTIndexModel>;
            if (models != null && models.Count > 0  )
            {
                pageDetailsService.UpdateImportTIndexs(models);
            }
        }

        /// <summary>
        /// 保存导入知识卡片数据
        /// </summary>
        /// <param name="paramStr">导入知识卡数据</param>
        public void SaveImportTCard(string paramStr)
        {
            paramStr = HttpUtility.UrlDecode(paramStr);
            List<TCardModel> tCardModels = JsonUtil.JsonToObject(paramStr, typeof(List<TCardModel>)) as List<TCardModel>;
            if (tCardModels != null && tCardModels.Count > 0&& !string.IsNullOrEmpty(tCardModels[0].Title))
            {
                pageDetailsService.UpdateImportTCard(tCardModels);
            }
        }


        public JsonResult ConvertToCardDef(string cardViewStr)
        {
            return Json(pageDetailsService.ConvertToCardDef(cardViewStr), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ConvertToCardView(string defModelStr)
        {
            return Json(pageDetailsService.ConvertToCardView(defModelStr), JsonRequestBehavior.AllowGet);
        }

    }
}
