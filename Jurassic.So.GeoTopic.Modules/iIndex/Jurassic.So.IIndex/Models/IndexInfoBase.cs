using System;
using System.Runtime.Serialization;
using Jurassic.So.Infrastructure;
using Jurassic.So.Business;

namespace Jurassic.So.Index
{
    /// <summary>索引操作信息基类</summary>
    [Serializable]
    [DataContract]
    public abstract class IndexInfoBase
    {
        /// <summary>构造函数</summary>
        protected IndexInfoBase()
        {
            this.IndexQuality = "100%";
        }
        /// <summary>操作</summary>
        [DataMember(Name = "action", IsRequired = true)]
        public IndexAction Action { get; set; }
        /// <summary>索引质量,默认值为"100%"</summary>
        [DataMember(Name = MetadataConsts.IndexQuality)]
        public string IndexQuality { get; set; }
        ///// <summary>索引选项</summary>
        //[DataMember(Name = "option")]
        //public IndexOption Option { get; set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}