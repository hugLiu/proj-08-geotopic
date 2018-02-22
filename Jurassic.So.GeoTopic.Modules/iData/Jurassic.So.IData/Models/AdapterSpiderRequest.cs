using Jurassic.ServiceModels;
using Jurassic.So.Infrastructure;
using Jurassic.So.Infrastructure.Model;
using System;
using System.Runtime.Serialization;

namespace Jurassic.So.Data
{
    /// <summary>爬取成果的元数据集合请求数据</summary>
    [Serializable]
    [DataContract]
    public class AdapterSpiderRequest
    {
        /// <summary>域</summary>
        [DataMember(Name = "scope", IsRequired = true)]
        public string Scope { get; set; }
        /// <summary>增量值</summary>
        [DataMember(Name = "incrementvalue")]
        public string IncrementValue { get; set; }
        /// <summary>分页参数</summary>
        [DataMember(Name = "pager")]
        public Pager Pager { get; set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}