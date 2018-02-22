using System.Linq;
using Jurassic.So.Infrastructure;
using Newtonsoft.Json.Linq;
using Jurassic.PKS.Service;

namespace Jurassic.So.Search.Mongo
{
    /// <summary>
    /// 解析mongo的查询条件的Json对象对应的表达式
    /// </summary>
    public static class MongoAnalyzeJson
    {
        /// <summary>
        /// 解析mongo对应的条件JObject
        /// </summary>
        /// <param name="exp">条件表达式</param>
        /// <param name="single">JObject对象</param>
        /// <returns></returns>
        public static IConditionalExpression AnalyzeJObject(this IConditionalExpression exp, JObject single)
        {
            var child = single[MongoOpretor.And.GetRemark()] ?? single[MongoOpretor.Or.GetRemark()];
            if (child != null && child.Any())
            {
                var parent = child.Path;
                return exp.AnalyzeJArray(JArray.Parse(child.ToString()), parent);
            }
            var obj = single.First;
            while (obj != null)
            {
                var field = single.Properties().First().Name;
                var op = BinaryOperator.Equal;
                var value = obj.First;
                if (obj.First.HasValues)
                {
                    var ss = JObject.Parse(obj.First.ToString());
                    op = ss.Properties().First().Name.ToOp();
                    value = ss.Properties().First().Value;
                }
                var conditionExp= new BinaryConditionExpression(field, op, value);
                if (exp == null)
                {
                    exp = conditionExp;
                    // exp = new ConditionCollectionExpression(LogicOperator.And);
                }
                else
                {
                    exp.Children.Add(conditionExp);
                }
                obj = obj.Next;
            }
            return exp;
        }

        /// <summary>
        /// 解析mongo对应的查询组合条件对应的JArray对象
        /// </summary>
        /// <param name="exp">条件表达式</param>
        /// <param name="array">组合条件JArray对象</param>
        /// <param name="parent">组合条件的逻辑运算符</param>
        /// <returns></returns>
        private static IConditionalExpression AnalyzeJArray(this IConditionalExpression exp, JArray array, string parent)
        {
            var op = parent.ToLogicOp();
            var isSingle = true;
            if (exp == null)
                exp = new ConditionCollectionExpression(op);
            else
            {
                exp.Children.Add(new ConditionCollectionExpression(op));
                isSingle = false;
            }
            foreach (var a in array)
            {
                var ob = JObject.Parse(a.ToString());
                if (isSingle)
                    exp.AnalyzeJObject(ob);
                else
                    exp.Children.Last().AnalyzeJObject(ob);
            }
            return exp;
        }
    }
}
