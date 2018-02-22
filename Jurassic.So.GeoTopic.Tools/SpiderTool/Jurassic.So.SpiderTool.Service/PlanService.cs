using Jurassic.So.SpiderTool.IService;
using Jurassic.So.SpiderTool.IService.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using Jurassic.So.Data.Entities;
using Jurassic.So.Infrastructure;

namespace Jurassic.So.SpiderTool.Service
{
    public class PlanService : IPlanService
    {
        private DataServiceDBContext _context { get; set; }
        public PlanService()
        {
            _context = new DataServiceDBContext();
        }

        public string GetPlanIdByAdapter(string adapterId)
        {
            var planInfo = _context.GT_PlanInfo.FirstOrDefault(s => s.AdapterId.ToString() == adapterId);
            return planInfo?.Id.ToString() ?? string.Empty;
        }

        /// <summary>
        /// 获取设置计划列表
        /// </summary>
        /// <param name="disabled"></param>
        /// <returns></returns>
        public List<SchedulePlan> GetPlans(bool disabled)
        {
            //列名 'GT_AdapterInfo_Id' 无效。
            var planInfos = _context.GT_PlanInfo
                .Where(t => t.Diabled == disabled)               
                .ToList();
            return planInfos
                .Select(planInfo => planInfo.MapTo<SchedulePlan>())
                .ToList();
        }

        /// <summary>
        /// 获取设置计划详细信息
        /// </summary>
        /// <param name="planInfoId"></param>
        /// <returns></returns>
        public SchedulePlan GetPlanDetail(string planInfoId)
        {
            var plan = _context.GT_PlanInfo.FirstOrDefault(t => t.Id.ToString() == planInfoId);
            var p = plan.MapTo<SchedulePlan>();
            return p;
        }

        /// <summary>
        /// 添加计划信息
        /// </summary>
        /// <param name="plan"></param>
        public void AddPlan(SchedulePlan plan)
        {
            var info = plan.MapTo<GT_PlanInfo>();
            _context.GT_PlanInfo.Add(info);
            _context.SaveChanges();
        }

        /// <summary>
        /// 更新设置计划信息
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        public SchedulePlan UpdatePlan(SchedulePlan plan)
        {
            var info = plan.MapTo<GT_PlanInfo>();
            var entity = _context.GT_PlanInfo.FirstOrDefault(t => t.Id == info.Id);
            if (entity == null) return plan;
            entity.AdapterId = info.AdapterId;
            entity.PlanName = info.PlanName;
            entity.ValidDate = info.ValidDate;
            entity.NextRunDate = info.NextRunDate;
            entity.PlanWillInvalid = info.PlanWillInvalid;
            entity.RunRule = info.RunRule;
            entity.DayRunTime = info.DayRunTime;
            entity.InvalidDate = info.InvalidDate;
            entity.InvalidTimes = info.InvalidTimes;
            entity.InvalidType = info.InvalidType;
            entity.MonthRunMonths = info.MonthRunMonths;
            entity.MonthRunDay = info.MonthRunDay;
            entity.MonthRunTime = info.MonthRunTime;
            entity.RunTimes = info.RunTimes;
            entity.SelfInterval = info.SelfInterval;
            entity.SelfRunTime = info.SelfRunTime;
            entity.WeekRunDays = info.WeekRunDays;
            entity.WeekRunTime = info.WeekRunTime;
            entity.Diabled = info.Diabled;
            entity.IsInvalid = info.IsInvalid;
            entity.OrderIndex = info.OrderIndex;
            entity.Remark = info.Remark;
            entity.CreatedDate = info.CreatedDate;
            entity.CreatedBy = info.CreatedBy;
            entity.UpdatedDate = DateTime.Now;
            entity.UpdatedBy = info.UpdatedBy;
            _context.SaveChanges();
            return plan;
        }

        /// <summary>
        /// 是否禁用计划设置状态
        /// </summary>
        /// <param name="planId"></param>
        /// <param name="disabled"></param>
        /// <returns></returns>
        public SchedulePlan ChagePlanState(int planId, bool disabled)
        {
            var entity = _context.GT_PlanInfo.FirstOrDefault(t => t.Id == planId);
            if (entity == null) return null;
            entity.Diabled = disabled;
            _context.SaveChanges();
            return entity.MapTo<SchedulePlan>();
        }
    }
}
