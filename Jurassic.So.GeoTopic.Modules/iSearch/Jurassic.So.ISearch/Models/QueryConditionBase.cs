using System;
using System.Runtime.Serialization;
using Jurassic.So.Infrastructure.Model;
using Jurassic.ServiceModels;

namespace Jurassic.So.Search
{
    /// <summary>
    /// 查询条件基础
    /// </summary>
    [Serializable]
    [DataContract]
    public class QueryConditionBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public QueryConditionBase()
        {
            this.Fields = new FieldCollection();
            this.SortRules = new SortRuleCollection();
            this.Pager = new Pager();
        }
        /// <summary>
        /// 字段投影
        /// </summary>
        [DataMember(Name = "fields")]
        public FieldCollection Fields { get; set; }
        /// <summary>
        /// 排序规则
        /// </summary>
        [DataMember(Name = "sortrules")]
        public SortRuleCollection SortRules { get; set; }
        /// <summary>
        ///分页参数
        /// </summary>
        [DataMember(Name = "pager", IsRequired = true)]
        public Pager Pager { get; set; }
    }
}
