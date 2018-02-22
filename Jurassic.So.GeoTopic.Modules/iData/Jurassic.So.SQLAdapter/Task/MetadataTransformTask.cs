using Jurassic.So.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.So.ETL;
using System.Xml.Linq;
using Jurassic.PKS.Service;

namespace Jurassic.So.Adapter
{
    /// <summary>元数据转换任务</summary>
    [ETLComponent("MetadataTransformTask", Title = "元数据转换任务")]
    public class MetadataTransformTask : ETLTransformerTask
    {
        /// <summary>构造函数</summary>
        public MetadataTransformTask() { }
        /// <summary>元数据标签集合</summary>
        public string MetadataTags { get; set; }
        /// <summary>元数据标签集合列信息</summary>
        private ETLColumnInfo MetadataTagsColumnInfo { get; set; }
        /// <summary>准备输出行集合</summary>
        protected override IETLRowCollection PrepareOutput(ETLExecuteContext context, IETLRow inputRow, IETLColumn inputColumn, object inputParameter)
        {
            var output = new MetadataRowCollection();
            var metadataTags = this.MetadataTagsColumnInfo.GetValue(context, inputRow, null, inputParameter).As<KMDConfiguration>();
            var columns = metadataTags.Select(e => e.Value.As<MetadataTag>().ToMetadataTagColumn(false).As<IETLColumn>()).ToDictionary(e => e.Name);
            output.Columns.AddRange(columns);
            return output;
        }

        #region XML配置方法
        /// <summary>加载</summary>
        public override void LoadXml(ETLXmlConfiguration config, XElement node)
        {
            this.MetadataTags = config.GetElementValue(node, nameof(this.MetadataTags));
            this.MetadataTagsColumnInfo = this.MetadataTags.ETLToColumnInfo();
            base.LoadXml(config, node);
        }
        /// <summary>生成</summary>
        public override void BuildXml(ETLXmlConfiguration config, XElement node)
        {
            config.SetElementValue(node, nameof(this.MetadataTags), this.MetadataTags);
            base.BuildXml(config, node);
        }
        #endregion
    }
}
