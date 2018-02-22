﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nest;
using Jurassic.PKS.Service.Search;
using Jurassic.PKS.Service;

namespace Jurassic.So.Search.ES
{
    /// <summary>
    /// ES搜索Provider
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SearchProvider<T> where T : class
    {
        /// <summary>
        /// 根据过滤条件构建查询
        /// </summary>
        /// <param name="filterExp">过滤条件表达式</param>
        /// <returns></returns>
        public QueryContainer BuildFilterQuery(IConditionalExpression filterExp)
        {
            return filterExp?.ToEsQuery();
        }

        /// <summary>
        /// 根据输入短语构建全文查询
        /// </summary>
        /// <param name="sentence"></param>
        /// <param name="ranks"></param>
        /// <returns></returns>
        public QueryContainer BuildFullTextQuery(string sentence, RankCollection ranks)
        {
            if (string.IsNullOrEmpty(sentence)) return null;
            //var fields = new List<string> { "source.*^1", "ep.*^1", "fulltext^1" };

            //The query_string and simple_query_string queries query the _all field by default
            //unless another field is specified:
            return ranks != null && ranks.Count > 0
                ? new QueryStringQuery { Query = sentence, Fields = GetBoostFields(ranks).ToArray(), UseDisMax = false }
                : new QueryStringQuery { Query = sentence, UseDisMax = false };
        }

        /// <summary>
        /// 创建自定义得分的查询
        /// </summary>
        /// <param name="sentence">输入短语</param>
        /// <param name="ranks">排序规则</param>
        /// <returns></returns>
        public QueryContainer BuildCustomScoreQuery(string sentence, RankCollection ranks)
        {
            if (string.IsNullOrEmpty(sentence)) return null;
            if (ranks != null && ranks.Count > 0)
                return new FunctionScoreQuery
                {
                    Functions = BuildScoreFunctions(sentence, ranks),
                    MinScore = ranks.Select(s => s.Value).Min()
                };
            return new QueryStringQuery { Query = sentence, UseDisMax = false };
        }

        /// <summary>
        /// 构建得分函数
        /// </summary>
        /// <param name="sentence">输入短语</param>
        /// <param name="ranks">排序规则</param>
        /// <returns>返回一个或多个得分函数定义</returns>
        private static IEnumerable<IScoreFunction> BuildScoreFunctions(string sentence, RankCollection ranks)
        {
            var scoreFunctions = new List<IScoreFunction>();
            foreach (var rank in ranks)
            {
                var scoreFunction = new WeightFunction()
                {
                    Filter = new MatchQuery() { Field = rank.Key, Query = sentence },
                    Weight = rank.Value
                };
                scoreFunctions.Add(scoreFunction);
            }
            return scoreFunctions;
        }

        /// <summary>
        /// 获得带权重的字段
        /// </summary>
        /// <param name="ranks">字段权重</param>
        /// <returns></returns>
        private static List<string> GetBoostFields(RankCollection ranks)
        {
            var boostFields = new List<string>();
            foreach (var rank in ranks)
            {
                var field = rank.Key + "^" + rank.Value;
                boostFields.Add(field);
            }
            return boostFields;
        }

        /// <summary>
        /// 合并多个查询为Should组合查询
        /// </summary>
        /// <param name="querys">多个查询条件</param>
        /// <returns></returns>
        public QueryContainer CombineShouldQuery(params QueryContainer[] querys)
        {
            var count = querys.Count(s => s != null);
            return count > 1
               ? new BoolQuery() { Should = querys } : count == 1
               ? querys.First(w => w != null)
               : null;
        }

        /// <summary>
        /// 合并多个查询为Must组合查询
        /// </summary>
        /// <param name="querys">多个查询条件</param>
        /// <returns></returns>
        public QueryContainer CombineMustQuery(params QueryContainer[] querys)
        {
            var count = querys.Count(s => s != null);
            return count > 1
               ? new BoolQuery() { Must = querys } : count == 1
               ? querys.First(w => w != null)
               : null;
        }


        /// <summary>
        /// 合并多个查询为MustNot组合查询
        /// </summary>
        /// <param name="querys">多个查询条件</param>
        /// <returns></returns>
        public QueryContainer CombineMustNotQuery(params QueryContainer[] querys)
        {
            var count = querys.Count(s => s != null);
            return count > 1
                ? new BoolQuery() { MustNot = querys } : count == 1
                ? querys.First(w => w != null)
                : null;
        }

        /// <summary>
        /// 根据排序规则构建排序器
        /// </summary>
        /// <param name="sorts">排序规则</param>
        /// <returns></returns>
        public SortDescriptor<T> BuildSort(SortRuleCollection sorts)
        {
            if (sorts == null || sorts.Count == 0) return null;

            var sortDesc = new SortDescriptor<T>();
            foreach (var s in sorts)
            {
                var order = s.Value.Direction == 1 ? SortOrder.Ascending : SortOrder.Descending;
                if (s.Value.Promotion == null || s.Value.Promotion.Count == 0)
                {
                    var sortField = new SortField() { Field = s.Key, Order = order };
                    sortDesc.Field(f => sortField);
                }
                else
                {
                    var scriptField = new ScriptSort { Script = new InlineScript(GetOrderScript(s.Value, s.Key)), Order = order, Type = "number" };
                    sortDesc.Script(sc => scriptField);
                }
            }
            return sortDesc;
        }

        /// <summary>
        /// 获得排序脚本
        /// </summary>
        /// <param name="sort">排序规则</param>
        /// <param name="field">排序字段</param>
        /// <returns></returns>
        private static string GetOrderScript(Sort sort, string field)
        {
            var value = sort.Promotion;
            var sb = new StringBuilder();
            for (int i = 0; i < value.Count + 1; i++)
            {
                string scriptText;
                string itemValue;
                if (i == 0)
                {
                    itemValue = value[i].Trim();
                    // scriptText = $"if('{itemValue}' == doc['{field.KeyWord()}'].value)return 0;";
                    scriptText = $"'{itemValue}'==doc['{field.KeyWord()}'].value?0";
                }
                else if (i == value.Count)
                {
                    // scriptText = $"else return {value.Count}";
                    scriptText = $":{i}";
                    for (var j = 0; j < value.Count - 1; j++)
                    {
                        scriptText += ")";
                    }
                }
                else
                {
                    itemValue = value[i];
                    //scriptText = $"else if('{itemValue}' == doc['{field.KeyWord()}'].value)return {i};";
                    scriptText = $":('{itemValue}'==doc['{field.KeyWord()}'].value?{i}";
                }
                sb.Append(scriptText);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 根据聚合条件构建聚合器
        /// </summary>
        /// <param name="groups">聚合条件</param>
        /// <returns></returns>
        public AggregationContainerDescriptor<T> BuildAggs(GroupRule groups)
        {
            if (groups == null || groups.GFields.Count == 0) return null;

            var aggsDesc = new AggregationContainerDescriptor<T>();
            foreach (var gfield in groups.GFields)
            {
                aggsDesc.Terms(gfield, s => s.Field(gfield.KeyWord()).Size(groups.Top).OrderDescending("_count"));
            }
            return aggsDesc;
        }

        /// <summary>
        /// 构建返回字段过滤器
        /// </summary>
        /// <param name="fields">字段投影</param>
        /// <returns></returns>
        public SourceFilterDescriptor<T> BuildFields(FieldCollection fields)
        {
            if (fields == null || fields.Count == 0) return null;

            var exFields = fields.Where(w => w.Value == 0).Select(s => s.Key);
            var inFields = fields.Where(w => w.Value == 1).Select(s => s.Key);
            var fieldDesc = new SourceFilterDescriptor<T>()
                .IncludeAll()
                .Excludes(ec => ec.Fields(exFields.ToArray()))
                .Includes(ic => ic.Fields(inFields.ToArray()));

            return fieldDesc;
        }
    }
}
