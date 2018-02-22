using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Jurassic.PKS.Service;
using Jurassic.So.ETL;
using Jurassic.So.Infrastructure;
using Newtonsoft.Json.Linq;

namespace Jurassic.So.Adapter
{
    /// <summary>转换器_转换为元数据</summary>
    [ETLComponent("Metadata", Title = "元数据行转换器", Description = "把输入行的值按元数据定义转换并设置到输出行")]
    public class MetadataRowConverter : ETLConverter
    {
        /// <summary>执行转换</summary>
        public override void Execute(ETLExecuteContext context, IETLRow input, IETLRow output, object inputParameter)
        {
            var input2 = input.As<FlatMetadataRow>();
            if (!ValidatePolicy(input2)) return;
            var output2 = output.As<MetadataRow>();
            input2.ToMetadata(output2);
        }
    }
}
