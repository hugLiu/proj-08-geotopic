namespace Jurassic.Semantics.Entity.EntityModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ES_EPsOfPTView
    {
        [Key]
        [Column(Order = 0)]
        public Guid TermClassId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(1000)]
        public string PT { get; set; }

        public string CPT { get; set; }

        public string UPT { get; set; }

        public string BP { get; set; }

        public string UBP { get; set; }

        public string BF { get; set; }

        public string BOT { get; set; }
    }
}
