using Jurassic.Sooil.IServiceBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.Sooil.IServiceBase
{
    public class Usage2 : JsonBase
    {
        public Usage2()
        {
            this.Media = string.Empty;
            this.SRC = new SRC2();
            this.View = string.Empty;
            this.Format = string.Empty;
        }
        public string Media { get; set; }
        public SRC2 SRC { get; set; }
        public string View { get; set; }
        public string Format { get; set; }
    }
}
