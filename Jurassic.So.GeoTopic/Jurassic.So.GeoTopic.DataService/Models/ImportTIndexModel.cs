using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.GeoTopic.DataService.Models
{
    public class ImportTIndexModel
    {
        public int Id { get; set; }
        public int TopicId { get; set; }
        public string IndexDefinitionCode { get; set; }
        public string Value { get; set; }
    }
}
