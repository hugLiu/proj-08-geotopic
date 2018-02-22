namespace Jurassic.So.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GT_SpiderScope
    {
        public int Id { get; set; }

        public int AdapterId { get; set; }

        [Required]
        [StringLength(255)]
        public string SpiderScope { get; set; }

        public int? Priority { get; set; }

        [StringLength(1024)]
        public string Desc { get; set; }

        public bool? IsInvalid { get; set; }

        public long? OrderIndex { get; set; }

        [StringLength(200)]
        public string Remark { get; set; }

        [StringLength(50)]
        public string IncrementType { get; set; }

        public bool? AsTask { get; set; }

        public int? LastSpiderStatus { get; set; }

        public int? TaskMode { get; set; }

        [StringLength(50)]
        public string IncrementValue { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(20)]
        public string CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [StringLength(20)]
        public string UpdatedBy { get; set; }

        public int? ExecuteTimes { get; set; }

        public virtual GT_AdapterInfo GT_AdapterInfo { get; set; }

    }
}
