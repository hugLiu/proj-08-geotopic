using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Runtime.Serialization;

namespace Jurassic.So.Index
{
    /// <summary>索引操作</summary>
    [Serializable]
    [DataContract]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum IndexAction
    {
        /// <summary>保存</summary>
        [EnumMember(Value = "Save")]
        Save,
        /// <summary>更新</summary>
        [EnumMember(Value = "Update")]
        Update,
        /// <summary>删除</summary>
        [EnumMember(Value = "Delete")]
        Delete,
    }
}
