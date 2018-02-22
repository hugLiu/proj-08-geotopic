using System;
using System.Runtime.Serialization;
using Jurassic.So.Business;
using System.Collections.Generic;
using Jurassic.So.Infrastructure;

namespace Jurassic.So.Index
{
    /// <summary>索引操作结果</summary>
    [Serializable]
    [DataContract]
    public class IndexResult
    {
        /// <summary>构造函数</summary>
        public IndexResult()
        {
            this.IIIds = new List<string>();
        }
        /// <summary>元数据ID集合</summary>
        [DataMember(Name = "iiids")]
        public List<string> IIIds { get; private set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}