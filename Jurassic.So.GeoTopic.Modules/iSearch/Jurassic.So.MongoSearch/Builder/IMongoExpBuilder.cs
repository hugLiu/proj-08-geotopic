using Jurassic.PKS.Service;

namespace Jurassic.So.Search.Mongo
{
    /// <summary>
    /// mongoo查询条件对应Json构建
    /// </summary>
    /// <typeparam name="T">表达式</typeparam>
    internal interface IMongoExpBuilder<in T> where T : IConditionalExpression
    {
        /// <summary>
        /// 获得指定表达式对应的Mongo查询条件Json格式
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        string GetJson(T exp);
    }
}
