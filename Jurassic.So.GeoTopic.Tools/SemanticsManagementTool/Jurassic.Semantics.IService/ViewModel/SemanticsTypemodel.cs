using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.Semantics.IService.ViewModel
{
    public class SemanticsTypemodel
    {
        public string SR { get; set; }

        public string CC1name { get; set; }

        public string CC2name { get; set; }

        public string Description { get; set; }

        public DateTime? CreatedDate { get; set; }


        public string CreatedBy { get; set; }

        public DateTime? LastUpdatedDate { get; set; }


        public string LastUpdatedBy { get; set; }


        public string Remark { get; set; }
    }
}
