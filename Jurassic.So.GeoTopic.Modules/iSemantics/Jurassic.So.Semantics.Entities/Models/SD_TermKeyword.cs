namespace Jurassic.So.Semantics.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SD_TermKeyword
    {
        [Key]
        [Column(Order = 0)]
        public Guid TermClassID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(100)]
        public string Keyword { get; set; }

        public int? OrderIndex { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(100)]
        public string CreatedBy { get; set; }

        public DateTime? LastUpdatedDate { get; set; }

        [StringLength(100)]
        public string LastUpdatedBy { get; set; }

        [StringLength(200)]
        public string Remark { get; set; }

    }
}
