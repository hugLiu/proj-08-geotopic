using Jurassic.So.Infrastructure;
using Jurassic.So.Infrastructure.Model;
using System;
using System.Runtime.Serialization;

namespace Jurassic.So.Data
{
    /// <summary>获取成果的内容项集合请求数据</summary>
    [Serializable]
    [DataContract]
    public class RetrieveRequest
    {
        /// <summary>适配器URL</summary>
        [DataMember(Name = "url", IsRequired = true)]
        public string Url { get; set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}