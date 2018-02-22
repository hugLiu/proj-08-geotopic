namespace Jurassic.Semantics.EntityNew
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ES_EPsOfPTView
    {
        [Key]
        public Guid TermClassId { get; set; }

        [StringLength(1000)]
        public string PT { get; set; }

        public string BD { get; set; }

        public string UBD { get; set; }

        public string BT { get; set; }

        public string UBT { get; set; }

        public string BP { get; set; }

        public string UBP { get; set; }

        public string BA { get; set; }

        public string UBA { get; set; }

        public string DS { get; set; }

        public string UDS { get; set; }

        public string UPT { get; set; }

        public string TL { get; set; }

        public string GN { get; set; }

        public string BF { get; set; }

        public string BS { get; set; }

        public string BOT { get; set; }
    }
}
