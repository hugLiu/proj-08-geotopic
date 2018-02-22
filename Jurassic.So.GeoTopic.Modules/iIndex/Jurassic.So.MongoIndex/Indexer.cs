using System.Collections.Generic;
using System.Threading.Tasks;
using Jurassic.PKS.Service.Index;
using Jurassic.So.Infrastructure;

namespace Jurassic.So.Index.Mongo
{
    /// <summary>Mongo索引服务</summary>
    public class Indexer : IIndexer
    {
        private readonly IndexerProvider _provider = new IndexerProvider();
        /// <summary>批量保存/更新/删除索引信息</summary>
        /// <param name="indexInfo">索引操作信息</param>
        /// <returns>索引操作结果</returns>
        public IndexResult SendIndex(IndexInfo indexInfo)
        {
            return SendIndexAsync(indexInfo).GetAwaiter().GetResult();
        }
        /// <summary>批量保存/更新/删除索引信息</summary>
        /// <param name="indexInfo">索引操作信息</param>
        /// <returns>索引操作结果</returns>
        public async Task<IndexResult> SendIndexAsync(IndexInfo indexInfo)
        {
            IEnumerable<string> iiids = null;
            var action = indexInfo.Action;
            var docs = indexInfo.Metadatas;
            switch (action)
            {
                case IndexAction.Save:
                    iiids = await _provider.SaveAsync(docs);
                    break;
                case IndexAction.Update:
                    iiids = await _provider.UpdateAsync(docs);
                    break;
                case IndexAction.Delete:
                    iiids = await _provider.DeleteAsync(docs);
                    break;
                default:
                    ExceptionCodes.InvalidEnumValue.ThrowUserFriendly("无效的索引操作！", $"枚举值[{indexInfo.Action.ToString()}]无效！");
                    break;
            }
            var result = new IndexResult();
            if (iiids != null) result.IIIds.AddRange(iiids);
            return result;
        }

    }
}
