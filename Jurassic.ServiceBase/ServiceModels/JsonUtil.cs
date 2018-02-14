using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Jurassic.ServiceModels
{
    /// <summary>JSON工具</summary>
    public static class JsonUtil
    {
        /// <summary>生成对象JSON串</summary>
        public static string ToJsonString(this object value, JsonSerializerSettings settings = null)
        {
            return JsonConvert.SerializeObject(value, Formatting.Indented, settings);
        }
    }
}
