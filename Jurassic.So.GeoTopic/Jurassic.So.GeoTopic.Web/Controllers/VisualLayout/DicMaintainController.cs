using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Jurassic.So.GeoTopic.DataService;
using Jurassic.So.GeoTopic.DataService.Models;
using Jurassic.So.GeoTopic.DataService.Tool;
using Jurassic.WebFrame;
using Jurassic.So.GeoTopic.DataService.Service;

namespace Jurassic.So.GeoTopic.Web.Controllers
{
    [Authorize]
    public class DicMaintainController : BaseController
    {
        public IDicMaintainService dicMaintainService { get; set; }
        public IDictTypeService dictTypeService { get; set; }
        public DicMaintainController(IDicMaintainService service,IDictTypeService type)
        {
            dicMaintainService = service;
            dictTypeService = type;
        }

        /// <summary>
        /// 字典维护主界面
        /// </summary>
        /// <returns></returns>
        public ActionResult DicMaintain()
        {
            return View();
        }

        /// <summary>
        /// 获取主题索引
        /// </summary>
        /// <returns></returns>
        public JsonResult GetDictIndexs()
        {
            return Json(dicMaintainService.GetDictIndexs(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取默认参数模板
        /// </summary>
        /// <returns></returns>
        public JsonResult GetPTemps()
        {
            return Json(dicMaintainService.GetPTemps(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据urlId获取默认模板
        /// </summary>
        /// <param name="urlId">展示页面Id</param>
        /// <returns></returns>
        public JsonResult GetPTempByUrlId(int urlId)
        {
            return Json(dicMaintainService.GetPTempsByUrlId(urlId), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据url获取默认模板
        /// </summary>
        /// <param name="urlId">展示页面Id</param>
        /// <returns></returns>
        public JsonResult GetPTempByUrl(string url)
        {
            url = HttpUtility.UrlDecode(url);
            return Json(dicMaintainService.GetPTempsByUrl(url), JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 保存主题索引
        /// </summary>
        /// <param name="nodeParams">主题索引节点信息</param>
        public JsonResult SaveDictIndexs(string nodeParams)
        {
            nodeParams = HttpUtility.UrlDecode(nodeParams);
            List<Dic_tIndexModel> models = JsonUtil.JsonToObject(nodeParams, typeof(List<Dic_tIndexModel>)) as List<Dic_tIndexModel>;
            if (/*models != null &&*/ models.Count > 0)
            {
                //try
                //{
                dicMaintainService.UpdateGeoDicTIndexs(models);
                return Json(new PromptMessage()
                {
                    Type = MessageType.success,
                    Message = "保存成功！"
                }, JsonRequestBehavior.AllowGet);
                //}
                //catch (Exception)
                //{
                //    return Json(new PromptMessage()
                //    {
                //        Type = MessageType.error,
                //        Message = "保存失败！"
                //    }, JsonRequestBehavior.AllowGet);
                //}           
            }
            else
            {
                return Json(new PromptMessage() {
                    Type=MessageType.info,
                    Message="无数据！"
                },JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 保存默认参数模板
        /// </summary>
        /// <param name="nodeParams"></param>
        public JsonResult SavePTemps(string data)
        {
            data = HttpUtility.UrlDecode(data);

            PTempModel models = JsonUtil.JsonToObject(data, typeof(PTempModel)) as PTempModel;
            if (models != null)
            {
                dicMaintainService.UpdateGeoPTemps(models);
                return Json("succ",JsonRequestBehavior.AllowGet);
            }
            return Json("fail", JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 查询所有类型
        /// </summary>
        /// <returns></returns>
        public JsonResult GetType()
        {
            var list = dictTypeService.GetTypeData();        
            return Json(list,JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 类型的增删改
        /// </summary>
        /// <param name="typeParams"></param>
        public JsonResult SaveDictType(string typeParams)
        {
            typeParams = HttpUtility.UrlDecode(typeParams);
            List<Dic_tTypeModel> models = JsonUtil.JsonToObject(typeParams, typeof(List<Dic_tTypeModel>)) as List<Dic_tTypeModel>;
            if (models != null && models.Count > 0)
            {
                try
                {
                    dictTypeService.UpdateDictType(models, CurrentUser.Name);
                    return Json(new PromptMessage() {
                        Type=MessageType.success,
                        Message="保存成功！"
                    }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(new PromptMessage()
                    {
                        Type = MessageType.error,
                        Message = "保存失败！"
                    }, JsonRequestBehavior.AllowGet);
                }
                
            }
            return Json(new PromptMessage() {
                Type=MessageType.info,
                Message="没有数据改变！"
            },JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 根据展示类型查询所有展示页面
        /// </summary>
        /// <returns></returns>
        public JsonResult QueryUrl()
        {
            int typeId =Convert.ToInt32(Request["TypeId"]);
            var list = dictTypeService.QueryUrlData(typeId);
            return Json(list,JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存展示页面信息
        /// </summary>
        /// <param name="urlParams">展示页面编辑节点</param>
        /// <param name="TypeId">展示类型Id</param>
        /// <returns></returns>
        public JsonResult SaveDictUrl(string urlParams, int TypeId)
        {
            urlParams = HttpUtility.UrlDecode(urlParams);
            List<Dic_tUrlModel> models = JsonUtil.JsonToObject(urlParams, typeof(List<Dic_tUrlModel>)) as List<Dic_tUrlModel>;
            if (models != null && models.Count > 0)
            {
                try
                {
                    dictTypeService.UpdataDictUrl(models, TypeId, CurrentUser.Name);
                    return Json(new PromptMessage()
                    {
                        Type = MessageType.success,
                        Message = "保存成功！"
                    },JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {

                    return Json(new PromptMessage() {
                        Type=MessageType.error,
                        Message="保存失败！"
                    });
                }
                
            }
            return Json(new PromptMessage()
            {
                Type = MessageType.info,
                Message = "没有数据改变！"
            });
        }

       /// <summary>
       /// 查询所有的展示类型
       /// </summary>
       /// <returns></returns>
        public JsonResult QueryAllType()
        {
            List<Dic_tTypeModel> list = dictTypeService.QueryDType();
            return Json(list,JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 查询展示页面
        /// </summary>
        /// <param name="typeId">展示类型Id</param>
        /// <returns></returns>
        public JsonResult QueryAllUrl(string typeId)
        {
            List<Dic_tUrlModel> list = dictTypeService.QueryDUrl(Convert.ToInt32(typeId));
            return Json(list,JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取所有展示页面
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllUrl()
        {
            return Json(dictTypeService.QueryAllUrl(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 通过展示的Code值获取展示页面
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllUrlByTypeCode()
        {
            return Json(dictTypeService.QueryAllUrlByTypeCode(), JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// 动态加载参数局部视图
        /// </summary>
        /// <param name="viewName">局部视图名称</param>
        /// <param name="mainName">主视图名称</param>
        /// <returns></returns>
        public PartialViewResult DynamicLoadParamsView(string viewName,string mainName)
        {
            viewName = HttpUtility.UrlDecode(viewName);
            mainName= HttpUtility.UrlDecode(mainName);
            ViewBag.MainName = mainName;
            return PartialView(viewName);
        }
    }
  
}
