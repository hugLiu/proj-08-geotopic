using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
//using System.Web.Http;
using System.Web.Mvc;
using System.Web.UI;
using Jurassic.Com.Tools;
using Jurassic.So.GeoTopic.DataService;
using Jurassic.So.GeoTopic.DataService.Enum;
using Jurassic.So.GeoTopic.DataService.Models;
using Jurassic.So.GeoTopic.DataService.Tool;
using Jurassic.WebFrame;

namespace Jurassic.So.GeoTopic.Web.Controllers
{
    [System.Web.Mvc.Authorize]
    public partial class PageDetailsController
    {
        //_data保存单一Tab页信息
        private string _data = string.Empty;
        public JsonResult GetLayoutTabs(int topicId)
        {
            var tCards = pageDetailsService.GetLayoutTabs(topicId);
            var result = MappingToTabModel(tCards);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取当前TCard页面内容信息
        /// </summary>
        /// <param name="Id">TCardId</param>
        /// <param name="data">TCard模型数据</param>
        /// <returns></returns>
        public ActionResult TCardContent(string data)
        {
            data = HttpUtility.UrlDecode(data);
            var code = HttpUtility.UrlEncode(data);
            //_data = code;
            //对参数进行编码
            ViewBag.Data = code;

            return View();
        }

        /// <summary>
        /// 跳转到知识卡页面
        /// </summary>
        /// <param name="data">知识卡数据</param>
        /// <param name="gridData">成果列表数据</param>
        /// <returns></returns>
        public ActionResult TCardLayout(string data, string gridData)
        {
            var code = HttpUtility.UrlEncode(data);
            ViewBag.Data = code;
            var gridDataCode = HttpUtility.UrlEncode(gridData);
            ViewBag.GridData = gridDataCode;
            return View();
        }

        /// <summary>
        /// 跳转到可视化布局页面
        /// </summary>
        /// <param name="data">知识卡数据</param>
        /// <param name="gridData">成果列表数据</param>
        /// <returns></returns>
        public ActionResult LayoutVisual(string data, string gridData)
        {
            var code = HttpUtility.UrlEncode(data);
            ViewBag.Data = code;
            var gridDataCode = HttpUtility.UrlEncode(gridData);
            ViewBag.GridData = gridDataCode;

            return View();
        }


        /// <summary>
        /// 跳转到数据导入参数设置
        /// </summary>
        /// <returns></returns>
        public ActionResult ImportParams()
        {
            return View();
        }

        /// <summary>
        ///跳转到知识卡片属性编辑页面
        /// </summary>
        /// <returns></returns>
        public ActionResult TCardEdit()
        {
            return View();
        }

        /// <summary>
        /// 删除已有知识卡
        /// </summary>
        /// <param name="Id">删除知识卡Id</param>
        public void DeleteTCard(int Id)
        {
            pageDetailsService.DeleteTcard(Id);
        }


        /// <summary>
        /// 将知识卡模型转化成前端Tab知识卡模型
        /// </summary>
        /// <param name="tCardModels">知识卡模型集合</param>
        /// <returns></returns>
        public List<TabModel> MappingToTabModel(List<TCardModel> tCardModels )
        {
            var tabModels = new List<TabModel>();
            foreach (var tm in tCardModels)
            {
                var taModel = new TabModel()
                {
                    Id=tm.Id,
                    title = tm.Title,
                    OrderId = tm.OrderId,
                    TopicId = tm.TopicId,
                    Definition = tm.Definition,
                    showCloseButton="true"
                };
                tabModels.Add(taModel);
            }
            return tabModels;
        }

        /// <summary>
        /// 保存当前知识卡
        /// </summary>
        /// <param name="data">知识卡数据</param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [ValidateInput(false)]
        public JsonResult SaveTCardLayout(string data)
        {
            data = HttpUtility.UrlDecode(data);
            if (string.IsNullOrEmpty(data))
                return Json("null");
            TabModel model = JsonUtil.JsonToObject(data, typeof(TabModel)) as TabModel;
            if(model==null)
                return Json("null");
            //保存tab页信息到后台
            try
            {
                pageDetailsService.SaveGeoTCard(model.Id, model.TopicId, model.title, model.Definition, model.OrderId);
            }
            catch (Exception ex)
            {
            }
            return Json("succ",JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 保存当前知识卡-不经过模型转换
        /// </summary>
        /// <param name="data">知识卡数据</param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [ValidateInput(false)]
        public JsonResult SaveTCardLayoutNoConvert(string data)
        {
            data = HttpUtility.UrlDecode(data);
            if (string.IsNullOrEmpty(data))
                return Json("null");
            TabModel model = JsonUtil.JsonToObject(data, typeof(TabModel)) as TabModel;

            if (model == null)
                return Json("null");

            //保存tab页信息到后台
            pageDetailsService.SaveGeoTCardNoConvert(model.Id, model.TopicId, model.title, model.Definition, model.OrderId);
            return Json("succ", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 更新知识卡数据
        /// </summary>
        /// <param name="data">当前知识卡数据</param>
        /// <returns>返回"succ"成功标志</returns>
        [System.Web.Mvc.HttpPost]
        [ValidateInput(false)]
        public JsonResult UpdateGeoTCard(string data)
        {
            data = HttpUtility.UrlDecode(data);
            if (string.IsNullOrEmpty(data))
                return Json("null");
            TabModel model = JsonUtil.JsonToObject(data, typeof(TabModel)) as TabModel;

            if (model == null)
                return Json("null");

            //保存tab页信息到后台
            pageDetailsService.UpdateGeoTCard(model.Id, model.TopicId, model.title, model.Definition, model.OrderId);
            return Json("succ", JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 设置布局样式和参数
        /// </summary>
        /// <param name="id">当前知识卡Id</param>
        /// <returns></returns>
        public JsonResult GetKCardLayout(string id)
        {
            if (string.IsNullOrEmpty(id))
                return Json("null");
            int Id = Convert.ToInt16(id);
            var tCard = pageDetailsService.GetTCardContent(Id);
            if (tCard != null)
            {
                return Json(tCard.Definition,JsonRequestBehavior.AllowGet);
            }
            return Json("null");
        }


        /// <summary>
        /// 设置布局样式和参数,如上方法，不需经过模型转换
        /// </summary>
        /// <param name="id">当前知识卡Id</param>
        /// <returns></returns>
        public JsonResult GetKCardLayoutNoConvert(string id)
        {
            if (string.IsNullOrEmpty(id))
                return Json("null");
            int Id = Convert.ToInt16(id);
            var tCard = pageDetailsService.GetGeoTCardById(Id);
            if (tCard != null)
            {
                return Json(tCard.Definition, JsonRequestBehavior.AllowGet);
            }
            return Json("null");
        }
  
        /// <summary>
        /// 返回设置导入模型模板参数
        /// </summary>
        /// <returns></returns>
        public JsonResult GetImportParamsResult()
        {
            // 获得程序路径
            string tempFilePath = Request.PhysicalApplicationPath;
            var upLoadFile = Request.Files["Fdata"];
            //放置文件路径

            var folderPath = tempFilePath + EnumSheetName.ImportFileCache + "\\";
            string filePath = folderPath + upLoadFile.FileName;
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath.Substring(0, filePath.LastIndexOf("\\")));
            }
            upLoadFile.SaveAs(filePath);

            //找到目标文件对象 ，获取选择模板专业信息
            List<ExcelEntityModel> excelEntityModels=importParamsService.ReadDataFromImportExcel(filePath);

            //删除清空文件夹
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            //返回在界面中
            return Json(excelEntityModels, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 下载模板文件
        /// </summary>
        /// <returns></returns>
        public JsonResult DonwnLoadTempFile()
        {
            string folderName = EnumSheetName.templateFolder;
            ExportExcelTemplate(folderName);
            importParamsService.DonwnLoadFile("/"+ folderName + "/"+ EnumSheetName.newFileName);  //传入相对路径s
            return Json("", JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 导出知识主题链数据
        /// </summary>
        /// <param name="folderName">文件夹名称</param>
        public void ExportExcelTemplate(string folderName)
        {
            string tempFilePath = Request.PhysicalApplicationPath;
            //导出文件
            string filePath = tempFilePath+ folderName + "\\" + EnumSheetName.newFileName;
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath.Substring(0, filePath.LastIndexOf("\\")));
            }
            importParamsService.ExportExcelTemplate(filePath);
        }

        /// <summary>
        /// 导出知识链数据到知识模板文件
        /// </summary>
        /// <param name="nodeParams"></param>
        /// <returns></returns>
        public JsonResult DownLoadDataFile(string nodeParams)
        {
            nodeParams = HttpUtility.UrlDecode(nodeParams);
            List<KTopicModel> nodeModelList = JsonUtil.JsonToObject(nodeParams, typeof(List<KTopicModel>)) as List<KTopicModel>;
            if (nodeModelList == null || nodeModelList.Count <= 0)
                return Json("知识链没有数据，无法导出！", JsonRequestBehavior.AllowGet);
            string tempFilePath = Request.PhysicalApplicationPath;

            string folderName= EnumSheetName.dataFolder;
            string filePath = tempFilePath + folderName + "\\" + EnumSheetName.dataFileName;
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath.Substring(0, filePath.LastIndexOf("\\")));
            }
            importParamsService.ExportDataToExcel(filePath, nodeModelList);
            return Json("", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        public void DonwnLoadDataFile()
        {
            string folderName = EnumSheetName.dataFolder;
            importParamsService.DonwnLoadFile("/" + folderName + "/" + EnumSheetName.dataFileName);
        }

        /// <summary>
        /// 获取元数据定义集合
        /// </summary>
        /// <returns></returns>
        public JsonResult GetMetaDataDefinition()
        {
            var metaDataList=wrapedDataSearchService.GetMetadataDefinition();
            var paramList = metaDataList.Select(t => new  { Title = t.Title, Name = t.Name});
            return Json(paramList, JsonRequestBehavior.AllowGet);
        }

    }


    public class TabModel
    {
        public int Id { get; set; }
        public int TopicId { get; set; }
        public string title { get; set; }
        public string Definition { get; set; }
        public string showCloseButton { get; set; }
        public int OrderId { get; set; }

    }
}
