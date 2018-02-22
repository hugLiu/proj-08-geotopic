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
    /// <summary>实体内容转换任务</summary>
    [ETLComponent("SQLGetDataTransformTask", Title = "实体内容转换任务")]
    public class SQLGetDataTransformTask : ETLTransformerTask
    {
        /// <summary>构造函数</summary>
        public SQLGetDataTransformTask() { }
        ///// <summary>准备输入行集合</summary>
        //protected override IEnumerable<IETLRow> PrepareInput(ETLExecuteContext context, IETLRow inputRow, IETLColumn inputColumn, object inputParameter)
        //{
        //    return new IETLRow[] { null };
        //}
        /// <summary>准备输出行集合</summary>
        protected override IETLRowCollection PrepareOutput(ETLExecuteContext context, IETLRow inputRow, IETLColumn inputColumn, object inputParameter)
        {
            return new DataResultRowCollection();
        }
    }
}
