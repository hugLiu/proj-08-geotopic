namespace Jurassic.So.Data.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class GT_PlanInfo
    {
        public int Id { get; set; }

        public int? AdapterId { get; set; }

        [StringLength(255)]
        public string PlanName { get; set; }

        public DateTime? ValidDate { get; set; }

        public DateTime? NextRunDate { get; set; }

        public bool? PlanWillInvalid { get; set; }

        [StringLength(255)]
        public string RunRule { get; set; }

        [StringLength(255)]
        public string DayRunTime { get; set; }

        public DateTime? InvalidDate { get; set; }

        public int? InvalidTimes { get; set; }

        public int? InvalidType { get; set; }

        [StringLength(255)]
        public string MonthRunMonths { get; set; }

        [StringLength(255)]
        public string MonthRunDay { get; set; }

        [StringLength(255)]
        public string MonthRunTime { get; set; }

        public int? RunTimes { get; set; }

        public int? SelfInterval { get; set; }

        [StringLength(255)]
        public string SelfRunTime { get; set; }

        [StringLength(255)]
        public string WeekRunDays { get; set; }

        [StringLength(255)]
        public string WeekRunTime { get; set; }

        public bool? Diabled { get; set; }

        public bool? IsInvalid { get; set; }

        public long? OrderIndex { get; set; }

        [StringLength(255)]
        public string Remark { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(20)]
        public string CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [StringLength(20)]
        public string UpdatedBy { get; set; }

        public virtual GT_AdapterInfo GT_AdapterInfo { get; set; }
    }
}
