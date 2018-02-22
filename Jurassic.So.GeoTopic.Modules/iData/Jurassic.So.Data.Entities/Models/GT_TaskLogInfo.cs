namespace Jurassic.So.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GT_TaskLogInfo
    {
        public int Id { get; set; }

        public int? SpiderScopeId { get; set; }

        public Guid TaskLogInfoId { get; set; }

        public int? AdapterId { get; set; }

        [StringLength(255)]
        public string SpiderScope { get; set; }

        [StringLength(50)]
        public string  AdapterName { get; set; }

        [StringLength(100)]
        public string DataSourceName { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool? Success { get; set; }

        public int? RecordCount { get; set; }

        public int? FailureTime { get; set; }

        [Column(TypeName = "text")]
        public string FailturReason { get; set; }

        public bool? IsInvalid { get; set; }

        public long? OrderIndex { get; set; }

        [StringLength(200)]
        public string Remark { get; set; }

        [StringLength(50)]
        public string Performer { get; set; }

        [StringLength(255)]
        public string AdapterTaskLogIdentity { get; set; }

        public bool? Deleted { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(20)]
        public string CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [StringLength(20)]
        public string UpdatedBy { get; set; }

    }
}
