namespace Jurassic.So.Submission.Mock.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GB_Submission
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GB_Submission()
        {
            GB_File = new HashSet<GB_File>();
        }

        public int Id { get; set; }

        public Guid NatureKey { get; set; }

        public bool? Authentic { get; set; }

        public bool? AutoComplement { get; set; }

        [StringLength(100)]
        public string UploadedBy { get; set; }

        public DateTime? UploadedDate { get; set; }

        [StringLength(500)]
        public string Contact { get; set; }

        [StringLength(500)]
        public string Application { get; set; }

        [StringLength(500)]
        public string Task { get; set; }

        public DateTime CreatedDate { get; set; }

        [StringLength(100)]
        public string CreatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }

        [StringLength(100)]
        public string UpdatedBy { get; set; }

        public bool IsDelete { get; set; }

        [StringLength(500)]
        public string Tag1 { get; set; }

        [StringLength(500)]
        public string Tag2 { get; set; }

        [StringLength(500)]
        public string Tag3 { get; set; }

        [StringLength(500)]
        public string Tag4 { get; set; }

        [StringLength(500)]
        public string Tag5 { get; set; }

        [StringLength(500)]
        public string Tag6 { get; set; }

        [StringLength(500)]
        public string Tag7 { get; set; }

        [StringLength(500)]
        public string Tag8 { get; set; }

        [StringLength(500)]
        public string Tag9 { get; set; }

        [StringLength(500)]
        public string Tag10 { get; set; }

        [StringLength(500)]
        public string Tag11 { get; set; }

        [StringLength(500)]
        public string Tag12 { get; set; }

        [StringLength(500)]
        public string Tag13 { get; set; }

        [StringLength(500)]
        public string Tag14 { get; set; }

        [StringLength(500)]
        public string Tag15 { get; set; }

        [StringLength(500)]
        public string Tag16 { get; set; }

        [StringLength(500)]
        public string Tag17 { get; set; }

        [StringLength(500)]
        public string Tag18 { get; set; }

        [StringLength(500)]
        public string Tag19 { get; set; }

        [StringLength(500)]
        public string Tag20 { get; set; }

        public DateTime? Tag21 { get; set; }

        public DateTime? Tag22 { get; set; }

        public DateTime? Tag23 { get; set; }

        public DateTime? Tag24 { get; set; }

        public DateTime? Tag25 { get; set; }

        public DateTime? Tag26 { get; set; }

        public DateTime? Tag27 { get; set; }

        public DateTime? Tag28 { get; set; }

        public DateTime? Tag29 { get; set; }

        public DateTime? Tag30 { get; set; }

        [StringLength(500)]
        public string Tag31 { get; set; }

        [StringLength(500)]
        public string Tag32 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GB_File> GB_File { get; set; }
    }
}
