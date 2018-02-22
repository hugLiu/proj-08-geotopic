using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.So.ETL;
using Jurassic.So.Infrastructure;

namespace Jurassic.So.Adapter
{
    /// <summary>内容项行行集</summary>
    public class DataSchemaRowCollection : ETLRowCollection<DataSchemaRow>
    {
        /// <summary>构造函数</summary>
        public DataSchemaRowCollection()
        {
            this.Columns = typeof(DataSchemaRow).ETLToEntityColumns(nameof(this.Columns), "Item")
                .Cast<IETLColumn>()
                .ToDictionary(e => e.Name, StringComparer.OrdinalIgnoreCase);
            this.InnerRows = new List<DataSchemaRow>();
        }
        /// <summary>行集合</summary>
        private List<DataSchemaRow> InnerRows { get; set; }
        /// <summary>行集合</summary>
        public override IEnumerable<IETLRow> Rows
        {
            get { return this.InnerRows; }
        }
        /// <summary>生成新行</summary>
        public override DataSchemaRow NewRow()
        {
            return new DataSchemaRow() { Columns = this.Columns };
        }
        /// <summary>加入行</summary>
        public override void AddRow(DataSchemaRow row)
        {
            this.InnerRows.Add(row);
        }
    }
}
