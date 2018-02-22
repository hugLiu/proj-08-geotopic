using System.Threading.Tasks;

namespace Jurassic.So.Search
{
    /// <summary>
    /// ElasticSearch查询接口
    /// </summary>
    /// ES version 5.0.0
    /// Elasticsearch.Net  version 2.4.6
    /// Nest version 2.4.6
    public interface ISearch
    {
        /// <summary>
        /// 查询数据实体元数据
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns>返回查询匹配结果</returns>
        Task<QueryResult> SearchAsync(SearchCondition condition);

        /// <summary>
        /// 查询数据实体元数据
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns>返回查询匹配结果</returns>
        QueryResult Search(SearchCondition condition);
    }
}
