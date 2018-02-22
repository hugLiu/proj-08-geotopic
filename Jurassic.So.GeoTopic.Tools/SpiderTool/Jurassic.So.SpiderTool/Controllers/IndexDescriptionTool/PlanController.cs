using Jurassic.So.SpiderTool.IService;
using Jurassic.So.SpiderTool.IService.ViewModel;
using Jurassic.So.SpiderTool.Service;
using Jurassic.WebFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jurassic.So.Infrastructure;

namespace Jurassic.So.SpiderTool.Controllers
{
    public class PlanController : BaseController
    {
        public IPlanService _planService { get; set; }

        public PlanController(IPlanService planService)
        {
            this._planService = planService;
        }

        public ActionResult GetPlanInfo(string adapterId)
        {
            string planInfo = _planService.GetPlanIdByAdapter(adapterId);
            return Json(planInfo, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Details(string planInfoId)
        {
            SchedulePlan p = this._planService.GetPlanDetail(planInfoId);
            if (p == null)
            {
                return Json(new SchedulePlan {LblPlanMsg = "还未设置计划"}, JsonRequestBehavior.AllowGet);
            }
            return Json(p, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetLblPlanMsg(string planInfoId, string adapterId)
        {
            SchedulePlan p = this._planService.GetPlanDetail(planInfoId);
            string lblPlanMsg = "";
            lblPlanMsg = p == null ? "还未设置计划" : p.LblPlanMsg;
            return Json(new { adapterId, lblPlanMsg}, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 创建或者更新计划
        /// </summary>
        /// <param name="planStr"></param>
        /// <returns></returns>
        public ActionResult Change(string planStr)
        {
            SchedulePlan plan = planStr.JsonTo<SchedulePlan>();
            //添加调度计划
            if (plan.Id==null)// TODO :!!!!!!!!要改动
            {
                plan.CreatedDate = DateTime.Now;
                plan.Creator = CurrentUser.Name;
                this._planService.AddPlan(plan);
            }//更新调度计划
            else
            {
                plan = this._planService.UpdatePlan(plan);
            }
            ScheduleContext.GetContext().SpiderSchedule.UpdatePlanSchedule(plan);
            return Json(new {plan.Id }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 禁用或启用计划
        /// </summary>
        /// <param name="planId"></param>
        /// <param name="disabled"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangePlanState(string planId, bool disabled)
        {
            if (string.IsNullOrEmpty(planId))
                return null;
            //TODO 检查
            int id = Convert.ToInt16(planId);
            SchedulePlan plan = this._planService.ChagePlanState(id, disabled);
            if (disabled)
            {
                ScheduleContext.GetContext()
                    .SpiderSchedule
                    .DeletePlanFromSchedule(plan);
            }
            else {
                ScheduleContext.GetContext()
                    .SpiderSchedule
                    .UpdatePlanSchedule(plan);
            }
            return JsonTips("success", "计划配置", "计划设置成功");
        }
    }
}
