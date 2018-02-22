using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.Semantics.IService.ViewModel
{
    public class ReportBPmodel
    {
        public Guid TermClassId { get; set; }
        public Guid? PId { get; set; }

        public string Term { get; set; }

        public string BOT { get; set; }

        public string Synonym { get; set; }

       //public string Keywords { get; set; }

        public string Source { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Remark { get; set; }
        //public bool isLeaf { get; set; }
        //public bool expanded { get; set; }
        //public bool asyncLoad { get; set; }
    }   
}
