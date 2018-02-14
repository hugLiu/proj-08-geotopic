namespace Jurassic.So.GeoTopic.Database.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GT_Remark
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GT_Remark()
        {
            UserProfile1 = new HashSet<UserProfile>();
        }

        public int Id { get; set; }

        [StringLength(50)]
        public string Scoap { get; set; }

        [StringLength(128)]
        public string NatureKey { get; set; }

        [Required]
        public string Comment { get; set; }

        public int? UserId { get; set; }

        public DateTime CreatedDate { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public bool IsDelete { get; set; }

        public virtual UserProfile UserProfile { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserProfile> UserProfile1 { get; set; }
    }
}
