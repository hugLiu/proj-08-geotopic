using System;

namespace Jurassic.So.SpiderTool.IService.ViewModel
{
    public class SpiderScopeModel
    {

        public int AdapterId { get; set; }
        public string AdapterName { get; set; }
        public string SpiderScope { get; set; }
        public int Priority { get; set; }
        public string IncrementType { get; set; }
        public int? AnalyseTitle { get; set; }

        public int? AnalyseDataType { get; set; }
        public int? AnalyseDescription { get; set; }
        public int? AnalyseFT { get; set; }
        public int? AutoTag { get; set; }

        public string Description { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool? IsInvalid { get; set; }

        public int? OrderIndex { get; set; }
        public string Remark { get; set; }
        public int? TaskMode { get; set; }
        public int? AsTask { get; set; }
        public int? LastSpiderStatus { get; set; }
        public int? SpiderCommand { get; set; }
        public int? ProcessedPercent { get; set; }

        public int? ExecuteTimes { get; set; }
    }

}
