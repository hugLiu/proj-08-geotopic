using System;

namespace Jurassic.So.GeoTopic.DataService.Models
{
    public class Dic_tUrlModel
    {
        public int Id { get; set; }
        public int RenderTypeId { get; set; }
        public string RenderTypeTitle { get; set; }
        public string Url { get; set; }
        public string _state { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public bool IsDelete { get; set; }
    }
}
