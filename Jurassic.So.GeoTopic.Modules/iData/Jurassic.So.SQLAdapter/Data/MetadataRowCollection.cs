using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.So.ETL;
using Jurassic.So.Infrastructure;

namespace Jurassic.So.Adapter
{
    /// <summary>元数据行行集</summary>
    public class MetadataRowCollection : ETLRowCollection<MetadataRow>
    {
        /// <summary>构造函数</summary>
        public MetadataRowCollection()
        {
            this.Columns = new Dictionary<string, IETLColumn>(StringComparer.OrdinalIgnoreCase);
            this.InnerRows = new List<MetadataRow>();
        }
        /// <summary>行集合</summary>
        private List<MetadataRow> InnerRows { get; set; }
        /// <summary>行集合</summary>
        public override IEnumerable<IETLRow> Rows
        {
            get { return this.InnerRows; }
        }
        /// <summary>生成新行</summary>
        public override MetadataRow NewRow()
        {
            return new MetadataRow() { Columns = this.Columns };
        }
        /// <summary>加入行</summary>
        public override void AddRow(MetadataRow row)
        {
            this.InnerRows.Add(row);
        }
    }
}
