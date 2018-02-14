using Jurassic.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.Adapter
{
    /// <summary>成果的内容项</summary>
    [Serializable]
    [DataContract]
    public class DataSchema
    {
        /// <summary>名称</summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }
        /// <summary>票据</summary>
        [DataMember(Name = "ticket")]
        public string Ticket { get; set; }
        /// <summary>是否主成果</summary>
        [DataMember(Name = "major")]
        public bool Major { get; set; }
        /// <summary>Mime格式</summary>
        [DataMember(Name = "format")]
        public string Format { get; set; }
        /// <summary>总数</summary>
        [DataMember(Name = "total")]
        public int Total { get; set; }
        /// <summary>单位</summary>
        [DataMember(Name = "unit")]
        public string Unit { get; set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJsonString();
        }
    }
}
