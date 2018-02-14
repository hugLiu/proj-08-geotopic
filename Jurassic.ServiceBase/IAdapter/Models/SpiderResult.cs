using Jurassic.ServiceModels;
using System;
using System.Runtime.Serialization;

namespace Jurassic.Adapter
{
    /// <summary>成果的元数据集合爬取结果</summary>
    [Serializable]
    [DataContract]
    public class SpiderResult
    {
        /// <summary>总数</summary>
        [DataMember(Name = "total")]
        public int Total { get; set; }
        /// <summary>元数据集合</summary>
        [DataMember(Name = "kmds")]
        public MetadataCollection Metadatas { get; set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJsonString();
        }
    }
}
