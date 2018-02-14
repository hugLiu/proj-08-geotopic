using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.GeoTopic.DataService.Models
{
    /// <summary>
    /// 知识主题
    /// </summary>
    public class KTopicModel
    {
        public int Id { get; set; }

        public int? PId { get; set; }

        public string Code { get; set; }

        public string Title { get; set; }

        public string fullPath { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public bool IsDelete { get; set; }

        public string _state { get; set; }

    }


}
