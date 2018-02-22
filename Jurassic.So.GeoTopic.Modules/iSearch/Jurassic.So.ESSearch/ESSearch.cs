using System.Linq;
using System.Threading.Tasks;
using Jurassic.PKS.Service;
using Jurassic.So.Business;
using Jurassic.So.Infrastructure;
using Nest;
using Jurassic.PKS.Service.Search;

namespace Jurassic.So.Search.ES
{
    /// <summary>
    /// ES搜索
    /// </summary>
    public class EsSearch : ISearch
    {
        private readonly SearchProvider<Metadata> _provider = new SearchProvider<Metadata>();
        private readonly ESAccess<Metadata> _access = new ESAccess<Metadata>();

        /// <summary>
        /// ES查询（异步）
        /// </summary>
        /// <param name="request">请求查询条件</param>
        /// <returns>自定义查询结果</returns>
        public async Task<QueryResult> SearchAsync(SearchCondition request)
        {
            var filterQuery = _provider.BuildFilterQuery(request.Filter);
            // var fulltextQuery = _provider.BuildFullTextQuery(request.Sentence, request.Ranks);
            var fulltextQuery = _provider.BuildCustomScoreQuery(request.Sentence, request.Ranks);
            var query = _provider.CombineMustQuery(filterQuery, fulltextQuery);
            var sort = _provider.BuildSort(request.SortRules);
            var aggs = _provider.BuildAggs(request.GroupRule);
            var fields = _provider.BuildFields(request.Fields);
            Pager pager;
            request.Pager.GetValidPager(out pager);
            //拆分参数
            var from = pager.From;
            var size = pager.Size;

            var searchresponse = await _access.PagingQueryAsync(query, sort, fields, aggs, from, size);
            var response = new QueryResult
            {
                Count = searchresponse.Total,
                Metadatas = searchresponse.Documents.ToMetadataList(),
            };
            if (searchresponse.Aggregations.Count <= 0)
                return await Task.FromResult(response);

            var groups = new GroupCollection();
            foreach (var agg in searchresponse.Aggregations)
            {
                var aggregates = agg.Value.As<BucketAggregate>()
                    .Items.ToDictionary(s => s.As<KeyedBucket>().Key, s => s.As<KeyedBucket>().DocCount);
                groups.Add(agg.Key, aggregates);
            }

            response.Groups = groups;
            return await Task.FromResult(response);
        }

        /// <summary>
        /// ES查询
        /// </summary>
        /// <param name="request">请求查询条件</param>
        /// <returns>自定义查询结果</returns>
        public QueryResult Search(SearchCondition request)
        {
            var query = _provider.BuildFilterQuery(request.Filter);
            var sort = _provider.BuildSort(request.SortRules);
            var aggs = _provider.BuildAggs(request.GroupRule);
            var fields = _provider.BuildFields(request.Fields);
            Pager pager;
            request.Pager.GetValidPager(out pager);
            //拆分参数
            var from = pager.From;
            var size = pager.Size;

            var searchresponse = _access.PagingQuery(query, sort, fields, aggs, from, size);

            var response = new QueryResult
            {
                Count = searchresponse.Total,
                Metadatas = searchresponse.Documents.ToMetadataList()
            };
            return response;
        }
    }
}
