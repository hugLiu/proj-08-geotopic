namespace Jurassic.Semantics.Entity.EntityModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Dict_ProfessionalView
    {
        [Key]
        [StringLength(255)]
        public string term { get; set; }
    }
}
