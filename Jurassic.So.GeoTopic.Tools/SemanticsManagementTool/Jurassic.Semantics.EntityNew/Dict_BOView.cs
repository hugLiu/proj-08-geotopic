namespace Jurassic.Semantics.EntityNew
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Dict_BOView
    {
        [Key]
        [StringLength(200)]
        public string dictterm { get; set; }
    }
}
