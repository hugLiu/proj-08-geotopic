using Jurassic.PKS.Service;

namespace Jurassic.So.Search.Mongo
{
    /// <summary>
    /// 二元表达式对应Mongo查询条件
    /// </summary>
    internal class MongoBinaryExpBuilder : IMongoExpBuilder<BinaryConditionExpression>
    {
        /// <summary>
        /// 获得简单查询条件对应Json格式
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public virtual string GetJson(BinaryConditionExpression exp)
        {
            var op = exp.Oprator.GetMongoName();
            var value = exp.Value.ToJson();
            if (op == "$lt" || op == "$gt" || op == "$lte" || op == "$gte")
                value = $"ISODate({value})";
            return $"{{ \"{exp.Field}\": {{ {op}:{value} }}}}";
        }
    }
}
