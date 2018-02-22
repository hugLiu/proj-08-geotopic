using System;
using System.Runtime.Serialization;
using Jurassic.So.Business;
using Jurassic.So.Infrastructure;
using Jurassic.ServiceModels;

namespace Jurassic.So.Index
{
    /// <summary>索引操作信息</summary>
    [Serializable]
    [DataContract]
    public class IndexInfo : IndexInfoBase
    {
        /// <summary>元数据集合</summary>
        [DataMember(Name = "kmds", IsRequired = true)]
        public MetadataCollection Metadatas { get; set; }
    }
}