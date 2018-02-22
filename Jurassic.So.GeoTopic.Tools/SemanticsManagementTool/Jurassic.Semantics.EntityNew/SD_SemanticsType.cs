namespace Jurassic.Semantics.EntityNew
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SD_SemanticsType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SD_SemanticsType()
        {
            SD_Semantics = new HashSet<SD_Semantics>();
        }

        [Key]
        [StringLength(100)]
        public string SR { get; set; }

        [StringLength(255)]
        public string CCCode1 { get; set; }

        [StringLength(255)]
        public string CCCode2 { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(100)]
        public string CreatedBy { get; set; }

        public DateTime? LastUpdatedDate { get; set; }

        [StringLength(100)]
        public string LastUpdatedBy { get; set; }

        [StringLength(200)]
        public string Remark { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SD_Semantics> SD_Semantics { get; set; }
    }
}