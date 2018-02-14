using System;

namespace Jurassic.So.GeoTopic.DataService.Models
{
    public class Dic_tTypeModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string _state { get; set; }
        public string Code { get; set; }
        public DateTime? CreateDate { get; set; }

        public string Creator { get; set; }

        public bool IsDelete { get; set; }

    }
}
