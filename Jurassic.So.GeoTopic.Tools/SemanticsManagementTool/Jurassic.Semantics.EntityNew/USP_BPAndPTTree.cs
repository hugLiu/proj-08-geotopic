﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.Semantics.Entity.EntityModel
{
    public class USP_BPAndPTTree
    {
        /// <summary>
        /// 叙词ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        ///  叙词
        /// </summary>
        public string Term { get; set; }
        /// <summary>
        /// 叙词父ID
        /// </summary>
        public Guid PId { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 排序索引
        /// </summary>
        public int OrderIndex { get; set; }
        /// <summary>
        /// 是否是PT
        /// </summary>
        public bool IsPT { get; set; }
        /// <summary>
        /// 关键字的个数
        /// </summary>
        public int kwCount { get; set; }


    }
}
