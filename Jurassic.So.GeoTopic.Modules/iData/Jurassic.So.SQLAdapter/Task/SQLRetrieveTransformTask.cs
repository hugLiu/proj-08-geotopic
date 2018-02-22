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
using Jurassic.PKS.Service.Adapter;

namespace Jurassic.So.Adapter
{
    /// <summary>实体内容项转换任务</summary>
    [ETLComponent("SQLRetrieveTransformTask", Title = "实体内容项转换任务")]
    public class SQLRetrieveTransformTask : ETLTransformerTask
    {
        /// <summary>构造函数</summary>
        public SQLRetrieveTransformTask() { }
        /// <summary>准备输出行集合</summary>
        protected override IETLRowCollection PrepareOutput(ETLExecuteContext context, IETLRow inputRow, IETLColumn inputColumn, object inputParameter)
        {
            return new DataSchemaRowCollection();
        }
    }
}
