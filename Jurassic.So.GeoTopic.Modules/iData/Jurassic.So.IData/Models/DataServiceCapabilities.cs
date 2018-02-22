using Jurassic.So.Business;
using Jurassic.So.Data;
using Jurassic.So.Infrastructure;
using Jurassic.So.Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Jurassic.So.Data
{
    /// <summary>API服务支持数据</summary>
    [Serializable]
    [DataContract]
    public class DataServiceCapabilities : ServiceCapabilities
    {
        /// <summary>适配器信息集合</summary>
        [DataMember(Name = "adapters")]
        public AdapterInfoCollection Adapters { get; set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}