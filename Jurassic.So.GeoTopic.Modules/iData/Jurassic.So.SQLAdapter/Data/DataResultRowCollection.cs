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
    public class DataResultRowCollection : ETLRowCollection<DataResultRow>
    {
        /// <summary>构造函数</summary>
        public DataResultRowCollection()
        {
            this.Columns = typeof(DataResultRow).ETLToEntityColumns(nameof(this.Columns), "Item")
                .Cast<IETLColumn>()
                .ToDictionary(e => e.Name, StringComparer.OrdinalIgnoreCase);
            this.InnerRows = new List<DataResultRow>();
        }
        /// <summary>行集合</summary>
        private List<DataResultRow> InnerRows { get; set; }
        /// <summary>行集合</summary>
        public override IEnumerable<IETLRow> Rows
        {
            get { return this.InnerRows; }
        }
        /// <summary>生成新行</summary>
        public override DataResultRow NewRow()
        {
            return new DataResultRow() { Columns = this.Columns };
        }
        /// <summary>加入行</summary>
        public override void AddRow(DataResultRow row)
        {
            this.InnerRows.Add(row);
        }
    }
}
