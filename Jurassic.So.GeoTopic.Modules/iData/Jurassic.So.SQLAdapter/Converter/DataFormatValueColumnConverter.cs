using Jurassic.So.ETL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Jurassic.So.Infrastructure;
using Jurassic.So.Business;
using Jurassic.PKS.Service.Adapter;
using Jurassic.PKS.Service;

namespace Jurassic.So.Adapter
{
    /// <summary>数据格式值列转换器</summary>
    [ETLComponent("DataFormatValue", Title = "数据格式值列转换器", Description = "根据数据格式转换源列的值并设置到目标列！")]
    public sealed class DataFormatValueColumnConverter : ETLConverter
    {
        /// <summary>执行转换</summary>
        public override void Execute(ETLExecuteContext context, IETLRow input, IETLRow output, object inputParameter)
        {
            var inputValue = this.InputColumns[0].GetValue(context, input, output, inputParameter);
            if (!ValidatePolicy(inputValue)) return;
            var inputValue2 = this.InputColumns[1].GetValue(context, input, output, inputParameter);
            if (!ValidatePolicy(inputValue)) return;
            var format = inputValue.ToString().ToDataFormat();
            object outputValue = null;
            switch (format)
            {
                case DataFormat.HTML:
                case DataFormat.XML:
                case DataFormat.TXT:
                case DataFormat.JSON:
                case DataFormat.DataSet:
                case DataFormat.URL:
                    outputValue = Encoding.UTF8.GetString(inputValue2.As<byte[]>());
                    break;
                default:
                    outputValue = inputValue2;
                    break;
            }
            this.DefaultOutputColumn.SetValue(context, output, inputParameter, outputValue);
        }
    }
}
