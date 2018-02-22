using System.Linq;
using System.Threading.Tasks;
using Jurassic.So.Business;
using Jurassic.So.Search.Mongo;
using Jurassic.PKS.Service.Search;
using Jurassic.PKS.Service;

namespace Jurassic.So.SearchMock
{
    /// <summary>
    ///     mongosearch demo 程序
    /// </summary>
    public class MongoSearchMock : IQuery
    {
        /// <summary>
        /// mongo搜索 正式实现
        /// </summary>
        private MongoSearch _search = new MongoSearch();
        /// <summary>
        /// 匹配搜索（异步）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<QueryResult> MatchAsync(MatchCondition request)
        {
            _search = new MongoSearch();
            var format = request.Fields;
            if (format.Count>0&&!string.IsNullOrEmpty(format.First().Key))
            {
                var result = new QueryResult
                {
                    Metadatas = new MetadataCollection()
                {
                    new Metadata {{"s:url", $"ADP://CNKI测试/Test/{format.First().Key}"}}
                }
                };
                return Task.FromResult(result);
            }
            return _search.MatchAsync(request);
        }

        /// <summary>
        /// 匹配搜索同步
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public QueryResult Match(MatchCondition request)
        {
            return _search.Match(request);
        }

        /// <summary>
        /// 获得元数据定义（异步）
        /// </summary>
        /// <param name="tagnames"></param>
        /// <returns></returns>
        public async Task<MetadataDefinitionCollection> GetMetadataDefinitionAsync(params string[] tagnames)
        {
            return await _search.GetMetadataDefinitionAsync(tagnames);
        }

        /// <summary>
        /// 获得元数据定义
        /// </summary>
        /// <param name="tagnames"></param>
        /// <returns></returns>
        public MetadataDefinitionCollection GetMetadataDefinition(params string[] tagnames)
        {
            return _search.GetMetadataDefinition(tagnames);
        }

        /// <summary>
        /// 统计（异步）
        /// </summary>
        /// <returns></returns>
        public Task<StatisticsInfo> StatisticsAsync()
        {
            return _search.StatisticsAsync();
        }

        /// <summary>
        /// 统计
        /// </summary>
        /// <returns></returns>
        public StatisticsInfo Statistics()
        {
            return _search.Statistics();
        }
    }
}
