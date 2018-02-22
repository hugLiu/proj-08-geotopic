using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.Semantics.IService.ViewModel
{
    public class BOTModel
    {
        public Guid TypeID { get; set; }

        public string BOT { get; set; }

        public string TypeName { get; set; }

        public string Description { get; set; }

        public int? OrderIndex { get; set; }

        public string Remark { get; set; }
    }
}
