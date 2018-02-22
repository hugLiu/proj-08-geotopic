using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.Semantics.IService.ViewModel
{
    public class CcTermModel
    {
        public Guid TermClassID { get; set; }

        public string CCCode { get; set; }


        public string Term { get; set; }

        public string PathTerm { get; set; }


        public string Source { get; set; }

        public string Description { get; set; }

        public int? OrderIndex { get; set; }

        public DateTime? CreatedDate { get; set; }


        public string CreatedBy { get; set; }

        public DateTime? LastUpdatedDate { get; set; }

        public string LastUpdatedBy { get; set; }

        public string Remark { get; set; }            
    }
}
