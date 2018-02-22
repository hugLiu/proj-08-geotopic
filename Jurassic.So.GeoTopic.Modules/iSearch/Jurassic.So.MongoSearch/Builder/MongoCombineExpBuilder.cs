using System.Text;
using Jurassic.PKS.Service;

namespace Jurassic.So.Search.Mongo
{
    /// <summary>
    /// 组合表达式对应Mongo查询条件
    /// </summary>
    internal class MongoCombineExpBuilder : IMongoExpBuilder<ConditionCollectionExpression>
    {
        /// <summary>
        /// 获得复杂查询条件对应Json格式
        /// </summary>
        /// <param name="exp">组合表达式</param>
        /// <returns></returns>
        public virtual string GetJson(ConditionCollectionExpression exp)
        {
            var sBuilder = new StringBuilder();
            var operatorName = exp.LogicOp.GetMongoName();
            sBuilder.AppendFormat("{{{0}:[", operatorName);
            var count = exp.Children.Count;
            if (count == 0)
                return "";
            sBuilder.Append(exp[0].ToMongoJson());
            if (count > 1)
            {
                for (int i = 1; i < count; i++)
                {
                    sBuilder.Append(",");
                    sBuilder.Append(exp[i].ToMongoJson());
                }
            }
            sBuilder.AppendFormat("]}}");
            return sBuilder.ToString();
        }
    }
}
