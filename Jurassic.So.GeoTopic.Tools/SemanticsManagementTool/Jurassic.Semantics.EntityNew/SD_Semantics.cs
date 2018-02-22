namespace Jurassic.Semantics.EntityNew
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SD_Semantics
    {
        [Key]
        [Column(Order = 0)]
        public Guid FTermClassId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(100)]
        public string SR { get; set; }

        [Key]
        [Column(Order = 2)]
        public Guid LTermClassId { get; set; }

        [Required]
        [StringLength(255)]
        public string FTerm { get; set; }

        [Required]
        [StringLength(255)]
        public string LTerm { get; set; }

        public int? OrderIndex { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(100)]
        public string CreatedBy { get; set; }

        public DateTime? LastUpdatedDate { get; set; }

        [StringLength(100)]
        public string LastUpdatedBy { get; set; }

        [StringLength(200)]
        public string Remark { get; set; }

        public virtual SD_CCTerm SD_CCTerm { get; set; }

        public virtual SD_CCTerm SD_CCTerm1 { get; set; }

        public virtual SD_SemanticsType SD_SemanticsType { get; set; }
    }
}
