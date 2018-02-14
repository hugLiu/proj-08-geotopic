using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.ServiceModels
{
    /// <summary>HTTP应答内容类型特性</summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class MimeTypeAttribute : Attribute
    {
        /// <summary>构造函数</summary>
        public MimeTypeAttribute(string value, bool isStreamOutput = false)
        {
            this.Value = value;
            this.IsStreamOutput = isStreamOutput;
        }
        /// <summary>值</summary>
        public string Value { get; private set; }
        /// <summary>是否是流输出</summary>
        public bool IsStreamOutput { get; private set; }
    }
}
