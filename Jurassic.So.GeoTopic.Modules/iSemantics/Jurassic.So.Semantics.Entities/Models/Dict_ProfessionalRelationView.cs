namespace Jurassic.So.Semantics.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Dict_ProfessionalRelationView
    {
        [Key]
        public string term { get; set; }
    }
}
