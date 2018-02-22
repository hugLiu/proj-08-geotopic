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

namespace Jurassic.So.Adapter
{
    /// <summary>数据格式列转换器</summary>
    [ETLComponent("DataFormat", Title = "数据格式列转换器", Description = "转换源列的值中到数据格式并设置到目标列！")]
    public sealed class DataFormatColumnConverter : ETLConverter
    {
        /// <summary>执行转换</summary>
        public override void Execute(ETLExecuteContext context, IETLRow input, IETLRow output, object inputParameter)
        {
            var inputValue = this.DefaultInputColumn.GetValue(context, input, output, inputParameter);
            if (!ValidatePolicy(inputValue)) return;
            var outputValue = inputValue?.ToString().ToDataFormat();
            if (!outputValue.HasValue)
            {
                AdapterExceptionCode.InvalidDataFormat.ThrowUserFriendly("无效的数据内容格式！", $"无效的数据内容格式[{inputValue ?? "null"}]！");
            }
            this.DefaultOutputColumn.SetValue(context, output, inputParameter, outputValue.Value);
        }
    }
}
