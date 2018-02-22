using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Jurassic.PKS.Service;
using Jurassic.So.ETL;

namespace Jurassic.So.Adapter
{
    /// <summary>扁平元数据行</summary>
    [DataContract]
    public class FlatMetadataRow : FlatMetadata, IETLRow
    {
        /// <summary>构造函数</summary>
        public FlatMetadataRow() { }
        /// <summary>列字典</summary>
        [IgnoreDataMember]
        public IDictionary<string, IETLColumn> Columns { get; set; }
        /// <summary>获得或设置列值</summary>
        public new object this[string name]
        {
            get { return GetValue(name); }
            set { SetValue(name, value); }
        }
        /// <summary>根据列获得或设置列值</summary>
        public object this[IETLColumn column]
        {
            get { return this[column.Name]; }
            set { this[column.Name] = value; }
        }
    }
}
