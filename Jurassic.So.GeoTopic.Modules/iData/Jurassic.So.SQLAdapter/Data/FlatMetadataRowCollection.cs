using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.So.ETL;
using Jurassic.So.Infrastructure;

namespace Jurassic.So.Adapter
{
    /// <summary>扁平元数据行行集</summary>
    public class FlatMetadataRowCollection : ETLRowCollection<FlatMetadataRow>
    {
        /// <summary>构造函数</summary>
        public FlatMetadataRowCollection()
        {
            this.Columns = new Dictionary<string, IETLColumn>(StringComparer.OrdinalIgnoreCase);
            this.InnerRows = new List<FlatMetadataRow>();
        }
        /// <summary>行集合</summary>
        private List<FlatMetadataRow> InnerRows { get; set; }
        /// <summary>行集合</summary>
        public override IEnumerable<IETLRow> Rows
        {
            get { return this.InnerRows; }
        }
        /// <summary>生成新行</summary>
        public override FlatMetadataRow NewRow()
        {
            return new FlatMetadataRow() { Columns = this.Columns };
        }
        /// <summary>加入行</summary>
        public override void AddRow(FlatMetadataRow row)
        {
            this.InnerRows.Add(row);
        }
    }
}
