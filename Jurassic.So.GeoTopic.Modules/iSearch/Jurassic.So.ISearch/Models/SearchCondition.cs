using System;
using System.Runtime.Serialization;
using Jurassic.So.Infrastructure;

namespace Jurassic.So.Search
{
    /// <summary>
    /// ES查询条件
    /// </summary>
    [Serializable]
    [DataContract]
    public class SearchCondition : MatchCondtion
    {
        /// <summary>
        /// 输入短语
        /// </summary>
        [DataMember(Name = "sentence")]
        public string Sentence { get; set; }
        /// <summary>
        /// 提升规则
        /// </summary>
        [DataMember(Name = "ranks")]
        public RankCollection Ranks { get; set; }
        /// <summary>
        /// 聚合条件
        /// </summary>
        [DataMember(Name = "grouprule")]
        public GroupRule GroupRule { get; set; }
    }

    /// <summary>
    /// ES查询条件 （for api）
    /// </summary>
    [Serializable]
    [DataContract]
    public class SearchRequest : MatchRequest
    {
        /// <summary>
        /// 输入短语
        /// </summary>
        [DataMember(Name = "sentence")]
        public string Sentence { get; set; }
        /// <summary>
        /// 提升规则
        /// </summary>
        [DataMember(Name = "ranks")]
        public RankCollection Ranks { get; set; }
        /// <summary>
        /// 聚合条件
        /// </summary>
        [DataMember(Name = "grouprule")]
        public GroupRule GroupRule { get; set; }
    }
}
