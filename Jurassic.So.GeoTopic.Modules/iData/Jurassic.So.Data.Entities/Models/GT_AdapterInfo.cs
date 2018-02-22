namespace Jurassic.So.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class GT_AdapterInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GT_AdapterInfo()
        {
            GT_PlanInfo = new HashSet<GT_PlanInfo>();
            GT_SpiderScope = new HashSet<GT_SpiderScope>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string AdapterName { get; set; }

        [StringLength(8)]
        public string SDomain { get; set; }

        [StringLength(100)]
        public string DataSourceName { get; set; }

        [StringLength(50)]
        public string DataSourceType { get; set; }

        [StringLength(1024)]
        public string AdapterURL { get; set; }

        public int? Status { get; set; }

        public int InvokeType { get; set; }

        public int? Hangup { get; set; }

        [StringLength(1024)]
        public string Desc { get; set; }

        public bool? IsInvalid { get; set; }

        public long? OrderIndex { get; set; }

        [StringLength(200)]
        public string Remark { get; set; }

        [StringLength(100)]
        public string ClassName { get; set; }

        [StringLength(1000)]
        public string ConfigAddress { get; set; }

        [StringLength(100)]
        public string AdapterType { get; set; }

        public int? SpiderSize { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(20)]
        public string CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [StringLength(20)]
        public string UpdatedBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GT_PlanInfo> GT_PlanInfo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GT_SpiderScope> GT_SpiderScope { get; set; }
    }
}
