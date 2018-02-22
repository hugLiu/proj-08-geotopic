using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.Sooil.IServiceBase
{
    public class Date
    {
        public Date()
        {
        }
        public Date(string type,DateTime value)
        {
            this.Type = type;
            this.Value = value;
        }
        public string Type { get; set; }
        public DateTime Value { get; set; }
    }
}
