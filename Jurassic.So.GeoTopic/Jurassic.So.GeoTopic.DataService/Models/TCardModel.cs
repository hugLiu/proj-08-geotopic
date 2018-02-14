using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.GeoTopic.DataService.Models
{
    public class TCardModel
    {
        public int Id { get; set; }

        public int TopicId { get; set; }

        public string Title { get; set; }

        public string Definition { get; set; }

        public int OrderId { get; set; }
    }
}
