using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Jurassic.So.Search
{
    /// <summary>
    /// 排序规则集合
    /// </summary>
    [Serializable]
    [DataContract]
    public class SortRuleCollection : Dictionary<string, Sort>
    {
    }
    /// <summary>
    /// 单个排序规则
    /// </summary>
    [DataContract]
    public class Sort
    {
        /// <summary>
        /// 无参构造
        /// </summary>
        public Sort() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="direction"></param>
        public Sort(int direction)
        {
            this.Direction = direction;
            this.Promotion = new List<string>();
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="promotion"></param>
        public Sort(int direction, List<string> promotion)
        {
            this.Direction = direction;
            this.Promotion = promotion;
        }
        /// <summary>
        /// 排序方向
        /// </summary>
        [DataMember(Name = "direction")]
        public int Direction { get; set; }
        /// <summary>
        /// 排序自定义值
        /// </summary>
        [DataMember(Name = "promotion")]
        public List<string> Promotion { get; set; }
    }
}
