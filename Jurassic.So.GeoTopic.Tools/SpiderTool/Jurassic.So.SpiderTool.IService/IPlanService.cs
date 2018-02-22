using Jurassic.So.SpiderTool.IService.ViewModel;
using System;
using System.Collections.Generic;

namespace Jurassic.So.SpiderTool.IService
{
    public interface IPlanService
    {
        /// <summary>
        /// 根据适配器Id获取相应计划Id
        /// </summary>
        /// <param name="adapterId">适配器Id</param>
        /// <returns>计划Id</returns>
        string GetPlanIdByAdapter(string adapterId);

        /// <summary>
        /// 根据禁用状态获取所有的调度计划
        /// </summary>
        /// <param name="disabled">禁用？</param>
        /// <returns></returns>
        List<SchedulePlan> GetPlans(bool disabled);

        /// <summary>
        /// 添加调度计划
        /// </summary>
        /// <param name="plan"></param>
        void AddPlan(SchedulePlan plan);

        /// <summary>
        /// 更新调度计划
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        SchedulePlan UpdatePlan(SchedulePlan plan);

        /// <summary>
        /// 获取计划详细信息
        /// </summary>
        /// <param name="planInfoId"></param>
        /// <returns></returns>
        SchedulePlan GetPlanDetail(string planInfoId);

        /// <summary>
        /// 更新计划的禁用状态
        /// </summary>
        /// <param name="planId"></param>
        /// <param name="disabled"></param>
        /// <returns></returns>
        SchedulePlan ChagePlanState(int planId, bool disabled);
    }
}
