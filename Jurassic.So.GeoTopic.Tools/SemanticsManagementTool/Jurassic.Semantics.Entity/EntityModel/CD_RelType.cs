namespace Jurassic.Semantics.Entity.EntityModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CD_RelType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CD_RelType()
        {
            BO_Relation = new HashSet<BO_Relation>();
            SD_ProfessionalTerm = new HashSet<SD_ProfessionalTerm>();
        }

        [Key]
        [StringLength(50)]
        public string RelTypeCode { get; set; }

        [StringLength(100)]
        public string RelTypeName { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(200)]
        public string Remark { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BO_Relation> BO_Relation { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SD_ProfessionalTerm> SD_ProfessionalTerm { get; set; }
    }
}
