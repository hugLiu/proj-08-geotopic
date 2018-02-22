using System.Threading.Tasks;
using Jurassic.So.Business;

namespace Jurassic.So.Search
{
    /// <summary>
    /// MongoDB查询接口
    /// </summary>
    public interface IQuery
    {
        /// <summary>
        /// 查询数据实体元数据
        /// </summary>
        /// <param name="request">查询请求条件</param>
        /// <returns>返回匹配结果</returns>
        Task<QueryResult> MatchAsync(MatchCondtion request);
        /// <summary>
        /// 查询数据实体元数据
        /// </summary>
        /// <param name="request">查询请求条件</param>
        /// <returns>返回匹配结果</returns>
        QueryResult Match(MatchCondtion request);

        /// <summary>
        /// 根据给定的标签，返回所有包含该标签的标签
        /// </summary>
        /// <param name="tagnames">标签名称（支持通配）</param>
        /// <returns>返回匹配标签本身信息</returns>
        Task<MetadataDefinitionCollection> GetMetadataDefinitionAsync(params string[] tagnames);
        /// <summary>
        /// 根据给定的标签，返回所有包含该标签的标签
        /// </summary>
        /// <param name="tagnames">标签名称（支持通配）</param>
        /// <returns>返回匹配标签本身信息</returns>
        MetadataDefinitionCollection GetMetadataDefinition(params string[] tagnames);

        /// <summary>
        /// 获取元数据统计信息
        /// </summary>
        /// <returns>返回统计信息</returns>
        Task<StatisticsInfo> StatisticsAsync();
        /// <summary>
        /// 获取元数据统计信息
        /// </summary>
        /// <returns>返回统计信息</returns>
        StatisticsInfo Statistics();
    }
}
