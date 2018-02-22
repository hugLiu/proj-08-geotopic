using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Jurassic.Sooil.IServiceBase
{
    public class MetaData
    {
        public MetaData() { }

        public MetaData(string tag)
        {
            Tag = tag;
        }
        /// <summary>
        /// 元素名称
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// 叙词
        /// </summary>
        public virtual string Value { get; set; }
        /// <summary>
        /// 元数据类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 得分
        /// </summary>
        public double Score { get; set; }

        public override bool Equals(object obj)
        {
            var metaData = obj as MetaData;
            if (metaData == null) return false;

            return (Tag == metaData.Tag
                && (Value ?? "").ToLower() == (metaData.Value ?? "").ToLower()
                && (Type ?? "").ToLower() == (metaData.Type ?? "").ToLower());
        }
    }
}
