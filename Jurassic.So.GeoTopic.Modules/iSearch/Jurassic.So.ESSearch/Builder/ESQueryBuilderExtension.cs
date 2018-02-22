using System;
using System.Collections.Generic;
using System.Linq;
using Nest;
using Jurassic.PKS.Service;

namespace Jurassic.So.Search.ES
{
    /// <summary>
    /// ES查询条件容器构建扩展
    /// </summary>
    public static class EsQueryBuilderExtension
    {
        private static Dictionary<Type, object> Mappers { get; set; }
        static EsQueryBuilderExtension()
        {
            Mappers = new Dictionary<Type, object>();
            AddBuilder(new EsTermQueryBuilder());
            AddBuilder(new EsBoolQueryBuilder());
        }

        private static void AddBuilder<TExp>(IEsQueryBuilder<TExp> builder)
            where TExp : IConditionalExpression
        {
            var baseType = builder.GetType().GetInterface(typeof(IEsQueryBuilder<>).Name);
            var expType = baseType?.GenericTypeArguments.First();
            if (expType != null) Mappers[expType] = builder;
        }

        /// <summary>
        /// 自定义条件表达式转化为查询条件容器
        /// </summary>
        /// <param name="exp">自定义条件表达式</param>
        /// <returns>返回查询条件容器</returns>
        public static QueryContainer ToEsQuery(this IConditionalExpression exp)
        {
            if (exp != null)
            {
                dynamic builder = Mappers[exp.GetType()];
                dynamic exp2 = exp;
                return builder.BuildQuery(exp2);
            }
            return null;
        }


        /// <summary>
        /// 获得字段的多字段（keyword 不分词字段）名称
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static string KeyWord(this string field)
        {
            return field.Contains("keyword") ? field : field + ".keyword";
        }
    }
}
