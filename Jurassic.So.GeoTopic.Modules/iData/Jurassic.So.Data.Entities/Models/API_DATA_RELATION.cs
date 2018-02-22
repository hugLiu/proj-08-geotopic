namespace Jurassic.So.Data.Entities
{
    using System.ComponentModel.DataAnnotations;

    public partial class API_DATA_RELATION
    {
        [Key]
        [StringLength(36)]
        public string RID { get; set; }

        [StringLength(50)]
        public string DataID { get; set; }

        [StringLength(50)]
        public string TokeyID { get; set; }
    }
}
