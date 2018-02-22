using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Jurassic.PKS.Service;
using Jurassic.So.ETL;
using Jurassic.So.Infrastructure;

namespace Jurassic.So.Adapter
{
    /// <summary>转换器_转换数据集</summary>
    [ETLComponent("JsonDataSet", Title = "JSON数据集转换器", Description = "把数据集输入设置到输出行")]
    public class JsonDataSetConverter : ETLConverter
    {
        /// <summary>执行转换</summary>
        public override void Execute(ETLExecuteContext context, IETLRow input, IETLRow output, object inputParameter)
        {
            var inputValue = context.InputRows.As<ETLDbDataSet>().InnerDataSet;
            if (!ValidatePolicy(inputValue)) return;
            if (inputValue == null) inputValue = new DataSet();
            var outputValue = new DataSetDataResult(inputValue);
            this.DefaultOutputColumn.SetValue(context, output, inputParameter, outputValue);
        }
    }
}
