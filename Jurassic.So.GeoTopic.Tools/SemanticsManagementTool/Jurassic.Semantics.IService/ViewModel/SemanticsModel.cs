using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.Semantics.IService.ViewModel
{
    public class SemanticsModel
    {
        public Guid FTermClassId { get; set; }


        public string SR { get; set; }


        public Guid LTermClassId { get; set; }

        public string FTerm { get; set; }


        public string LTerm { get; set; }

        public int? OrderIndex { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? LastUpdatedDate { get; set; }

        public string LastUpdatedBy { get; set; }

        public string Remark { get; set; }

        public bool IsLeaf { get; set; }
    }
}
