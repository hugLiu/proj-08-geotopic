using System.Threading.Tasks;
using Jurassic.PKS.Service;
using Jurassic.PKS.Service.Index;
using Jurassic.So.Infrastructure;
using Jurassic.So.Infrastructure.Util;

namespace Jurassic.So.Index.MockMongo
{
    /// <summary>API包装索引服务</summary>
    public class ApiWrappedIndexer : IIndexer
    {
        /// <summary>构造函数</summary>
        public ApiWrappedIndexer(IIndexerConfig config)
        {
            this.Config = config;
        }
        /// <summary>配置</summary>
        private IIndexerConfig Config { get; set; }
        /// <summary>插入/更新/删除索引信息</summary>
        public IndexResult SendIndex(IndexInfo indexInfo)
        {
            return SendIndexAsync(indexInfo).GetAwaiter().GetResult();
        }
        /// <summary>插入/更新/删除索引信息</summary>
        public async Task<IndexResult> SendIndexAsync(IndexInfo indexInfo)
        {
            var result = await WebRequestUtil.PostJson(this.Config.SendIndexUrl, indexInfo.ToJson());
            return result.JsonTo<IndexResult>();
        }
    }
}
