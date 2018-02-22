﻿
namespace Jurassic.Semantics.IService.ViewModel
{
    /// <summary>
    /// 与成果类型存在关系的语义关系
    /// </summary>
    public class PtRelations
    {
        /// <summary>
        /// 关系类型
        /// </summary>
        public string SR { get; set; }
        /// <summary>
        /// 对应的概念类
        /// </summary>
        public string Field { get; set; }

    }
}