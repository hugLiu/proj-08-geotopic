using Jurassic.So.Data;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Jurassic.PKS.Service.Data;
using Jurassic.So.Infrastructure;

namespace Jurassic.So.Data.Center
{
    /// <summary>ADP协议地址</summary>
    /// <remarks>格式：ADP://adaptername/datatype/naturekey</remarks>
    public class ADPUrl
    {
        /// <summary>构造函数</summary>
        public ADPUrl()
        {
            this.Adapter = string.Empty;
            this.Scope = string.Empty;
            this.NatureKey = string.Empty;
            this.Url = string.Empty;
        }
        /// <summary>构造函数</summary>
        public ADPUrl(string url)
        {
            this.Url = url;
        }
        /// <summary>原Url</summary>
        public string Url { get; set; }
        /// <summary>适配器</summary>
        public string Adapter { get; set; }
        /// <summary>域</summary>
        public string Scope { get; set; }
        /// <summary>实体数据键</summary>
        public string NatureKey { get; set; }
        /// <summary>分析</summary>
        public static ADPUrl Analyze(string url)
        {
            //var pattern = @"ADP://(?<AdapterName>[^/]+)/(?<DataType>[^/]+)/(?<NatureKey>.+)";
            var pattern = @"ADP://(?<Adapter>[^/]+)/(?<Scope>[^/]+)/?(?<NatureKey>.*)";
            var match = Regex.Match(url, pattern, RegexOptions.IgnoreCase);
            if (!match.Success)
            {
                DataExceptionCode.InvalidUrlFormat.ThrowUserFriendly(
                    $"ADPUrl[{url}]不是有效的ADP协议地址！", "无效的URL格式！");
            }
            var adpUrl = new ADPUrl(url);
            adpUrl.Adapter = match.Groups["Adapter"].Value;
            adpUrl.Scope = match.Groups["Scope"].Value;
            adpUrl.NatureKey = match.Groups["NatureKey"].Value.TrimEnd('/');
            return adpUrl;
        }
        /// <summary>生成字符串</summary>
        private string Build()
        {
            return $"ADP://{this.Adapter}/{this.Scope}/{this.NatureKey}";
        }
        /// <summary>生成字符串</summary>
        public override string ToString()
        {
            return Build();
        }
    }
}
