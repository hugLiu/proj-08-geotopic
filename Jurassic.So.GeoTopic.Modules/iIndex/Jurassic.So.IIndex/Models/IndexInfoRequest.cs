using System;
using System.Runtime.Serialization;
using Jurassic.So.Business;
using Jurassic.So.Infrastructure;

namespace Jurassic.So.Index
{
    /// <summary>索引操作信息请求数据</summary>
    [Serializable]
    [DataContract]
    public class IndexInfoRequest : IndexInfoBase
    {
        /// <summary>元数据集合</summary>
        [DataMember(Name = "kmds", IsRequired = true)]
        public object[] Metadatas { get; set; }
    }
}