using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Jurassic.Sooil.IServiceBase//Jurassic.Sooil.IServiceBase
{
    public class Groups:List<Group>
    {
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
