using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Jurassic.So.Infrastructure;

namespace Jurassic.So.SpiderTool.IService.ViewModel
{
    public class SchedulePlan
    {
        public SchedulePlan()
        {
            this.ValidDate = DateTime.Now;
        }

        public int? Id { get; set; }

        public int AdapterId { get; set; }

        public string PlanName { get; set; }

        public DateTime? ValidDate { get; set; }

        public bool PlanWillInvalid { get; set; }

        public string RunRule { get; set; }

        public int InvalidType { get; set; }

        public int InvalidTimes { get; set; }

        public int RunTimes { get; set; }

        public DateTime? InvalidDate { get; set; } = new DateTime?();

        public bool DayRun { get; set; }

        public string DayRunTime { get; set; }

        public bool WeekRun { get; set; }

        public string WeekRunDays { get; set; }

        public string WeekRunTime { get; set; }

        public bool MonthRun { get; set; }

        public string MonthRunMonths { get; set; }

        public string MonthRunDay { get; set; }

        public string MonthRunTime { get; set; }

        public bool SelfRun { get; set; }

        public string SelfRunTime { get; set; }

        public int SelfInterval { get; set; } = 2;

        public DateTime? NextRunDate { get; set; }

        public bool Diabled { get; set; }

        public string Creator { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string Remark { get; set; }

        public string LblPlanMsg
        {
            get { return ToString(); }
            set { }
        }
        public override string ToString()
        {
            string toStr = $"{GetPlanStateMsg()}{GetPlanExecuteMsg()}|{GetInvalidMsg()}";
            return toStr;
        }
        private string GetPlanStateMsg()
        {
            string planStateMsg;
            if (Diabled)
            {
                planStateMsg = "[已禁用]";
            }
            else
            {
                if (ValidDate <= DateTime.Now)
                {
                    planStateMsg = "[已生效]";
                }
                else
                {
                    planStateMsg = "[未生效]";
                }
            }
            return planStateMsg;
        }
        private string GetPlanExecuteMsg()
        {
            string planExecuteMsg = string.Empty;
            string nextRunDate = NextRunDate?.ToString("yyyy/MM/dd HH:mm:ss") ?? string.Empty;
            switch (RunRule)
            {
                case "DayRun":
                    planExecuteMsg = $"每天 {DayRunTime} 运行|下次运行时间为：{nextRunDate}";
                    break;
                case "MonthRun":
                    string monthListPath = @"~/Data/MonthList.txt";
                    string dayListPath = @"~/Data/MonthDayList.txt";
                    string[] monthIds = MonthRunMonths.Split(',').ToArray();
                    string[] monthDayIds = MonthRunDay.Split(',').ToArray();
                    List<Dictionary<string, string>> monthList = GetCalenderHeader(monthListPath);
                    List<Dictionary<string, string>> monthDayList = GetCalenderHeader(dayListPath);
                    string monthHeadStr = string.Join(",", monthList.Where(t => monthIds.Contains(t["id"])).Select(t => t["text"]));
                    string monthDayHeadStr = string.Join(",", monthDayList.Where(t => monthDayIds.Contains(t["id"])).Select(t => t["text"]));
                    planExecuteMsg = $"每个[{monthHeadStr}][{monthDayHeadStr}] {MonthRunTime} 运行|下次运行时间为：{nextRunDate}";
                    break;
                case "WeekRun":
                    string weekListPath = @"~/Data/DayList.txt";
                    string[] weekRunDayIds = WeekRunDays.Split(',').ToArray();
                    List<Dictionary<string, string>> weekDayList = GetCalenderHeader(weekListPath);
                    string weekDayHeadStr = string.Join(",", weekDayList.Where(t => weekRunDayIds.Contains(t["id"])).Select(t => t["text"]));
                    planExecuteMsg = $"每个[{weekDayHeadStr}] {WeekRunTime} 运行|下次运行时间为：{nextRunDate}";
                    break;
                case "SelfRun":
                    planExecuteMsg = $"{SelfRunTime}开始执行，间隔{SelfInterval}分钟重复执行|下次运行时间为：{nextRunDate}";
                    break;
                default:
                    break;
            }
            return planExecuteMsg;
        }
        private string GetInvalidMsg()
        {
            string invalidMsg = string.Empty;
            if (!PlanWillInvalid)
            {
                invalidMsg = "计划一直生效";
                return invalidMsg;
            }
            switch (InvalidType)
            {
                //按次数过期
                case 1:
                    invalidMsg = $"执行{InvalidTimes}次后过期";
                    break;
                //按日期过期
                case 2:
                    invalidMsg = $"{InvalidDate}过期";
                    break;
                default:
                    break;
            }
            return invalidMsg;
        }
        private List<Dictionary<string, string>> GetCalenderHeader(string filePath)
        {
            string dir = HttpContext.Current.Server.MapPath(filePath);
            StreamReader sr = new StreamReader(dir, Encoding.Default);
            string jsonStr = sr.ReadToEnd();
            List<Dictionary<string, string>> data = jsonStr.JsonTo<List<Dictionary<string, string>>>();
            return data;
        }

    }
}
