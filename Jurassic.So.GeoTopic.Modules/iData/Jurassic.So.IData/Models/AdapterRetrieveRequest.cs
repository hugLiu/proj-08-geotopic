using Jurassic.So.Infrastructure;
using Jurassic.So.Infrastructure.Model;
using System;
using System.Runtime.Serialization;

namespace Jurassic.So.Data
{
    /// <summary>获取成果的内容项集合请求数据</summary>
    [Serializable]
    [DataContract]
    public class AdapterRetrieveRequest
    {
        /// <summary>域</summary>
        [DataMember(Name = "scope", IsRequired = true)]
        public string Scope { get; set; }
        /// <summary>成果数据键</summary>
        [DataMember(Name = "naturekey", IsRequired = true)]
        public string NatureKey { get; set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}