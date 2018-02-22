using System;
using Jurassic.So.SpiderTool.IService.ViewModel;

namespace Jurassic.So.SpiderTool.Service
{
    public class PlanUtil
    {
        /// <summary>
        /// 判断一个计划是否已经过期
        /// </summary>
        /// <returns></returns>
        public static bool IsPlanInvalid(SchedulePlan p)
        {
            const int invalidTypeTime = 1, invalidTypeDate = 2;
            if (!p.PlanWillInvalid)
            {
                return false;
            }
            switch (p.InvalidType)
            {
                case invalidTypeTime:
                    return p.InvalidTimes <= p.RunTimes;
                case invalidTypeDate:
                    return p.InvalidDate != null && p.InvalidDate < DateTime.Now;
                default:
                    return false;
            }
        }
        public static bool IsPlanCanBeSchedule(SchedulePlan p)
        {
            if (IsPlanInvalid(p))
            {
                return false;
            }
            return p.ValidDate != null && p.ValidDate <= DateTime.Now;
        }
    }
}
