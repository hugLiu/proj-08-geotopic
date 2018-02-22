using System.Threading.Tasks;
using Jurassic.So.Infrastructure;

namespace Jurassic.So.Index
{
    /// <summary>索引服务接口</summary>
    public interface IIndexer
    {
        /// <summary>批量保存/更新/删除索引信息</summary>
        /// <param name="indexInfo">索引操作信息</param>
        /// <returns>索引操作结果</returns>
        IndexResult SendIndex(IndexInfo indexInfo);
        /// <summary>批量保存/更新/删除索引信息</summary>
        /// <param name="indexInfo">索引操作信息</param>
        /// <returns>索引操作结果</returns>
        Task<IndexResult> SendIndexAsync(IndexInfo indexInfo);
    }
}
