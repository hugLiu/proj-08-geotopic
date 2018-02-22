using System.Collections.Generic;

namespace Jurassic.Sooil.IServiceBase
{
    public class OrderRule
    {
        /// <summary>
        /// 字段名
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 升序/降序
        /// </summary>
        public string Direction { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public List<string> Value { get; set; }
    }
}
