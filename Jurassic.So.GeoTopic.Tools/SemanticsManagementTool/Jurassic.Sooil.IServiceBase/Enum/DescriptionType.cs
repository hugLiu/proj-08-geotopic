using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.Sooil.IServiceBase
{
    [Serializable]
    public class DescriptionType
    {
        /// <summary>
        /// 摘要
        /// </summary>
        public const string Abstract = "Abstract";
        /// <summary>
        /// 目录
        /// </summary>
        public const string Catalogue = "Catalogue";
        /// <summary>
        /// 引言
        /// </summary>
        public const string Introduction = "Introduction";
        /// <summary>
        /// 结束语
        /// </summary>
        public const string Summary = "Summary";
    }
}
