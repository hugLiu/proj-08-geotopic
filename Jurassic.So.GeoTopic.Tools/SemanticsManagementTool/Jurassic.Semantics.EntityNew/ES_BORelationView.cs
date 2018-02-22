namespace Jurassic.Semantics.EntityNew
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ES_BORelationView
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(255)]
        public string BOT1 { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid ID1 { get; set; }

        [StringLength(200)]
        public string Name1 { get; set; }

        [StringLength(255)]
        public string TypeName1 { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string RelTypeCode { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(255)]
        public string BOT2 { get; set; }

        [Key]
        [Column(Order = 4)]
        public Guid ID2 { get; set; }

        [StringLength(200)]
        public string Name2 { get; set; }

        [StringLength(255)]
        public string TypeName2 { get; set; }
    }
}
