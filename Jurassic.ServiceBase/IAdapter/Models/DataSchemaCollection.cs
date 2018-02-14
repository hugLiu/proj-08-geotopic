using Jurassic.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.Adapter
{
    /// <summary>成果的内容项集合</summary>
    [Serializable]
    public class DataSchemaCollection : List<DataSchema>
    {
        /// <summary>构造函数</summary>
        public DataSchemaCollection()
        {
        }
        /// <summary>构造函数</summary>
        public DataSchemaCollection(IEnumerable<DataSchema> values) : base(values)
        {
        }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJsonString();
        }
    }
}
