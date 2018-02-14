using Jurassic.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.Adapter
{
    /// <summary>域信息</summary>
    [Serializable]
    [DataContract]
    public class Scope
    {
        /// <summary>名称</summary>
        [DataMember(Name = "name", IsRequired = true)]
        public string Name { get; set; }
        /// <summary>增量类型</summary>
        [DataMember(Name = "incrementtype", IsRequired = true)]
        public IncrementType IncrementType { get; set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJsonString();
        }
    }
}
