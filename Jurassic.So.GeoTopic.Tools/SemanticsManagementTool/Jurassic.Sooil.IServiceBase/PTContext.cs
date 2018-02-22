using System.Collections.Generic;
using Newtonsoft.Json;

namespace Jurassic.Sooil.IServiceBase
{
    public class PTContext
    {
        public PTContext()
        {
            this.UPT = new List<string>();
            this.CPT = new List<string>();
            this.BD = new List<string>();
            this.UBD = new List<string>();
            this.BT = new List<string>();
            this.UBT = new List<string>();
            this.BP = new List<string>();
            this.UBP = new List<string>();
            this.BA = new List<string>();
            this.UBA = new List<string>();
            this.DS = new List<string>();
            this.UDS = new List<string>();
            this.TL = new List<string>();
            this.GN = new List<string>();
            this.BF = new List<string>();
            this.BS = new List<string>();
            this.BOT = new List<string>();

        }
        public string CurrentPT { get; set; }
        public IList<string> UPT { get; set; }  //使用的成果类型
        public IList<string> CPT { get; set; }  //产生的成果类型
        public IList<string> BD { get; set; }
        public IList<string> UBD { get; set; }
        public IList<string> BT { get; set; }
        public IList<string> UBT { get; set; }
        public IList<string> BP { get; set; }
        public IList<string> UBP { get; set; }
        public IList<string> BA { get; set; }
        public IList<string> UBA { get; set; }
        public IList<string> DS { get; set; }
        public IList<string> UDS { get; set; }
        public IList<string> TL { get; set; }
        public IList<string> GN { get; set; }
        public IList<string> BF { get; set; }
        public IList<string> BS { get; set; }
        public IList<string> BOT { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
