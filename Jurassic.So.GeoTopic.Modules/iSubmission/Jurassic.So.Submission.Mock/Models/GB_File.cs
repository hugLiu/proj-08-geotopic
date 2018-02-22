namespace Jurassic.So.Submission.Mock.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GB_File
    {
        public int Id { get; set; }

        public int? SubmissiomId { get; set; }

        [Required]
        [StringLength(50)]
        public string FileName { get; set; }

        public byte[] File { get; set; }

        public DateTime CreatedDate { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public bool IsDelete { get; set; }

        public virtual GB_Submission GB_Submission { get; set; }
    }
}
