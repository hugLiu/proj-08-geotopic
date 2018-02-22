using Jurassic.So.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.So.ETL;

namespace Jurassic.So.Adapter
{
    /// <summary>NatureKey转换任务</summary>
    [ETLComponent("NatureKeyTransformTask", Title = "NatureKey解析任务")]
    public class NatureKeyTransformTask : ETLDefaultTransformerTask
    {
        /// <summary>构造函数</summary>
        public NatureKeyTransformTask() { }
    }
}
