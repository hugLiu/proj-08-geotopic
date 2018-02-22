namespace Jurassic.So.Data.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class API_DATA_NODE_INFO
    {
        [Key]
        [StringLength(36)]
        public string DataID { get; set; }

        [StringLength(50)]
        public string DataParentID { get; set; }

        [StringLength(50)]
        public string DataNodeName { get; set; }

        [StringLength(50)]
        public string DataNodeID { get; set; }

        [StringLength(1000)]
        public string Memo { get; set; }

        public int? IsValid { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }
    }
}
