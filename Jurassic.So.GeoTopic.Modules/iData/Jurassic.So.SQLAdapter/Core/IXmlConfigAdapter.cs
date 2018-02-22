using System.Collections.Generic;
using Jurassic.PKS.Service;
using Jurassic.PKS.Service.Adapter;
using Jurassic.So.ETL;

namespace Jurassic.So.Adapter
{
    /// <summary>支持XML配置的适配器接口</summary>
    public interface IXmlConfigAdapter
    {
        /// <summary>配置</summary>
        AdapterConfigurationSectionHandler Configuration { get; set; }
        /// <summary>适配器信息</summary>
        AdapterInfo AdapterInfo { get; set; }
        /// <summary>连接字典</summary>
        Dictionary<string, ETLConnection> Connections { get; set; }
        /// <summary>变量字典</summary>
        Dictionary<string, object> Variables { get; set; }
        /// <summary>域集合</summary>
        List<IAdapterScope> Scopes { get; set; }
        /// <summary>元数据标签字典</summary>
        KMDConfiguration MetadataTags { get; set; }
    }
}
