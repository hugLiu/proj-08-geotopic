using System;
using System.Collections.Generic;

namespace Jurassic.So.SpiderTool.IService.ViewModel
{
    public class AdapterInfoModel
    {
        public AdapterInfoModel()
        {
            this.SpiderCommand = 1;
        }
        public string PlanInfoId { get; set; }
		public string AdapterId { get; set; }
        public int? SpiderSize { get; set; }
        public int? SpiderCommand { get; set; }
        public string AdapterName { get; set; }
        public string DataSourceName { get; set; }
        public string DataSourceType { get; set; }
        public string AdapterURL { get; set; }
        public string TaskCount { get; set; }
		public int TaskRunningCount { get; set; }
        public int? ExecuteTimes { get; set; }
        public bool? IsInvalid { get; set; }
        public int? ProcessedPercent { get; set; }
        public DateTime CreateDate { get; set; }
        public int? Status { get; set; }
        public int InvokeType { get; set; }
        public int? Hangup { get; set; }
    }


    public class AdapterInfoModelPager
    {
        public int total { get; set; }
        public List<AdapterInfoModel> data { get; set; }
    }
}
