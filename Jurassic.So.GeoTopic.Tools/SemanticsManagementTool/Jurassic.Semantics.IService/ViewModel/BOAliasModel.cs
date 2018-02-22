using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.Semantics.IService.ViewModel
{
    public class BoAliasModel
    {
        public Guid ID { get; set; }

        public string Alias { get; set; }

        public int? OrderIndex { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? LastUpdatedDate { get; set; }

        public string LastUpdatedBy { get; set; }

        public string Remark { get; set; }

        public string SourceID { get; set; }

        public string SourceDB { get; set; }

        public string SourceTable { get; set; }
    }
}
