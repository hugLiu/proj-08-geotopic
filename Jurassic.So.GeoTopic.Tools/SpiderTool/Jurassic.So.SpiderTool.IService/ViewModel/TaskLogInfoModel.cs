using System;

namespace Jurassic.So.SpiderTool.IService.ViewModel
{
    public class TaskLogInfoModel
    {
        public Guid TaskLogInfoId { get; set; }
        public string AdapterId { get; set; }
        public string SpiderScope { get; set; }
        public string Performer { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? Success { get; set; }
        public int? RecordCount { get; set; }
        public int? FailureTime { get; set; }
        public string FailturReason { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsInvalid { get; set; }
        public long? OrderIndex { get; set; }
        public string Remark { get; set; }
        public int? TaskMode { get; set; }

    }
}
