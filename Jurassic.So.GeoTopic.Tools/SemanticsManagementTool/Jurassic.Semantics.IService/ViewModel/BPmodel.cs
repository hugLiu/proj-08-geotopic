using System;

namespace Jurassic.Semantics.IService.ViewModel
{
    public class BPmodel
    {
        public Guid TermClassId { get; set; }

        public Guid? PId { get; set; }

        public string Term { get; set; }

        public int? OrderIndex { get; set; }

        public bool isLeaf { get; set; }
        public bool expanded { get; set; }
        public bool asyncLoad { get; set; }

        public bool isChecked { get; set; }

        //public int _id { get; set; }





    }
}
