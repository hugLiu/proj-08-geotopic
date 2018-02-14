namespace Jurassic.So.GeoTopic.Database.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GT_TopicIndex
    {
        public int Id { get; set; }

        public int TopicId { get; set; }

        public int IndexDefinitionId { get; set; }

        [Required]
        [StringLength(128)]
        public string Value { get; set; }

        public DateTime CreatedDate { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public bool IsDelete { get; set; }

        public virtual GT_IndexDefinition GT_IndexDefinition { get; set; }

        public virtual GT_Topic GT_Topic { get; set; }
    }
}
