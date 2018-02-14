using Jurassic.ServiceModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Jurassic.ServiceModels
{
    /// <summary>元数据集合</summary>
    [Serializable]
    public class MetadataCollection : List<IMetadata>
    {
        /// <summary>构造函数</summary>
        public MetadataCollection()
        {
        }
        /// <summary>构造函数</summary>
        public MetadataCollection(IEnumerable<IMetadata> values) : base(values)
        {
        }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJsonString();
        }
    }
}
