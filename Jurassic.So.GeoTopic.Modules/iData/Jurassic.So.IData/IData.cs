using Jurassic.So.Infrastructure.Model;
using System.Threading.Tasks;
using Jurassic.So.Infrastructure;
using Jurassic.Adapter;
using Jurassic.ServiceModels;

namespace Jurassic.So.Data
{
    /// <summary>数据组件接口</summary>
    public interface IData
    {
        /// <summary>获得已注册的适配器信息集合</summary>
        AdapterInfoCollection GetAdapterInfos();
        /// <summary>分批或增量爬取某个适配器的某个域的成果的元数据集合</summary>
        /// <param name="adapterId">适配器ID</param>
        /// <param name="scope">适配器的某个域</param>
        /// <param name="incrementValue">增量值，允许为空，如果有值必须是符合该域的增量类型的值</param>
        /// <param name="pager">分页参数，允许为null，为null表示爬取全部，否则只爬取某页数据</param>
        /// <returns>爬取结果</returns>
        SpiderResult Spider(string adapterId, string scope, Pager pager, string incrementValue);
        /// <summary>分批或增量爬取某个适配器的某个域的成果的元数据集合</summary>
        /// <param name="adapterId">适配器ID</param>
        /// <param name="scope">适配器的某个域</param>
        /// <param name="incrementValue">增量值，允许为空，如果有值必须是符合该域的增量类型的值</param>
        /// <param name="pager">分页参数，允许为null，为null表示爬取全部，否则只爬取某页数据</param>
        /// <returns>爬取结果</returns>
        Task<SpiderResult> SpiderAsync(string adapterId, string scope, Pager pager, string incrementValue);
        /// <summary>根据url获取成果的内容项集合</summary>
        /// <param name="url">适配器URL</param>
        /// <returns>成果的内容项集合</returns>
        DataSchemaCollection Retrieve(string url);
        /// <summary>根据url获取成果的内容项集合</summary>
        /// <param name="url">适配器URL</param>
        /// <returns>成果的内容项集合</returns>
        Task<DataSchemaCollection> RetrieveAsync(string url);
        /// <summary>根据url和ticket获取成果的数据项</summary>
        /// <param name="url">适配器URL</param>
        /// <param name="ticket">成果的数据项票据</param>
        /// <param name="pager">分页参数</param>
        /// <returns>成果的数据项结果</returns>
        DataResult GetData(string url, string ticket, Pager pager);
        /// <summary>根据url和ticket获取成果的数据项</summary>
        /// <param name="url">适配器URL</param>
        /// <param name="ticket">成果的数据项票据</param>
        /// <param name="pager">分页参数</param>
        /// <returns>成果的数据项结果</returns>
        Task<DataResult> GetDataAsync(string url, string ticket, Pager pager);
    }
}