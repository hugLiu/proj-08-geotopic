using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Jurassic.PKS.Service;
using Jurassic.PKS.Service.Adapter;
using Jurassic.So.ETL;
using Jurassic.So.Infrastructure;

namespace Jurassic.So.Adapter
{
    /// <summary>内容项行</summary>
    [DataContract]
    public class DataSchemaRow : DataSchema, IETLRow
    {
        /// <summary>构造函数</summary>
        public DataSchemaRow() { }
        /// <summary>列字典</summary>
        [IgnoreDataMember]
        public IDictionary<string, IETLColumn> Columns { get; set; }
        /// <summary>格式</summary>
        [IgnoreDataMember]
        public string Format2 { get; set; }
        /// <summary>获得或设置列值</summary>
        public object this[string name]
        {
            get { return this[this.Columns[name]]; }
            set { this[this.Columns[name]] = value; }
        }
        /// <summary>根据列获得或设置列值</summary>
        public object this[IETLColumn column]
        {
            get { return column.As<ETLEntityColumn>().Accessor.GetValue(this); }
            set { column.As<ETLEntityColumn>().Accessor.SetValue(this, value); }
        }
    }
}
