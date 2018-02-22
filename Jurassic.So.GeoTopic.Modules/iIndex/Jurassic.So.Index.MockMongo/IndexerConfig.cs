using System.Web;
using Jurassic.So.Infrastructure;
using System.Configuration;

namespace Jurassic.So.Index.MockMongo
{
    /// <summary>索引服务配置</summary>
    public class IndexerConfig : IIndexerConfig
    {
        /// <summary>构造函数</summary>
        public IndexerConfig()
        {
            var context = HttpContext.Current;
            if (context == null) return;
            var domainUrl = context.Request.Url.GetDomainUrl();
            Set(domainUrl);
        }
        /// <summary>设置</summary>
        public void Set(string domainUrl)
        {
            domainUrl = domainUrl.Trim().TrimEnd('/');
            var settings = ConfigurationManager.AppSettings["IndexerConfig"];
            var values = settings.Split(',');
            var path = values[0].Trim().Trim('/');
            var action = values[1].Trim().Trim('/');
            this.SendIndexUrl = $"{domainUrl}/{path}/{action}";
        }
        /// <summary>发送方法URL</summary>
        public string SendIndexUrl { get; private set; }
    }
}
