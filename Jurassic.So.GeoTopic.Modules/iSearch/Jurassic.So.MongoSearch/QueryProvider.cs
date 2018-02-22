using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Jurassic.So.Infrastructure;
using MongoDB.Bson;
using MongoDB.Driver;
using Jurassic.PKS.Service;
using Jurassic.PKS.Service.Search;

namespace Jurassic.So.Search.Mongo
{
    /// <summary>
    /// Mongo查询provider
    /// </summary>
    public class QueryProvider
    {
        private const string GroupId = "_id";
        private const string GroupCount = "count";
        #region MongoDB查询结果转化
        /// <summary>
        /// 获得单个字段的聚合结果
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="limit"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, long?>> GetAggsAsync(string filter, int limit, string field)
        {
            var access = new MongoAccessAuto();
            var pipeline = GetPipeLine(filter, limit, field);
            List<BsonDocument> aggs;
            try
            {
                aggs = await access.GetAggregateAsync(pipeline);
            }
            catch (Exception)
            {
                pipeline = GetPipeLine(filter, limit, field, false);
                aggs = await access.GetAggregateAsync(pipeline);
            }
            //转化返回聚合形式
            var groups = aggs.ConvertAll(s => s.ToDictionary());
            var tokens = new Dictionary<string, long?>();
            foreach (var group in groups)
            {
                var id = @group[GroupId]?.ToString() ?? "null";
                //这里的处理是为保证读到对象数组中的内容进行聚合
                if (!(@group[GroupId] is string))
                {
                    if (@group[GroupId] != null)
                        id = BsonDocument.Create(@group[GroupId]).GetElement(1).Value.ToString();
                }
                var records = Convert.ToInt64(group[GroupCount].ToString());
                if (tokens.ContainsKey(id)) continue;
                tokens.Add(id, records);
            }
            return tokens;
        }
        /// <summary>
        /// 获得单个字段的聚合结果
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="limit"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, long?>> GetAggsAsync(IConditionalExpression filter, int limit, string field)
        {
            var filterJson = filter.ToExpressionJson();
            return await GetAggsAsync(filterJson, limit, field);
        }
        /// <summary>
        /// 获得单个字段的聚合结果
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="limit"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public Dictionary<string, long?> GetAggs(IConditionalExpression filter, int limit, string field)
        {
            var filterJson = filter.ToExpressionJson();
            return GetAggs(filterJson, limit, field);
        }
        /// <summary>
        /// 获得mongo聚合结果
        /// </summary>
        /// <param name="filter">查询过滤</param>
        /// <param name="limit">返回条数限制</param>
        /// <param name="field">聚合字段</param>
        /// <returns></returns>
        public Dictionary<string, long?> GetAggs(string filter, int limit, string field)
        {
            var access = new MongoAccessAuto();
            var pipeline = GetPipeLine(filter, limit, field);
            List<BsonDocument> aggs;
            try
            {
                aggs = access.GetAggregate(pipeline);
            }
            catch (Exception)
            {
                pipeline = GetPipeLine(filter, limit, field, false);
                aggs = access.GetAggregate(pipeline);
            }
            //转化返回聚合形式
            var groups = aggs.ConvertAll(s => s.ToDictionary());
            var tokens = new Dictionary<string, long?>();
            foreach (var group in groups)
            {
                var id = @group[GroupId]?.ToString() ?? "null";
                //这里的处理是为保证读到对象数组中的内容进行聚合
                if (!(@group[GroupId] is string))
                {
                    if (@group[GroupId] != null)
                        id = BsonDocument.Create(@group[GroupId]).GetElement(1).Value.ToString();
                }
                var records =Convert.ToInt64( group[GroupCount].ToString());
                tokens.Add(id, records);
            }
            return tokens;
        }


        /// <summary>
        /// BsonDocument转化为对象类型(适合扁平结构)
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        [Obsolete]
        public MetadataDefinition ToMetaDataDef(BsonDocument doc)
        {
            var dic = doc.ToDictionary<Dictionary<string, object>>();
            var metadataDef = new MetadataDefinition();
            foreach (var d in dic)
            {
                var field = d.Key.ToUpperInitial(); //转化为首字母大写
                if (d.Key == GroupId) continue;
                try
                {
                    var value = d.Value;
                    if (d.Key.ToLower() == "type")
                    {
                        value = Enum.Parse(typeof(TagType), value.ToString());
                    }
                    metadataDef.GetType().GetProperty(field).SetValue(metadataDef, value);
                }
                catch (Exception ex)
                {
                    ex.Throw(SearchExceptionCodes.DataConvertFaild, "数据转化失败");
                }
            }
            return metadataDef;
        }
        #endregion

        #region MongoDB查询参数构建
        /// <summary>
        /// 构建投影
        /// </summary>
        /// <param name="fields">字段名称及是否返回标志</param>
        /// <returns></returns>
        public ProjectionDefinition<BsonDocument> GetProjects(Dictionary<string, int> fields)
        {
            if (fields == null) return GetDefaultProjects();
            //默认不返回MongoDB的_id字段
            if (!fields.ContainsKey("_id"))
            {
                fields.Add("_id", 0);
            }
            var projects = new List<ProjectionDefinition<BsonDocument>>();
            foreach (var field in fields)
            {
                var projectfiled = new StringFieldDefinition<BsonDocument>(field.Key);
                var project = field.Value == 0 ? Builders<BsonDocument>.Projection.Exclude(projectfiled) : Builders<BsonDocument>.Projection.Include(projectfiled);
                projects.Add(project);
            }
            return Builders<BsonDocument>.Projection.Combine(projects);
        }

        /// <summary>
        /// 默认的字段投影，不返回mongodb自动生成的_id
        /// </summary>
        /// <returns></returns>
        public ProjectionDefinition<BsonDocument> GetDefaultProjects()
        {
            var fields = new Dictionary<string, int>() { { "_id", 0 } };
            return GetProjects(fields);
        }
        /// <summary>
        /// 构建过滤
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public FilterDefinition<BsonDocument> GetFilters(IConditionalExpression filter)
        {
            return new JsonFilterDefinition<BsonDocument>(filter.ToExpressionJson());
        }
        /// <summary>
        /// 构建过滤
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public FilterDefinition<BsonDocument> GetFilters(string filter)
        {
            return new JsonFilterDefinition<BsonDocument>(filter);
        }
        /// <summary>
        /// 构建过滤（正则过滤）
        /// </summary>
        /// <param name="field">匹配字段</param>
        /// <param name="values">匹配值（或的关系）</param>
        /// <returns></returns>
        public FilterDefinition<BsonDocument> GetRegexFilters(string field, params string[] values)
        {
            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Empty;
            if (values != null && values.Length > 0)
            {
                var filters = new List<FilterDefinition<BsonDocument>>();
                foreach (var tag in values)
                {
                    if (tag.IsContainRegex())
                    {
                        var filterRegex = Builders<BsonDocument>.Filter
                            .Regex(f => f[field], new BsonRegularExpression(new Regex(tag)));
                        filters.Add(filterRegex);
                    }
                    else
                    {
                        filters.Add(Builders<BsonDocument>.Filter.Eq(f => f[field], tag));
                    }

                }
                filter = Builders<BsonDocument>.Filter.Or(filters);
            }
            return filter;
        }

        /// <summary>
        /// 构建排序
        /// </summary>
        /// <param name="sortCollection">
        /// 排序条件（字段，方向（true:asc，false:desc）</param>
        /// <returns></returns>
        public SortDefinition<BsonDocument> GetSorts(Dictionary<string, int> sortCollection)
        {
            var sorts = new List<SortDefinition<BsonDocument>>();
            foreach (var sort in sortCollection)
            {
                var field = sort.Key;
                var direction = sort.Value;
                var sortdef = direction == -1
                    ? Builders<BsonDocument>.Sort.Descending(field)
                    : Builders<BsonDocument>.Sort.Ascending(field);
                sorts.Add(sortdef);
            }
            return Builders<BsonDocument>.Sort.Combine(sorts);
        }

        /// <summary>
        /// 构建聚合管道
        /// </summary>
        /// <param name="filterJson">过滤条件</param>
        /// <param name="limit">限制返回条数</param>
        /// <param name="field">聚合字段名称</param>
        /// <param name="isArray"></param>
        /// <returns></returns>
        public PipelineDefinition<BsonDocument, BsonDocument> GetPipeLine(string filterJson, int limit, string field, bool isArray = true)
        {
            string pipeline3 = $" {{$group: {{{GroupId}:'${field}', {GroupCount}: {{$sum: 1}}}}}}";
            string pipeline4 = $" {{$sort:{{{GroupCount}:-1}}}}";
            // pipelines.Add(pipeline5);
            var pipelines = new List<string> { pipeline3, pipeline4 };
            if (limit != 0)
            {
                string pipeline5 = $" {{$limit: {limit}}}";
                pipelines.Add(pipeline5);
            }
            if (!string.IsNullOrEmpty(filterJson))
            {
                var pipeline2 = $" {{$match:{filterJson}}}";
                pipelines.Insert(0, pipeline2);
            }
            if (isArray)
            {
                var pipeline1 = $" {{$unwind:'${field}'}}";
                pipelines.Insert(0, pipeline1);
            }
            IList<IPipelineStageDefinition> stages = new List<IPipelineStageDefinition>();
            foreach (var pipeline in pipelines)
            {
                var stage = new JsonPipelineStageDefinition<BsonDocument, BsonDocument>(pipeline);
                stages.Add(stage);
            }
            return new PipelineStagePipelineDefinition<BsonDocument, BsonDocument>(stages);
        }
        #endregion
    }
}
