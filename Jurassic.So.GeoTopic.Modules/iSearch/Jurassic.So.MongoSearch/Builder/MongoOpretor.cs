using Jurassic.So.Infrastructure;

namespace Jurassic.So.Search.Mongo
{
    /// <summary>
    /// Mongo运算符
    /// </summary>
    public enum MongoOpretor
    {
        #region 二元运算符
        /// <summary>等于</summary>
        [Remark("$eq")]
        Equal,
        /// <summary>不等于</summary>
        [Remark("$neq")]
        NotEqual,
        /// <summary>在值列表内或子查询</summary>
        [Remark("$all")]
        All,
        /// <summary>在值列表内或子查询</summary>
        [Remark("$in")]
        In,
        /// <summary>不在值列表内或子查询</summary>
        [Remark("$nin")]
        NotIn,
        /// <summary>不在值列表内或子查询</summary>
        [Remark("$exists")]
        Exists,
        /// <summary>小于查询值</summary>
        [Remark("$lt")]
        LessThan,
        /// <summary>小于等于查询值</summary>
        [Remark("$lte")]
        LessThanOrEqual,
        /// <summary>大于查询值</summary>
        [Remark("$gt")]
        GreaterThan,
        /// <summary>大于等于查询值</summary>
        [Remark("$gte")]
        GreaterThanOrEqual,
        #endregion

        #region 逻辑运算符
        /// <summary>满足所有组合条件</summary>  
        [Remark("$and")]
        And,
        /// <summary>满足任意组合条件</summary>  
        [Remark("$or")]
        Or,
        /// <summary>正则模糊匹配</summary>  
        [Remark("$regex")]
        Regex
        #endregion
    }
}
