namespace Jurassic.So.Search
{
    /// <summary>搜索服务异常代码</summary>
    public enum SearchExceptionCodes
    {
        /// <summary>参数不合法</summary>
        InvalidPagerParameter,
        /// <summary>数据转化失败</summary>
        DataConvertFaild,
        /// <summary>ES请求不合法 </summary>
        InvalidEsRequest
    }
}
