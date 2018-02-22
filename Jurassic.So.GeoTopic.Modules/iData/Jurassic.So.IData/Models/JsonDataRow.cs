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
    /// <summary>JSON数据行</summary>
    [Serializable]
    public class JsonDataRow : Dictionary<string, object>
    {
        /// <summary>构造函数</summary>
        public JsonDataRow() { }
        /// <summary>构造函数</summary>
        public JsonDataRow(DataRow row)
        {
            foreach (DataColumn column in row.Table.Columns)
            {
                base.Add(column.ColumnName, row[column]);
            }
        }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}
