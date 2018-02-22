using System;
using Jurassic.PKS.Service;
using Jurassic.PKS.Service.Search;
using Jurassic.So.Business;
using Jurassic.So.Infrastructure;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;
using Jurassic.So.Infrastructure.Util;

namespace Jurassic.So.Search.Mongo
{
    /// <summary>
    /// Mongo查询
    /// </summary>
    public class MongoSearch : IQuery
    {
        private readonly QueryProvider _provider = new QueryProvider();
        private readonly string matchTag = "mapping.set.key";
        private readonly string MetadatadefCollectionName = "MetadataDefinition";
        /// <summary>
        /// 信息搜索
        /// </summary>
        /// <param name="request">请求的查询参数</param>
        /// <returns></returns>
        public async Task<QueryResult> MatchAsync(MatchCondition request)
        {
            //支持字段通配的转化
            var requestParams = request.SupportWildCard();
            SortDefinition<BsonDocument> sorts = null;
            Pager pager;
            requestParams.Pager.GetValidPager(out pager);
            //拆分参数
            var from = pager.From;
            var size = pager.Size;

            var sortCondition = requestParams.SortRules;
            var groupCondition = requestParams.GroupRule;
            var fields = requestParams.Fields;
            //var filterCondition = requestParams.Filter == "null" || requestParams.Filter == "{}" ? null : requestParams.Filter;
            var filterCondition = requestParams.Filter.ToExpressionJson();

            var access = new MongoAccessAuto();
            //构建条件
            var filter = string.IsNullOrEmpty(filterCondition) ? null : _provider.GetFilters(filterCondition);
            var projects = fields == null ? null : _provider.GetProjects(fields);
            if (sortCondition != null)
            {
                sorts = _provider.GetSorts(sortCondition.ToDictionary(s => s.Key, s => s.Value.Direction));
            }
            //获得分页搜索结果
            var docs = await access.GetPaginationAsync(filter, sorts, projects, from, size);
            var dics = docs.ConvertAll(s => s.ToDictionary());
            var metadatacollection = dics.ToMetadataList();
            //获得匹配文档总个数
            var count = await access.GetCountAsync(filter);

            var response = new QueryResult();
            //获得聚合结果
            var gc = new GroupCollection();
            if (groupCondition?.GFields != null)
            {
                foreach (var field in groupCondition.GFields)
                {
                    var tokens = await _provider.GetAggsAsync(filterCondition, groupCondition.Top, field);
                    if (gc.ContainsKey(field)) continue;
                    gc.Add(field, tokens.SortByValue());
                }
                response.Groups = gc;
            }
            response.Count = count;
            response.Metadatas = metadatacollection;
            return response;
        }

        /// <summary>
        /// 信息搜索
        /// </summary>
        /// <param name="request">查询条件</param>
        /// <returns></returns>
        public QueryResult Match(MatchCondition request)
        {
            var requestParams = request.SupportWildCard();
            SortDefinition<BsonDocument> sorts = null;
            //拆分参数
            Pager pager;
            requestParams.Pager.GetValidPager(out pager);
            //拆分参数
            var from = pager.From;
            var size = pager.Size;

            var sortCondition = requestParams.SortRules;
            var groupCondition = requestParams.GroupRule;
            var fields = requestParams.Fields;
            // var filterCondition = requestParams.Filter == "null" || requestParams.Filter == "{}" ? null : requestParams.Filter;
            var filterCondition = requestParams.Filter.ToExpressionJson();

            var access = new MongoAccessAuto();
            //构建条件
            var filter = string.IsNullOrEmpty(filterCondition) ? null : _provider.GetFilters(filterCondition);
            var projects = fields == null ? null : _provider.GetProjects(fields);
            if (sortCondition != null)
            {
                sorts = _provider.GetSorts(sortCondition.ToDictionary(s => s.Key, s => s.Value.Direction));
            }
            //获得分页搜索结果
            var docs = access.GetPagination(filter, sorts, projects, from, size);
            var metadatas = docs.ConvertAll(s => s.ToDictionary<Metadata>()).Cast<IMetadata>().ToList(new MetadataCollection());
            //获得匹配文档总个数
            var count = access.GetCount(filter);
            var response = new QueryResult();
            //获得聚合结果
            var gc = new GroupCollection();
            if (groupCondition?.GFields != null)
            {
                foreach (var field in groupCondition.GFields)
                {
                    var tokens = _provider.GetAggs(filterCondition, groupCondition.Top, field);
                    if (gc.ContainsKey(field)) continue;
                    gc.Add(field, tokens.SortByValue());
                }
                response.Groups = gc;
            }
            response.Count = count;
            response.Metadatas = metadatas;
            return response;
        }

        /// <summary>
        /// 根据给定的标签，返回所有包含该标签的标签
        /// </summary>
        /// <param name="tagnames"></param>
        /// <returns></returns>
        public async Task<MetadataDefinitionCollection> GetMetadataDefinitionAsync(params string[] tagnames)
        {
            var mongoAccess = new MongoAccessAuto(MetadatadefCollectionName);
            var filter = _provider.GetRegexFilters(matchTag, tagnames);
            var project = _provider.GetDefaultProjects();
            var docs = await mongoAccess.GetPaginationAsync(filter, project);
            var metadataDefinitions = docs.ConvertAll(doc => doc.ToJson().JsonTo<MetadataDefinition>());
            var result = new MetadataDefinitionCollection(metadataDefinitions);
            return result;
        }

        /// <summary>
        /// 根据给定的标签，返回所有包含该标签的标签
        /// </summary>
        /// <param name="tagnames"></param>
        /// <returns></returns>
        public MetadataDefinitionCollection GetMetadataDefinition(params string[] tagnames)
        {
            var mongoAccess = new MongoAccessAuto(MetadatadefCollectionName);
            var filter = _provider.GetRegexFilters(matchTag, tagnames);
            var project = _provider.GetDefaultProjects();
            var docs = mongoAccess.GetPagination(filter, project);
            var metadataDefinitions = docs.ConvertAll(doc => doc.ToJson().JsonTo<MetadataDefinition>());
            var result = new MetadataDefinitionCollection(metadataDefinitions);

            //string jsonString;
            //var path = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"App_Data\MetadataDefinition.json") ;
            //using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            //using (var sr = new StreamReader(fs))
            //{
            //    jsonString = sr.ReadToEnd();
            //}
            //var result = jsonString.JsonTo<MetadataDefinitionCollection>();
            return result;
        }

        /// <summary>
        /// 获取元数据的统计信息
        /// </summary>
        /// <returns></returns>
        public async Task<StatisticsInfo> StatisticsAsync()
        {
            var result = new StatisticsInfo();
            var mongoAccess = new MongoAccessAuto();
            var count = await mongoAccess.GetCountAsync(null);
            result.Add("documentcounts", count);
            return result;
        }

        /// <summary>
        /// 获取元数据的统计信息
        /// </summary>
        /// <returns></returns>
        public StatisticsInfo Statistics()
        {
            var result = new StatisticsInfo();
            var mongoAccess = new MongoAccessAuto();
            var count = mongoAccess.GetCount(null);
            result.Add("documentcounts", count);
            return result;
        }
    }
}
