using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Jurassic.Semantics.IService.ViewModel
{
    public class TermSourceModel
    {
        public Guid ConceptClassId { get; set; }

        public string Source { get; set; }

        public DateTime CreateDate { get; set; }

    }
}
