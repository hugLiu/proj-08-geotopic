namespace Jurassic.So.GeoTopic.Database.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserProfile")]
    public partial class UserProfile
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserProfile()
        {
            GT_Remark = new HashSet<GT_Remark>();
            GT_Remark1 = new HashSet<GT_Remark>();
            webpages_Roles = new HashSet<webpages_Roles>();
            GT_UserBehavior = new HashSet<GT_UserBehavior>();
        }

        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(56)]
        public string UserName { get; set; }

        [StringLength(200)]
        public string Email { get; set; }

        [StringLength(50)]
        public string PhoneNumber { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GT_Remark> GT_Remark { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GT_Remark> GT_Remark1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<webpages_Roles> webpages_Roles { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GT_UserBehavior> GT_UserBehavior { get; set; }
    }
}
