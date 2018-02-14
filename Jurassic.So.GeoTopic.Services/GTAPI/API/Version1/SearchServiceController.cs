using System.Threading.Tasks;
using System.Web.Http;
using Jurassic.So.Infrastructure;
using Jurassic.PKS.Service.Search;
using Jurassic.PKS.Service;
using Jurassic.PKS.WebAPI.Models;

namespace GTAPI.API.Version1
{
    /// <summary>
    /// 搜索服务API
    /// </summary>
    public class SearchServiceController : WebAPIController
    {
        private IQuery QueryService { get; }
        private ISearch SearchService { get; }
        private const string Iiid = "iiid";
        private const string Tb = "thumbnail";
        private const string Ft = "fulltext";
        /// <summary>
        /// 依赖注入
        /// </summary>
        public SearchServiceController(ISearch search, IQuery query)
        {
            this.SearchService = search;
            this.QueryService = query;
        }

        /// <summary>
        /// 从ES查询，支持全文检索，内部调用ISearch.Search方法
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>返回搜索结果（记录总条数、文档集合、聚合结果）</returns>
        [HttpPost]
        public async Task<QueryResult> Search(SearchRequest request)
        {
            if (request != null)
            {
                return await SearchService.SearchAsync(request.MapTo<SearchCondition>());
            }
            return null;
        }

        /// <summary>
        /// 从MongoDB中查询，匹配标签查询，内部调用IQuery.Match方法
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns>返回搜索结果（记录总条数、文档集合、聚合结果）</returns>
        [HttpPost]
        public async Task<QueryResult> Match(MatchRequest request)
        {
            if (request != null)
            {
                return await QueryService.MatchAsync(request.MapTo<MatchCondition>());
            }
            return null;
        }

        /// <summary>
        /// 从MongoDB中查询，根据iiid获取元数据，内部调用IQuery.Match方法
        /// </summary>
        /// <param name="iiid">文档唯一标识</param>
        /// <param name="category">元数据分类</param>
        /// <returns>返回分类元数据信息</returns>
        [HttpGet]
        public async Task<IMetadata> GetMetaData(string iiid, Category category = Category.All)
        {
            var query = new MatchCondition();
            if (!string.IsNullOrEmpty(iiid))
            {
                //query.Filter = ConditionExpressionBuilder.Eq(Iiid, iiid).ToExpressionJson();
                query.Filter = ConditionExpressionBuilder.Eq(Iiid, iiid);
            }
            else return null;
            //返回指定分类的元数据信息
            if (category != Category.All)
            {
                query.Fields.Add(category.GetRemark(), 1);
            }
            var retData = await QueryService.MatchAsync(query);
            return retData.ToMetaData();
        }

        /// <summary>
        ///  从MongoDB中查询，根据iiid获取缩略图数据，内部调用IQuery.Match方法。
        /// </summary>
        /// <param name="iiid">文档唯一标识</param>
        /// <returns>返回缩略图信息</returns>
        [HttpGet]
        public async Task<IMetadata> GetThumbnail(string iiid)
        {
            var query = new MatchCondition();
            if (!string.IsNullOrEmpty(iiid))
            {
                //query.Filter = ConditionExpressionBuilder.Eq(Iiid, iiid).ToExpressionJson();
                query.Filter = ConditionExpressionBuilder.Eq(Iiid, iiid);
                query.Fields = new FieldCollection { { Tb, 1 } };
            }
            else
            {
                return null;
            }
            var retData = await QueryService.MatchAsync(query);
            return retData.ToMetaData();
        }

        /// <summary>
        /// 从MongoDB中查询，根据iiid获取全文数据，内部调用IQuery.Match方法。
        /// </summary>
        /// <param name="iiid">文档唯一标识</param>
        /// <returns>返回全文信息</returns>
        [HttpGet]
        public async Task<IMetadata> GetFulltext(string iiid)
        {
            var query = new MatchCondition();
            if (!string.IsNullOrEmpty(iiid))
            {
                //query.Filter = ConditionExpressionBuilder.Eq(Iiid, iiid).ToExpressionJson();
                query.Filter = ConditionExpressionBuilder.Eq(Iiid, iiid);
                query.Fields = new FieldCollection() { { Ft, 1 } };
            }
            var retData = await QueryService.MatchAsync(query);
            return retData.ToMetaData();
        }

        /// <summary>
        /// 从MongoDB中查询，获取元数据统计信息，内部调用IQuery.Statistics
        /// </summary>
        /// <returns>返回索引统计信息</returns>
        [HttpGet]
        public async Task<StatisticsInfo> Statistics()
        {
            return await QueryService.StatisticsAsync();
        }

        /// <summary>
        /// 从MongoDB中查询，获取标签信息，内部调用IQuery.GetMetadataDefinition
        /// </summary>
        /// <param name="tagNames">标签名称集合</param>
        /// <returns>返回匹配标签的信息</returns>
        [HttpPost]
        public async Task<MetadataDefinitionCollection> GetMetadataDefinition(string[] tagNames)
        {
            return QueryService.GetMetadataDefinition(tagNames);
        }

        /// <summary>获得服务信息</summary>
        protected override ServiceInfo GetServiceInfo()
        {
            return new ServiceInfo()
            {
                Description = "搜索服务主要用于从索引库中查询获得匹配的成果"
            };
        }
    }
}
