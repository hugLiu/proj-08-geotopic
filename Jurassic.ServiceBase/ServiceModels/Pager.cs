using System;
using System.Runtime.Serialization;

namespace Jurassic.ServiceModels
{
    /// <summary>分页参数</summary>
    [Serializable]
    [DataContract]
    public class Pager
    {
        /// <summary>构造函数</summary>
        public Pager() { }
        /// <summary>构造函数</summary>
        public Pager(int from, int size)
        {
            this.From = from;
            this.Size = size;
        }
        /// <summary>起始索引</summary>
        [DataMember(Name = "from", IsRequired = true)]
        public int From { get; set; }
        /// <summary>每页数量</summary>
        [DataMember(Name = "size", IsRequired = true)]
        public int Size { get; set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJsonString();
        }
    }
}
