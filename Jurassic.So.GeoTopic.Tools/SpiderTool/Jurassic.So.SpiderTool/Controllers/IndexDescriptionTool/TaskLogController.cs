using Jurassic.So.SpiderTool.IService;
using Jurassic.So.SpiderTool.IService.ViewModel;
using Jurassic.So.SpiderTool.Service;
using Jurassic.WebFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jurassic.So.SpiderTool.Controllers
{
    public class TaskLogController : BaseController
    {
        public ITaskLogInfoService taskLogInfoService { get; set; }
        public TaskLogController(ITaskLogInfoService service)
        {
            taskLogInfoService = service;
        }
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 清空所有日志记录
        /// </summary>
        /// <returns></returns>
        public ActionResult Clear()
        {
            taskLogInfoService.ClearTaskLog();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 加载任务日志信息
        /// </summary>
        /// <param name="adapterName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult GetTaskLogInfo(string adapterName, int pageIndex, int pageSize)
        {
            int totalCount = 0;
            var data = taskLogInfoService.GetAdapterTaskLog(adapterName, ref totalCount, null, pageIndex, pageSize);
            return Json(new {data, totalCount }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 加载某一爬取范围下的数据信息
        /// </summary>
        /// <param name="adapterTaskLogIdentity"></param>
        /// <param name="spiderScope"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public ActionResult GetTaskLogDetail(string adapterTaskLogIdentity, string spiderScope, int state)
        {
            var data = taskLogInfoService.GetTaskLogDetail(adapterTaskLogIdentity, spiderScope, state);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 加载某一爬取范围下的所有爬取日志
        /// </summary>
        /// <param name="adapterId"></param>
        /// <param name="spiderScope"></param>
        /// <returns></returns>
        public ActionResult GetTaskLogByScope(string adapterId, string spiderScope, int state)
        {
            var data = taskLogInfoService.GetTaskLogByScope(adapterId, spiderScope, state);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 跳转至日志详情页面
        /// </summary>
        /// <param name="adapterTaskLogIdentity"></param>
        /// <returns></returns>
        public ActionResult ExeLogDetails(string adapterTaskLogIdentity)
        {
            ViewBag.adapterTaskLogIdentity = adapterTaskLogIdentity;
            return View();
        }

        
    }
}
