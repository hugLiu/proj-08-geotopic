using System.Collections.Generic;
using System.Runtime.Serialization;
using Jurassic.PKS.Service;
using Jurassic.So.Business;
using Jurassic.So.ETL;
using Newtonsoft.Json;

namespace Jurassic.So.Adapter
{
    /// <summary>支持增量爬取的元数据行接口</summary>
    public interface IIncMetadataRow
    {
        /// <summary>获得当前增量列值</summary>
        object IncrementColumnValue { get; }
        /// <summary>取出当前增量列值</summary>
        object TakeIncrementColumnValue();
    }

    /// <summary>元数据行</summary>
    [DataContract]
    public class MetadataRow : JsonMetadata, IETLRow, IIncMetadataRow
    {
        /// <summary>构造函数</summary>
        public MetadataRow() { }
        /// <summary>列字典</summary>
        [IgnoreDataMember, JsonIgnore]
        public IDictionary<string, IETLColumn> Columns { get; set; }
        /// <summary>获得或设置列值</summary>
        [IgnoreDataMember, JsonIgnore]
        object IETLRow.this[string name]
        {
            get { return GetValue(name, false); }
            set { SetValue(name, value); }
        }
        /// <summary>根据列获得或设置列值</summary>
        [IgnoreDataMember, JsonIgnore]
        object IETLRow.this[IETLColumn column]
        {
            get { return GetValue(column.Name, false); }
            set { SetValue(column.Name, value); }
        }
        /// <summary>当前增量列值</summary>
        [IgnoreDataMember, JsonIgnore]
        public object IncrementColumnValue
        {
            get { return GetValue(nameof(this.IncrementColumnValue), false); }
        }
        /// <summary>取出当前增量列值</summary>
        public object TakeIncrementColumnValue()
        {
            var value = this.IncrementColumnValue;
            if (value != null) SetValue(nameof(this.IncrementColumnValue), null);
            return value;
        }
    }
}
