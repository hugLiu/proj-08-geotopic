using System.Linq;
using System.Runtime.Serialization;
using Jurassic.So.Infrastructure;
using Jurassic.So.Business;
using Jurassic.ServiceModels;

namespace Jurassic.So.Search
{
    /// <summary>
    /// 查询匹配结果
    /// </summary>
    [DataContract]
    public class QueryResult
    {
        /// <summary>
        /// 匹配的总记录条数
        /// </summary>
        [DataMember(Name = "count")]
        public long Count { get; set; }
        /// <summary>
        /// 匹配文档信息
        /// </summary>
        [DataMember(Name = "metadatas", IsRequired = true)]
        public MetadataCollection Metadatas { get; set; }
        /// <summary>
        /// 聚合结果
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "groups")]
        public GroupCollection Groups { get; set; }

        /// <summary>
        /// 转化为字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.ToJson();
        }

        /// <summary>
        /// QueryResult转化为Metadata
        /// </summary>
        /// <returns>返回metadatas的第一个值</returns>
        public Metadata ToMetaData()
        {
            if (Metadatas.Count > 0)
            {
                return Metadatas.First().As<Metadata>();
            }
            return null;
        }
    }
}
