namespace Jurassic.Semantics.Entity.EntityModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BO_BaseInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BO_BaseInfo()
        {
            BO_BOAlias = new HashSet<BO_BOAlias>();
            BO_Relation = new HashSet<BO_Relation>();
            BO_Relation1 = new HashSet<BO_Relation>();
        }

        public Guid ID { get; set; }

        public Guid? PID { get; set; }

        [Required]
        [StringLength(255)]
        public string BOT { get; set; }

        [StringLength(100)]
        public string SID { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        public Guid? TypeID { get; set; }

        public string TypeName { get; set; }

        public DbGeography SpaceLocationPoint { get; set; }

        public DbGeometry SpaceLocationArea { get; set; }

        public int? OrderIndex { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(100)]
        public string CreatedBy { get; set; }

        public DateTime? LastUpdatedDate { get; set; }

        [StringLength(100)]
        public string LastUpdatedBy { get; set; }

        [StringLength(200)]
        public string Remark { get; set; }

        [StringLength(200)]
        public string SourceID { get; set; }

        [StringLength(200)]
        public string SourceDB { get; set; }

        [StringLength(200)]
        public string SourceTable { get; set; }

        public virtual CD_TypeCode CD_TypeCode { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BO_BOAlias> BO_BOAlias { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BO_Relation> BO_Relation { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BO_Relation> BO_Relation1 { get; set; }
    }
}
