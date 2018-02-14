using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jurassic.So.GeoTopic.DataService;
using Jurassic.So.GeoTopic.DataService.Models;
using Jurassic.So.GeoTopic.DataService.Tool;
using Jurassic.WebFrame;

namespace Jurassic.So.GeoTopic.Web.Controllers
{
    public class GeoTopicCodeController : BaseController
    {

        public ICodeOperateService codeOperateService { get; set; }


        public GeoTopicCodeController(ICodeOperateService codeService)
        {
            codeOperateService = codeService;
        }


        // GET: GeoTpoicCode
        public ActionResult CodeOperateView()
        {
            return View();
        }


        /// <summary>
        /// 获取所有的码表类型
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllCodeType()
        {
            var result=codeOperateService.GetAllCodeType();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取码表记录
        /// </summary>
        /// <param name="codeTypeId"></param>
        /// <returns></returns>
        public JsonResult GetCodes(int? codeTypeId, string code,string title)
        {
            var dataResult=new List<CodeModel>();
            //var pageIndex = Convert.ToInt32(Request["pageIndex"]);
            //var pageSize = Convert.ToInt32(Request["pageSize"]);
            var result= codeOperateService.GetCodes(codeTypeId);

            if (!string.IsNullOrEmpty(code))
            {
                code = HttpUtility.UrlDecode(code);
                result = result.FindAll(t => t.Code.Contains(code.Trim()));
            }

            if (!string.IsNullOrEmpty(title))
            {
                title=HttpUtility.UrlDecode(title);
                result = result.FindAll(t => t.Title.Contains(title.Trim()));
            }


            //if (pageSize != 0)
            //{
            //    dataResult = result.Skip(pageIndex * pageSize).Take(pageSize).ToList();
            //}
            //else
            //{
            //    dataResult = result;
            //}
            //return Json(new{count=result.Count, data = dataResult }
            //, JsonRequestBehavior.AllowGet);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 编辑码表信息
        /// </summary>
        /// <param name="codeModel">传入码表对象</param>
        public void UpdateCode(string codeModelStr)
        {
            if(string.IsNullOrEmpty(codeModelStr))
                return;
            codeModelStr = HttpUtility.UrlDecode(codeModelStr);
            List<CodeModel> models= JsonUtil.JsonToObject(codeModelStr, typeof(List<CodeModel>)) as List<CodeModel>;
            if(models!=null&&models.Count>0)
                codeOperateService.UpdateCode(models);
        }

    }
}