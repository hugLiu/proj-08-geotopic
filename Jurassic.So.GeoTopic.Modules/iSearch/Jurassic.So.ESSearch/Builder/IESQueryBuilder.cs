using Jurassic.PKS.Service;
using Nest;

namespace Jurassic.So.Search.ES
{
    /// <summary>
    /// ES查询条件构建
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEsQueryBuilder<in T> where T : IConditionalExpression
    {
        /// <summary>
        /// 构建查询条件
        /// </summary>
        /// <param name="exp">自定义条件表达式</param>
        /// <returns>查询条件</returns>
        QueryContainer BuildQuery(T exp);
    }
}
