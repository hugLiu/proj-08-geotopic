using System.Collections.Generic;
using System.Linq;
using Jurassic.So.Infrastructure;
using Jurassic.So.Search;
using Jurassic.PKS.Service.Search;
using Jurassic.PKS.Service;

namespace Jurassic.So.Search.Mongo
{
    /// <summary>
    /// 列表对象支持字段通配
    /// </summary>
    internal class ListSupport : IFieldSupportWildCard<List<string>>
    {
        public List<string> SupportWildCard(List<string> list)
        {
            var targetList = new List<string>();
            foreach (var item in list)
            {
                targetList.AddRange(item.WildCardFields());
            }
            return targetList;
        }
    }

    /// <summary>
    /// 字典对象支持字段通配
    /// </summary>
    internal class DicSupport : IFieldSupportWildCard<Dictionary<string, int>>
    {
        public Dictionary<string, int> SupportWildCard(Dictionary<string, int> dics)
        {
            var target = new Dictionary<string, int>();
            foreach (var dic in dics)
            {
                var oldValue = dic.Value;
                foreach (var field in dic.Key.WildCardFields())
                {
                    if (target.ContainsKey(field)) continue;
                    target.Add(field, oldValue);
                }
            }
            return target;
        }
    }

    /// <summary>
    /// 聚合规则支持字段通配
    /// </summary>
    internal class GroupRuleSupport : IFieldSupportWildCard<GroupRule>
    {
        public GroupRule SupportWildCard(GroupRule groups)
        {
            var target = new GroupRule { Top = groups.Top };
            target.GFields.AddRange(groups.GFields.SupportWildCard());
            return target;
        }
    }

    /// <summary>
    /// 排序规则支持字段通配
    /// </summary>
    internal class SortRuleSupport : IFieldSupportWildCard<SortRuleCollection>
    {
        public SortRuleCollection SupportWildCard(SortRuleCollection sorts)
        {
            var target = new SortRuleCollection();
            foreach (var sort in sorts)
            {
                var oldsort = sort.Value;
                foreach (var field in sort.Key.WildCardFields())
                {
                    if (target.ContainsKey(field)) continue;
                    target.Add(field, oldsort);
                }
            }
            return target;
        }
    }

    /// <summary>
    /// 投影支持字段通配
    /// </summary>
    internal class ProjectsSupport : IFieldSupportWildCard<FieldCollection>
    {
        public FieldCollection SupportWildCard(FieldCollection fields)
        {
            var target = new FieldCollection();
            var dics = fields.ToDictionary(s => s.Key, s => s.Value).SupportWildCard();
            foreach (var dic in dics)
            {
                target.Add(dic.Key, dic.Value);
            }
            return target;
        }
    }

    /// <summary>
    /// 过滤条件支持字段通配
    /// </summary>
    internal class FilterSupport : IFieldSupportWildCard<IConditionalExpression>
    {
        public IConditionalExpression SupportWildCard(IConditionalExpression filter)
        {
            if (filter.IsCombine && filter.Children.Count > 0)
            {
                return CombineExpSupport(filter);
            }
            return BinaryExpSupport(filter);
        }

        private IConditionalExpression BinaryExpSupport(IConditionalExpression binaryExp)
        {
            var binaryFilter = binaryExp as BinaryConditionExpression;
            if (binaryFilter != null)
            {
                var binaryList = new List<BinaryConditionExpression>();
                foreach (var field in binaryFilter.Field.WildCardFields())
                {
                    var newBinary = new BinaryConditionExpression(field, binaryFilter.Oprator, binaryFilter.Value);
                    binaryList.Add(newBinary);
                }
                return ConditionExpressionBuilder.Or(binaryList);
            }
            return binaryExp;
        }

        private IConditionalExpression CombineExpSupport(IConditionalExpression combineExp)
        {
            var combineFilter = combineExp as ConditionCollectionExpression;
            if (combineFilter != null && combineFilter.Children.Count > 0)
            {
                ConditionCollectionExpression target = new ConditionCollectionExpression(combineFilter.LogicOp);
                target.Children.AddRange(combineFilter.Children);
                foreach (var child in combineFilter.Children)
                {
                    if (child.Children != null && child.Children.Count > 0)
                    {
                        SupportWildCard(child);
                    }
                    else
                    {
                        target.Children.Remove(child);
                        target.Children.Add(BinaryExpSupport(child));
                    }
                    return target;
                }
            }
            return BinaryExpSupport(combineExp);
        }
    }

    /// <summary>
    /// 查询条件支持字段通配
    /// </summary>
    internal class MatchConditionSupport : IFieldSupportWildCard<MatchCondition>
    {
        public MatchCondition SupportWildCard(MatchCondition query)
        {
            //修改filter直接传入 不做转化 就不能支持统配
            //if (query.Filter != null) query.Filter = new FilterSupport().SupportWildCard(query.Filter);
            if (query.SortRules != null) query.SortRules = new SortRuleSupport().SupportWildCard(query.SortRules);
            if (query.Fields != null) query.Fields = new ProjectsSupport().SupportWildCard(query.Fields);
            return query;
        }
    }
}

