namespace Jurassic.So.Semantics.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SD_CCTerm
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SD_CCTerm()
        {
            SD_TermTranslation = new HashSet<SD_TermTranslation>();
        }

        [Key]
        public Guid TermClassID { get; set; }

        [StringLength(255)]
        public string CCCode { get; set; }

        [Required]
        [StringLength(255)]
        public string Term { get; set; }

        [StringLength(10)]
        public string LangCode { get; set; }

        [StringLength(1000)]
        public string PathTerm { get; set; }

        [StringLength(255)]
        public string Source { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public int? OrderIndex { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(100)]
        public string CreatedBy { get; set; }

        public DateTime? LastUpdatedDate { get; set; }

        [StringLength(100)]
        public string LastUpdatedBy { get; set; }

        [StringLength(200)]
        public string Remark { get; set; }

        public virtual SD_ConceptClass SD_ConceptClass { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SD_TermTranslation> SD_TermTranslation { get; set; }
    }
}
