namespace Jurassic.Semantics.Entity.EntityModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BO_Relation
    {
        [Key]
        [Column(Order = 0)]
        public Guid ID1 { get; set; }

        [Required]
        [StringLength(255)]
        public string BOT1 { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string RelTypeCode { get; set; }

        [Key]
        [Column(Order = 2)]
        public Guid ID2 { get; set; }

        [Required]
        [StringLength(255)]
        public string BOT2 { get; set; }

        public virtual BO_BaseInfo BO_BaseInfo { get; set; }

        public virtual BO_BaseInfo BO_BaseInfo1 { get; set; }

        public virtual CD_RelType CD_RelType { get; set; }
    }
}
