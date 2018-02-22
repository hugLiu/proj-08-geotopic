namespace Jurassic.Semantics.Entity.EntityModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tmp_Location
    {
        [StringLength(255)]
        public string Name { get; set; }

        public double? X { get; set; }

        public double? Y { get; set; }

        public Guid ID { get; set; }
    }
}
