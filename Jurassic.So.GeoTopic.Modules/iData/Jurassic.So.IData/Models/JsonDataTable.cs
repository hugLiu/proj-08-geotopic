using Jurassic.So.Business;
using Jurassic.So.Infrastructure;
using Jurassic.So.Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.Data
{
    /// <summary>JSON数据表</summary>
    [Serializable]
    [DataContract]
    public class JsonDataTable
    {
        /// <summary>构造函数</summary>
        public JsonDataTable() { }
        /// <summary>构造函数</summary>
        public JsonDataTable(DataTable table)
        {
            this.Columns = table.Columns
                .Cast<DataColumn>()
                .Select(e =>  new JsonDataColumn(e))
                .ToArray();
            this.Rows = table.Rows
                .Cast<DataRow>()
                .Select(e => new JsonDataRow(e))
                .ToArray();
        }
        /// <summary>列集合</summary>
        [DataMember(Name = "fields")]
        public JsonDataColumn[] Columns { get; private set; }
        /// <summary>行集合</summary>
        [DataMember(Name = "records")]
        public JsonDataRow[] Rows { get; private set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}
