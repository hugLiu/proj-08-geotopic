using Jurassic.So.Business;
using Jurassic.So.Infrastructure;
using Jurassic.So.Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.Data
{
    /// <summary>URL格式的数据结果</summary>
    [Serializable]
    [DataContract]
    public class UrlDataResult
    {
        /// <summary>Http动作</summary>
        [DataMember(Name = "httpverb")]
        public string HttpVerb { get; set; }
        /// <summary>链接地址</summary>
        [DataMember(Name = "url")]
        public string Url { get; set; }
        /// <summary>内容</summary>
        [DataMember(Name = "body")]
        public string Body { get; set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}
