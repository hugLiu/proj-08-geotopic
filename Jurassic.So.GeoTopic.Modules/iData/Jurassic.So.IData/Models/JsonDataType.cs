using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Jurassic.So.Data
{
    /// <summary>JSON数据类型</summary>
    [Serializable]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum JsonDataType
    {
        /// <summary>Boolean</summary>
        Boolean,
        /// <summary>数字</summary>
        Number,
        /// <summary>字符串</summary>
        String,
        /// <summary>日期</summary>
        Date,
    }
}
