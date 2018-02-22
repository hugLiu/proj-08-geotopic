using System.Collections.Generic;
using Nest;
using Jurassic.PKS.Service;

namespace Jurassic.So.Search.ES
{
    /// <summary>
    /// 组合表达式对应的ES查询
    /// </summary>
    public class EsBoolQueryBuilder : IEsQueryBuilder<ConditionCollectionExpression>
    {
        /// <summary>
        /// 构建查询条件容器
        /// </summary>
        /// <param name="exp">组合条件表达式</param>
        /// <returns></returns>
        public QueryContainer BuildQuery(ConditionCollectionExpression exp)
        {
            var childs = exp.Children;
            var queryContainerList = new List<QueryContainer>();
            if (childs.Count > 0)
            {
                foreach (var child in childs)
                {
                    queryContainerList.Add(child.ToEsQuery());
                }
            }
            switch (exp.LogicOp)
            {
                case LogicOperator.And:
                    return new BoolQuery { Must = queryContainerList };
                case LogicOperator.Or:
                    return new BoolQuery { Should = queryContainerList };
                default:
                    return null;
            }
        }
    }
}
