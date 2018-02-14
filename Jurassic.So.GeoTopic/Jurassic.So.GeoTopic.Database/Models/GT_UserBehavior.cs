namespace Jurassic.So.GeoTopic.Database
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GT_UserBehavior
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string BehaviorType { get; set; }

        [StringLength(50)]
        public string Type { get; set; }

        [Required]
        [StringLength(128)]
        public string Title { get; set; }

        [Required]
        [StringLength(1024)]
        public string Content { get; set; }

        public DateTime CreatedDate { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public bool IsDelete { get; set; }

        public virtual UserProfile UserProfile { get; set; }
    }
}
