using System.Collections.Generic;
using System.Threading.Tasks;
using Jurassic.PKS.Service;
using Jurassic.PKS.Service.Adapter;
using Jurassic.So.ETL;

namespace Jurassic.So.Adapter
{
    /// <summary>适配器域接口</summary>
    public interface IAdapterScope
    {
        /// <summary>配置</summary>
        AdapterConfigurationSectionHandler Configuration { get; set; }
        /// <summary>适配器域信息</summary>
        Scope ScopeInfo { get; set; }
        /// <summary>爬取分页大小</summary>
        int SpiderSize { get; set; }
        /// <summary>公共任务</summary>
        Dictionary<string, ETLTask> CommonTasks { get; set; }
        /// <summary>变量字典</summary>
        Dictionary<string, object> Variables { get; set; }
        /// <summary>获取元数据总数包</summary>
        ETLPackage SpiderCountPackage { get; set; }
        /// <summary>获取元数据包</summary>
        ETLPackage SpiderPackage { get; set; }
        /// <summary>获取数据内容项包</summary>
        ETLPackage RetrievePackage { get; set; }
        /// <summary>获取数据内容包</summary>
        ETLPackage GetDataPackage { get; set; }
        /// <summary>获得元数据总数</summary>
        int GetMetadataTotal(ETLExecuteContext context, string incrementValue);
        /// <summary>获得元数据集合</summary>
        MetadataCollection GetMetadatas(ETLExecuteContext context, string incrementValue, Pager pager);
        /// <summary>根据域和成果键获取成果的内容项集合</summary>
        DataSchemaCollection Retrieve(ETLExecuteContext context, string natureKey);
        /// <summary>根据数据项票据获取成果的数据项</summary>
        DataResult GetData(ETLExecuteContext context, string natureKey, string ticket, Pager pager);
    }
}
