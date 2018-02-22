namespace Jurassic.Semantics.Entity.EntityModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SD_TermTranslation
    {
        [Key]
        [Column(Order = 0)]
        public Guid TermClassID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string LangCode { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(255)]
        public string Translation { get; set; }

        public int? IsMain { get; set; }

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
    }
}
