using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jurassic.WebFrame;
using Jurassic.So.SpiderTool.IService;
using Jurassic.So.Infrastructure;
using ProcessStatus = Jurassic.So.SpiderTool.IService.Processers.ProcessStatus;

namespace Jurassic.So.SpiderTool.Controllers
{
    public class SpiderTaskController : BaseController
    {
        public IAdapterInfoService adapterInfoService { get; set; }
        public ITaskService taskService { get; set; }

        public SpiderTaskController(IAdapterInfoService service, ITaskService tasksService)
        {
            adapterInfoService = service;
            taskService = tasksService;
        }

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 加载适配器
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAdapterList(int pageIndex, int pageSize)
        {
            int total = 0;
            var list = adapterInfoService.GetAdapterPageInfos(pageIndex, pageSize, ref total);
            return Json(new { data = list, total }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///加载某一适配器下的数据类型
        /// </summary>
        /// <returns></returns>
        public ActionResult Details()
        {
            string adapterId = Request["adapterId"];
            string spiderScope = Request["spiderScope"];

            var list = adapterInfoService.GetSpiderScopeList(adapterId, spiderScope);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public void UpdateSpiderSize(string adapterId, string spiderSize)
        {
            if (string.IsNullOrEmpty(adapterId)|| string.IsNullOrEmpty(spiderSize)) return;
            try
            {
                adapterInfoService.UpdateSpiderSize(adapterId, Convert.ToInt32(spiderSize));
            }
            catch (Exception ex)
            {               
                throw ex;
            }            
        }

        /// <summary>
        /// 开始爬取任务
        /// </summary>
        /// <param name="adapterId">适配器Id</param>
        /// <param name="scopes"></param>
        public int SpiderSelected(string adapterId, string scopes)
        {
            if (CurrentUser?.Name == null)
            {
                return -1;
            }
            var scopeList = scopes.JsonTo<List<string>>();
            adapterInfoService.UpdateSpiderScopeSelected(adapterId, scopeList);
            taskService.ProgressTaskRun(CurrentUser.Name, adapterId);
            return 1;
        }

        /// <summary>
        /// 爬取适配器下所有爬取范围
        /// </summary>
        /// <param name="adapterId">适配器Id</param>
        public int SpiderAll(string adapterId)
        {
            if (CurrentUser?.Name == null)
            {
                return -1;
            }
            adapterInfoService.UpdateSpiderScopeSelectAll(adapterId);
            taskService.ProgressTaskRunAll(CurrentUser.Name, adapterId);
            return 1;
        }

        /// <summary>
        /// 以爬取范围为单位开始爬取任务
        /// </summary>
        /// <param name="adapterId">适配器Id</param>
        /// <param name="scope">待爬取的范围</param>
        public int SpiderByScope(string adapterId, string scope)
        {
            if (CurrentUser?.Name == null)
            {
                return -1;
            }
            taskService.ProgressTaskRunByScope(CurrentUser.Name, adapterId, scope);
            return 1;
        }

        /// <summary>
        /// 爬取失败项
        /// </summary>
        /// <param name="adapterId"></param>
        public int SpiderFailed(string adapterId)
        {
            if (CurrentUser?.Name == null)
            {
                return -1;
            }
            taskService.ProgressTaskRunByStatus(CurrentUser.Name, adapterId, ProcessStatus.Failed);
            return 1;
        }

        /// <summary>
        /// 跳转至设置任务计划页面
        /// </summary>
        /// <param name="adapterId">适配器Id</param>
        /// <returns></returns>
        public ActionResult SetConfigPlan(string adapterId, string adapterName)
        {
            ViewBag.AdapterId = adapterId;
            ViewBag.AdapterName = adapterName;
            return View();
        }

        public ActionResult ExeLogDetails(string adapterId, string spiderScope)
        {
            ViewBag.adapterId = adapterId;
            ViewBag.spiderScope = spiderScope;
            return View();
        }


    }
}
