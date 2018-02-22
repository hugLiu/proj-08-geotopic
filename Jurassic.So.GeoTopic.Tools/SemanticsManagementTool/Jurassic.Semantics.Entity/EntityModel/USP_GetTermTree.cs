using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Jurassic.Semantics.Entity.EntityModel
{
    public class USP_GetTermTree
    {
        [Key]
        //public string TermClassId { get; set; }

        public Guid TermClassId { get; set; }
        //public string TermClassID { get; set; }
        public string Term { get; set; }

        public Guid? PId { get; set; }
        public int lvl { get; set; }

        public string Source { get; set; }

        public int? OrderIndex { get; set; }

        public string Description { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }

        public  int IsLeaf { get; set; }
        public string PathTerm { get; set; }

    }
}
