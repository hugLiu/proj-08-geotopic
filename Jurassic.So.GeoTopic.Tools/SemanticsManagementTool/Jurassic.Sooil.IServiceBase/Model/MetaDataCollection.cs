using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml;

namespace Jurassic.Sooil.IServiceBase
{
    public class MetaDataCollection<T> : List<T>
        where T : class, new()
    {
        public MetaDataCollection()
        {
            Tag = string.Empty;
        }

        public MetaDataCollection(string tag)
        {
            Tag = tag;
        }
        public string Tag { get; set; }
    }

    public class MetaDataCollection : List<MetaData>
    {
        public MetaDataCollection(string tag)
        {
            Tag = tag;
        }
        public string Tag { get; set; }
    }
}
