﻿using Jurassic.So.SpiderTool.IService.ViewModel;
using Quartz;
using System;
using Jurassic.So.SpiderTool.IService.Processers;
using static System.Int32;

namespace Jurassic.So.SpiderTool.Service.Triggers
{
    [Processor(Type = "DRunTriggerCreator")]
    public class DRunTriggerCreator : Processor<SchedulePlan, ITriggerCreator>
    {

        protected override bool CanProcess(SchedulePlan s)
        {
            return s.RunRule == RunRule.DayRun;
        }

        protected override ITriggerCreator DoProcess(SchedulePlan s)
        {
            return new DRunTrigger(s);
        }
    }
    public class DRunTrigger : ITriggerCreator
    {
        private readonly SchedulePlan _schedulePlan;
        public DRunTrigger(SchedulePlan s)
        {
            _schedulePlan = s;
        }
        public ITrigger CreateCronTrigger()
        {
            string timeStr = _schedulePlan.DayRunTime;
            if (timeStr == null || "".Equals(timeStr.Trim()))
            {
                return default(ITrigger);
            }
            string[] timeArr = timeStr.Split(':');
            if (timeArr.Length != 3) throw new ArgumentException("按天执行触发器中时间格式不正确", "DRunTrigger-Time");

            if (timeArr[0].Length == 2
                && timeArr[0].IndexOf('0') == 0)
            {
                timeArr[0] = timeArr[0].Substring(1);
            }
            if (timeArr[1].Length == 2
                && timeArr[1].IndexOf('0') == 0)
            {
                timeArr[1] = timeArr[1].Substring(1);
            }
            if (timeArr[2].Length == 2
                && timeArr[2].IndexOf('0') == 0)
            {
                timeArr[2] = timeArr[2].Substring(1);
            }
            int hour = Parse(timeArr[0]);
            int minute = Parse(timeArr[1]);
            int second = Parse(timeArr[2]);
            string conStr = $"{second} {minute} {hour} * * ?";
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity($"trigger-{_schedulePlan.Id}")
                .WithSchedule(CronScheduleBuilder.CronSchedule(conStr))
                .Build();
            return trigger;
        }
    }
}