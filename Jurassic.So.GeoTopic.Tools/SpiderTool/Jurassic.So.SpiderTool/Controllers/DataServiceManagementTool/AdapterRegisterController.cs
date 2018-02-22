using System;
using System.Collections.Generic;
using System.Linq;
using Jurassic.WebFrame;
using System.Web.Mvc;
using System.IO;
using Jurassic.AppCenter;
using Jurassic.So.Data.Entities;
using Jurassic.So.Data;
using Jurassic.So.SpiderTool.IService;
using Jurassic.So.SpiderTool.Service;
using Jurassic.So.SpiderTool.Service.Util;
using Jurassic.PKS.Service.Adapter;

namespace Jurassic.So.SpiderTool.Controllers
{
    public class AdapterRegisterController : BaseController
    {

        public IAdapterManageService adapterInfoService { get; set; }

        public AdapterRegisterController(IAdapterManageService service)
        {
            adapterInfoService = service;
        }

        //
        // GET: /AdapterRegister/

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 显示注册适配器页面
        /// </summary>
        /// <returns></returns>
        public ActionResult RegisterAdapterAction()
        {
            return View("RegisterAdapter");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAdapterList(int pageIndex, int pageSize, string adapterName, string status, string hangup)
        {
            var data = adapterInfoService.GetAdapterInfoPageList(adapterName, status, hangup, pageIndex, pageSize);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 挂起
        /// </summary>
        /// <param name="adapterId"></param>
        /// <returns></returns>
        public JsonResult Hangup(string adapterId)
        {
            adapterInfoService.HangupAdapter(adapterId, User.Identity.GetUserName());
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 加载某一适配器下的数据类型
        /// </summary>
        /// <param name="adapterId"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public JsonResult Details(string adapterId, string dataType)
        {
            var list = adapterInfoService.GetDataTypeList(adapterId, dataType);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 将适配器数据写入数据库
        /// </summary>
        /// <param name="IsLocalAdapter"></param>
        /// <param name="AdapterName"></param>
        /// <param name="ds_AI"></param>
        /// <param name="adapter"></param>
        /// <param name="adapterInfoCom"></param>
        /// <returns></returns>
        private JsonResult UpDateAdapterToDataBase(GT_AdapterInfo ds_AI, AdapterInfo adapterInfoCom, List<Scope> adapterDataTypesCom)
        {
            int result = adapterInfoService.RegisterAdapter(adapterInfoCom, ds_AI, adapterDataTypesCom, User.Identity.GetUserName());
            switch (result)
            {
                case -1:
                    return Json(new { duplicatAdapterName = true }, JsonRequestBehavior.AllowGet);
                case -2:
                    return Json(new { duplicatAdapterDataType = true }, JsonRequestBehavior.AllowGet);
                case -3:
                    return Json(new { duplicatAdapterId = true }, JsonRequestBehavior.AllowGet);
                case 0:
                    return Json(new { sucess = false }, JsonRequestBehavior.AllowGet);
                default:
                    return Json(adapterInfoCom, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 注册或测试远程适配器
        /// </summary>
        /// <param name = "adapterUrl" ></param >
        /// <param name="forTest"></param>
        /// <param name = "adapterName"></param >
        /// <returns ></returns >
        public JsonResult RegisterRemoteAdapter(string adapterUrl, bool forTest, string adapterName)
        {
            if(adapterUrl.EndsWith("/GetCapabilities"))
            {
                adapterUrl = adapterUrl.Substring(0, adapterUrl.LastIndexOf("/GetCapabilities"));
            }
            GT_AdapterInfo ds_AI = new GT_AdapterInfo
            {
                AdapterURL = adapterUrl,
                ClassName = "",
                ConfigAddress = "",
                InvokeType = 1
            };
            //获得适配器实例
            var adapter = AdapterFactory.CreateAdapter(ds_AI);
            AdapterInfo adapterInfoCom;
            //调用适配器AdapterInfo方法
            try
            {
                adapterInfoCom = adapter.GetAdapterInfo();
            }
            catch (Exception ex)
            {
                Log.WriteError("调用适配器AdapterInfo方法出错", ex);
                return Json(new { sucess = false }, JsonRequestBehavior.AllowGet);
            }
            if (forTest)  //测试适配器
            {
                return Json(adapterInfoCom, JsonRequestBehavior.AllowGet);
            }
            adapterInfoCom.Id = adapterName;
            AdapterInfo adapterSpiderScopesCom;
            //调用适配器SpiderScopes方法
            try
            {
                adapterSpiderScopesCom = adapter.GetAdapterInfo();
            }
            catch (Exception ex)
            {
                Log.WriteError("调用适配器DataTypes方法出错", ex);
                return Json(new { sucess = false }, JsonRequestBehavior.AllowGet);
            }
            //从接口获取适配器数据类型信息
            return UpDateAdapterToDataBase(ds_AI, adapterInfoCom, adapterSpiderScopesCom.Scopes);
        }

        /// <summary>
        /// 反注册适配器
        /// </summary>
        /// <param name="adapterId">适配器ID</param>
        /// <returns></returns>
        public JsonResult UnRegisterAdapter(string adapterId)
        {
            string adapterPath = adapterInfoService.UnRegisterAdapter(adapterId);
            if (!string.IsNullOrEmpty(adapterPath) && adapterPath != "-1")
            {
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { existAdapterDataSourceAuthorize = true }, JsonRequestBehavior.AllowGet);
        }

    }
}
