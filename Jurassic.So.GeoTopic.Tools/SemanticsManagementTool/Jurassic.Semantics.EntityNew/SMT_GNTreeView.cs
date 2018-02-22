namespace Jurassic.Semantics.Entity.EntityModel
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class SMT_GNTreeView
    {
        [StringLength(500)]
        public string Description { get; set; }

        [Key]
        public Guid TermClassId { get; set; }

        [StringLength(255)]
        public string Term { get; set; }

        [StringLength(100)]
        public string SR { get; set; }

        public int? OrderIndex { get; set; }

        [StringLength(100)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(255)]
        public string Source { get; set; }

        public Guid? PId { get; set; }
    }
}
