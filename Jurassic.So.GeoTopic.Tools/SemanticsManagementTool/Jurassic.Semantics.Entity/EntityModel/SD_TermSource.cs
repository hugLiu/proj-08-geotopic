namespace Jurassic.Semantics.Entity.EntityModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SD_TermSource
    {
        [Key]
        [Column(Order = 0)]
        public Guid ConceptClassId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(255)]
        public string Source { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
