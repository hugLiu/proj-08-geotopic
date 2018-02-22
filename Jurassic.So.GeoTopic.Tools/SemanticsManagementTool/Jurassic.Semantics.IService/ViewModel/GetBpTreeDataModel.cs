using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.Semantics.IService.ViewModel
{
    public class GetBpTreeDataModel
    {
        public Guid TermClassId { get; set; }
      
        public string Term { get; set; }

        public int? OrderIndex { get; set; }

   
        public string CreatedBy { get; set; }

       
        public DateTime CreatedDate { get; set; }


        public string Source { get; set; }

        public Guid? PId { get; set; }
        public Guid? ConceptClassId { get; set; }
        public string Description { get; set; }

    }
}
