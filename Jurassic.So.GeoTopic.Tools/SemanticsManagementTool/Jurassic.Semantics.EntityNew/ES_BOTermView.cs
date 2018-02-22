namespace Jurassic.Semantics.EntityNew
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ES_BOTermView
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(255)]
        public string BOT { get; set; }

        [StringLength(255)]
        public string PTypeName { get; set; }

        [StringLength(200)]
        public string PName { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid ID { get; set; }

        public Guid? PID { get; set; }

        [StringLength(100)]
        public string SID { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(200)]
        public string Name { get; set; }

        public int? OrderIndex { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(100)]
        public string CreatedBy { get; set; }

        public DateTime? LastUpdatedDate { get; set; }

        [StringLength(100)]
        public string LastUpdatedBy { get; set; }

        [StringLength(200)]
        public string Remark { get; set; }

        [StringLength(200)]
        public string SourceID { get; set; }

        [StringLength(200)]
        public string SourceDB { get; set; }

        [StringLength(200)]
        public string SourceTable { get; set; }
    }
}
