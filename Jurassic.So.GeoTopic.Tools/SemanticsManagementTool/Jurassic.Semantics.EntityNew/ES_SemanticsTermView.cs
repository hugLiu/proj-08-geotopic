namespace Jurassic.Semantics.EntityNew
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ES_SemanticsTermView
    {
        [Key]
        public Guid TermClassId { get; set; }

        [StringLength(1000)]
        public string Term { get; set; }

        [StringLength(4000)]
        public string ConceptClass { get; set; }

        public string Keyword { get; set; }
    }
}
