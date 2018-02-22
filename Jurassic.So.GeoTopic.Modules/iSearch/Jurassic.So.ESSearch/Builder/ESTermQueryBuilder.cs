using System;
using System.Collections.Generic;
using Nest;
using Jurassic.PKS.Service;
using Jurassic.So.Infrastructure;

namespace Jurassic.So.Search.ES
{
    /// <summary>
    /// 二元条件表达式对应的ES查询
    /// </summary>
    public class EsTermQueryBuilder : IEsQueryBuilder<BinaryConditionExpression>
    {
        /// <summary>
        /// 构建查询条件容器
        /// </summary>
        /// <param name="exp">二元条件表达式</param>
        /// <returns></returns>
        public QueryContainer BuildQuery(BinaryConditionExpression exp)
        {
            var flag = exp.Field.IsContainRegex();
            var field = exp.Field.KeyWord();
            var value = exp.Value;
            var op = exp.Oprator;
            switch (op)
            {
                case BinaryOperator.Equal:
                    if (flag) return new QueryStringQuery() { Fields = exp.Field, Query = value.ToString() };
                    return new TermQuery { Field = field, Value = value };
                case BinaryOperator.NotEqual:
                    if (flag) return new BoolQuery { MustNot = new List<QueryContainer> { new QueryStringQuery() { Fields = exp.Field, Query = value.ToString() } } };
                    return new BoolQuery { MustNot = new List<QueryContainer> { new TermQuery { Field = field, Value = value } } };
                case BinaryOperator.In:
                case BinaryOperator.All:
                    return new TermsQuery { Field = field, Terms = value as IEnumerable<object>};
                case BinaryOperator.NotIn:
                    return new BoolQuery { MustNot = new List<QueryContainer> { new TermsQuery { Field = field, Terms = value as IEnumerable<object> } } };
                case BinaryOperator.Exists:
                    return new ExistsQuery { Field = field };
                case BinaryOperator.GreaterThan:
                case BinaryOperator.LessThan:
                case BinaryOperator.GreaterThanOrEqual:
                case BinaryOperator.LessThanOrEqual:
                    return RangeQuery(exp.Field, value, op);
                default: return null;
            }
        }

        /// <summary>
        /// 获得范围查询的查询容器
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="value">范围值</param>
        /// <param name="op">操作符</param>
        /// <returns></returns>
        private static QueryContainer RangeQuery(string field, object value, BinaryOperator op)
        {
            var strValue = value.ToString();
            double numberValue;
            var isNumber = strValue.IsNumberic(out numberValue);
            DateTime dateValue;
            var isDate = strValue.IsDate(out dateValue);
            switch (op)
            {
                case BinaryOperator.GreaterThan:
                    if (isDate) return new DateRangeQuery { Field = field, GreaterThan = dateValue };
                    if (isNumber) return new NumericRangeQuery { Field = field, GreaterThan = numberValue };
                    return new TermRangeQuery { Field = field, GreaterThan = strValue };
                case BinaryOperator.LessThan:
                    if (isDate) return new DateRangeQuery { Field = field, LessThan = dateValue };
                    if (isNumber) return new NumericRangeQuery { Field = field, LessThan = numberValue };
                    return new TermRangeQuery { Field = field, LessThan = strValue };
                case BinaryOperator.GreaterThanOrEqual:
                    if (isDate) return new DateRangeQuery { Field = field, GreaterThanOrEqualTo = dateValue };
                    if (isNumber) return new NumericRangeQuery { Field = field, GreaterThanOrEqualTo = numberValue };
                    return new TermRangeQuery { Field = field, GreaterThanOrEqualTo = strValue };
                case BinaryOperator.LessThanOrEqual:
                    if (isDate) return new DateRangeQuery { Field = field, LessThanOrEqualTo = dateValue };
                    if (isNumber) return new NumericRangeQuery { Field = field, LessThanOrEqualTo = numberValue };
                    return new TermRangeQuery { Field = field, LessThanOrEqualTo = strValue };
                default:
                    return null;
            }
        }
    }
}
