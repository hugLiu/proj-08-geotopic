using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.GeoTopic.DataService.Models
{
    public class CodeModel
    {
        public int Id { get; set; }

        public int CodeTypeId { get; set; }

        //public string CodeType { get; set; }

        public string Code { get; set; }

        public string Title { get; set; }

        public string _state { get; set; }
    }
}
