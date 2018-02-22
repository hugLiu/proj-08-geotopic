using System;
using System.Runtime.Serialization;
using Jurassic.So.Infrastructure;

namespace Jurassic.So.Search
{
    /// <summary>
    /// Mongo查询匹配条件
    /// </summary>
    [Serializable]
    [DataContract]
    public class MatchCondtion : QueryConditionBase
    {
        /// <summary>
        /// 过滤条件(自定义条件表达式类型)
        /// </summary>
        [DataMember(Name = "filter")]
        public IConditionalExpression Filter { get; set; }
    }

    /// <summary>
    /// Mongo查询匹配条件 （for api）
    /// </summary>
    [Serializable]
    [DataContract]
    public class MatchRequest : QueryConditionBase
    {
        /// <summary>
        /// 过滤条件(object类型)
        /// </summary>
        [DataMember(Name = "filter")]
        public object Filter { get; set; }
    }
}
