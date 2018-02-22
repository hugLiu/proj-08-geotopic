using Jurassic.So.Infrastructure;
using Jurassic.So.Infrastructure.Model;
using System;
using System.Runtime.Serialization;

namespace Jurassic.So.Data
{
    /// <summary>成果的数据项请求数据</summary>
    [Serializable]
    [DataContract]
    public class AdapterGetDataRequest : INullablePager
    {
        /// <summary>构造函数</summary>
        public AdapterGetDataRequest()
        {
            this.From = 0;
            this.Size = 0;
        }
        /// <summary>成果的数据项票据</summary>
        [DataMember(Name = "ticket", IsRequired = true)]
        public string Ticket { get; set; }
        /// <summary>起始索引</summary>
        [DataMember(Name = "from")]
        public int? From { get; set; }
        /// <summary>每页数量</summary>
        [DataMember(Name = "size")]
        public int? Size { get; set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}