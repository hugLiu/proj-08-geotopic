namespace Jurassic.So.GeoTopic.Database.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GT_Topic
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GT_Topic()
        {
            GT_TopicCard = new HashSet<GT_TopicCard>();
            GT_TopicIndex = new HashSet<GT_TopicIndex>();
            GT_Topic1 = new HashSet<GT_Topic>();
            webpages_Roles = new HashSet<webpages_Roles>();
        }

        public int Id { get; set; }

        public int? PId { get; set; }

        [StringLength(50)]
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
        public virtual ICollection<GT_TopicCard> GT_TopicCard { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GT_TopicIndex> GT_TopicIndex { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GT_Topic> GT_Topic1 { get; set; }

        public virtual GT_Topic GT_Topic2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<webpages_Roles> webpages_Roles { get; set; }
    }
}
