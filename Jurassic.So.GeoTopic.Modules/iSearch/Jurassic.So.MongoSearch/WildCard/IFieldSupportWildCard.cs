namespace Jurassic.So.Search.Mongo
{
    /// <summary>
    /// 查询字段支持通配符
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal interface IFieldSupportWildCard<T>
    {
        /// <summary>
        /// 转化对象T使之包含通配字段，并返回新的对象T
        /// </summary>
        /// <param name="obj">包含查询字段的对象</param>
        /// <returns></returns>
        T SupportWildCard(T obj);
    }
}
