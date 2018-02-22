using Jurassic.WebFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jurassic.So.SpiderTool.IService;

namespace Jurassic.So.SpiderTool.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class AdapterPriorityController : BaseController
    {
        /// <summary>
        /// 适配器服务接口
        /// </summary>
        public IAdapterManageService adapterInfoService { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="service"></param>
        public AdapterPriorityController(IAdapterManageService service)
        {
            adapterInfoService = service;
        }

        /// <summary>
        /// 获得AdapterPriority
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取重复的数据类型列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetDuplicateDataTypeList()
        {
            var data = adapterInfoService.GetDataTypeDuplicateList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 通过选择的数据类型获得适配器列表
        /// </summary>
        /// <param name="AdapterDataType"></param>
        /// <returns></returns>
        public JsonResult GetAdapterListFromDataType(string AdapterDataType)
        {
            var data = adapterInfoService.GetAdapterInfoListFromDataType(AdapterDataType);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 交换相同数据类型适配的优先级
        /// </summary>
        /// <param name="DataType">数据类型</param>
        /// <param name="AdapterId1">适配器1ID</param>
        /// <param name="AdapterId2">适配器2ID</param>
        /// <returns></returns>
        public JsonResult ExchangeAdapterTypePriority(string DataType, string AdapterId1, string AdapterId2)
        {
            adapterInfoService.ExchangeAdapterTypePriority(DataType, AdapterId1, AdapterId2);
            return Json(new { sucess = true }, JsonRequestBehavior.AllowGet);
        }

    }
}
