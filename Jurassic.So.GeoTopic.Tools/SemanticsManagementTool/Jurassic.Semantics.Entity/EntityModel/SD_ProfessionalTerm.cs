namespace Jurassic.Semantics.Entity.EntityModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SD_ProfessionalTerm
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(255)]
        public string FTerm { get; set; }

        [StringLength(255)]
        public string LTerm { get; set; }

        [StringLength(50)]
        public string RelTypeCode { get; set; }

        public int? OrderIndex { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(100)]
        public string CreatedBy { get; set; }

        public DateTime? LastUpdatedDate { get; set; }

        [StringLength(100)]
        public string LastUpdatedBy { get; set; }

        [StringLength(200)]
        public string Remark { get; set; }

        public virtual CD_RelType CD_RelType { get; set; }
    }
}
