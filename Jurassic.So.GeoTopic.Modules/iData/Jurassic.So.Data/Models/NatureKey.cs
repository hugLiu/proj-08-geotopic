using Jurassic.So.Data;
using Jurassic.So.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Jurassic.So.Data.Center
{
    /// <summary>实体数据键</summary>
    /// <remarks>格式：key=value&key1=value1</remarks>
    public class NatureKey
    {
        /// <summary>构造函数</summary>
        public NatureKey() : this(string.Empty) { }
        /// <summary>构造函数</summary>
        public NatureKey(string value)
        {
            this.Dict = new Dictionary<string, string>();
            if (!value.IsNullOrEmpty()) Analyze(value);
        }
        /// <summary>构造函数</summary>
        public NatureKey(Dictionary<string, string> dict)
        {
            this.Dict = dict ?? new Dictionary<string, string>(); 
        }
        /// <summary>数据字典</summary>
        public Dictionary<string, string> Dict { get; private set; }
        /// <summary>分析</summary>
        private void Analyze(string value)
        {
            var query = value.Base64Utf8Decode();
            var pairs = query.Split('&');
            foreach (var pair in pairs)
            {
                var pair2 = pair.Split('=');
                var key = HttpUtility.UrlDecode(pair2[0]);
                if (pair2.Length == 1)
                {
                    this.Dict.Add(key, string.Empty);
                }
                else
                {
                    var kvalue = HttpUtility.UrlDecode(pair2[1]);
                    this.Dict.Add(key, kvalue);
                }
            }
        }
        /// <summary>生成字符串</summary>
        private string Build(bool urlEncode)
        {
            var builder = new StringBuilder();
            foreach (var pair in this.Dict)
            {
                var key = pair.Key;
                var value = pair.Value;
                if (urlEncode)
                {
                    key = HttpUtility.UrlEncode(key);
                    value = HttpUtility.UrlEncode(value);
                }
                builder.AppendFormat("{0}={1}&", key, value);
            }
            builder.Length--;
            return builder.ToString();
        }
        /// <summary>生成Base64字符串</summary>
        public string BuildBase64()
        {
            var query = Build(true);
            return query.Base64Utf8Encode();
        }
        /// <summary>生成字符串</summary>
        public override string ToString()
        {
            return Build(false);
        }
    }
}
