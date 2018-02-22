using Jurassic.Adapter;
using Jurassic.So.Business;
using Jurassic.So.Infrastructure;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Jurassic.So.Data
{
    /// <summary>API服务支持数据</summary>
    [Serializable]
    [DataContract]
    public class AdapterServiceCapabilities : ServiceCapabilities
    {
        /// <summary>适配器信息</summary>
        [DataMember(Name = "adapter")]
        public AdapterInfo Adapter { get; set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}