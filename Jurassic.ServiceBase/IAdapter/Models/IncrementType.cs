using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Runtime.Serialization;

namespace Jurassic.Adapter
{
    /// <summary>增量类型</summary>
    [DataContract]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum IncrementType
    {
        /// <summary>无</summary>
        [EnumMember(Value = "None")]
        None,
        /// <summary>ID</summary>
        [EnumMember(Value = "ID")]
        ID,
        /// <summary>日期</summary>
        [EnumMember(Value = "Date")]
        Date
    }
}
