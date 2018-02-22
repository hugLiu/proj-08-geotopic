using Jurassic.Adapter;
using Jurassic.So.Infrastructure;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Jurassic.So.Data
{
    /// <summary>适配器信息集合</summary>
    [Serializable]
    public class AdapterInfoCollection : List<AdapterInfo>
    {
        /// <summary>构造函数</summary>
        public AdapterInfoCollection()
        {
        }
        /// <summary>构造函数</summary>
        public AdapterInfoCollection(IEnumerable<AdapterInfo> values) : base(values)
        {
        }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}
