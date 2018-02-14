namespace Jurassic.So.GeoTopic.Database.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GT_IndexDefinition
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GT_IndexDefinition()
        {
            GT_TopicIndex = new HashSet<GT_TopicIndex>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Code { get; set; }

        [Required]
        [StringLength(128)]
        public string Title { get; set; }

        public DateTime CreatedDate { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public bool IsDelete { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GT_TopicIndex> GT_TopicIndex { get; set; }
    }
}
