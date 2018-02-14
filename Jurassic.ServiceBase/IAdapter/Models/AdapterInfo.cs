using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Jurassic.ServiceModels;

namespace Jurassic.Adapter
{
    /// <summary>适配器信息</summary>
    [Serializable]
    [DataContract]
    public class AdapterInfo
    {
        /// <summary>ID</summary>
        [DataMember(Name = "id", IsRequired = true)]
        public string Id { get; set; }
        /// <summary>数据源名称</summary>
        [DataMember(Name = "datasourcename")]
        public string DataSourceName { get; set; }
        /// <summary>数据源类型</summary>
        [DataMember(Name = "datasourcetype")]
        public string DataSourceType { get; set; }
        /// <summary>域集合</summary>
        [DataMember(Name = "scopes")]
        public ScopeCollection Scopes { get; set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJsonString();
        }
    }
}
