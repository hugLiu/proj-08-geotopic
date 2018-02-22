using System;

namespace Jurassic.Semantics.IService.ViewModel
{
    public class TermTreeModel
    {
        public Guid TermClassId { get; set; }
        public string Term { get; set; }

        public Guid? PId { get; set; }
        public int lvl { get; set; }

        public string Source { get; set; }

        public int? OrderIndex { get; set; }

        public string Description { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }

        public int IsLeaf { get; set; }

        public bool isChecked { get; set; }
        public string PathTerm { get; set; }
    }
}
