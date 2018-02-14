using Jurassic.PKS.Service;

namespace GTAPI.API
{
    public enum Category
    {
        /// <summary>
        /// 数据来源信息
        /// </summary>
        [Remark("source.*")]
        Source,
        /// <summary>
        /// DC属性（基础标签）
        /// </summary>
        [Remark("dc.*")]
        Dc,
        /// <summary>
        /// EP属性（专业标签）
        /// </summary>
        [Remark("ep.*")]
        Ep,
        /// <summary>
        /// 所有元数据
        /// </summary>
        [Remark("")]
        All
    }
}