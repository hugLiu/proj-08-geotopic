namespace Jurassic.So.GeoTopic.Database.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GT_RenderUrl
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GT_RenderUrl()
        {
            GT_UrlTemplate = new HashSet<GT_UrlTemplate>();
        }

        public int Id { get; set; }

        public int RenderTypeId { get; set; }

        [Required]
        [StringLength(128)]
        public string Url { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public bool IsDelete { get; set; }

        public virtual GT_RenderType GT_RenderType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GT_UrlTemplate> GT_UrlTemplate { get; set; }
    }
}
