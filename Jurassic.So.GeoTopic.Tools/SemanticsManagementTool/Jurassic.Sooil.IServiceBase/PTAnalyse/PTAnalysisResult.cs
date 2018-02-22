using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.Sooil.IServiceBase
{
    public class PTAnalysisResult
    {
        public string AnalysisSource { get; set; }

        public List<PTItem> PTItems { get; set; }
    }
}
