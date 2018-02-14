using Jurassic.PKS.Service;
using Jurassic.So.Business;
using Jurassic.So.Infrastructure;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using TDoc = Jurassic.So.MongoDB.MongoMetadata;
using TDoc = MongoDB.Bson.BsonDocument;

namespace Jurassic.So.MongoDB
{
    /// <summary>Mongo元数据数据访问</summary>
    public class MongoMetadataRepository : IMetadataRepository
    {
        /// <summary>构造函数</summary>
        public MongoMetadataRepository()
        {
            this.Accessor = new MongoAccess<TDoc>(MongoConsts.Collection_MetaData);
        }
        /// <summary>数据访问器</summary>
        private MongoAccess<TDoc> Accessor { get; set; }
        /// <summary>获得一条元数据</summary>
        public TDoc GetOne(string url)
        {
            var filter = Builders<TDoc>.Filter.Eq(f => f[MetadataConsts.SourceUrl], url);
            return this.Accessor.GetOne(filter);
        }
        /// <summary>保存</summary>
        /// <param name="metadatas">元数据集合</param>
        /// <returns>返回受影响的元数据IIId集合</returns>
        public async Task<IEnumerable<string>> SaveAsync(MetadataCollection metadatas)
        {
            var docs = metadatas.Select(e => new MongoMetadata(e.As<IDictionary<string, object>>()));
            Func<TDoc, FilterDefinition<TDoc>> filterBuilder = doc => Builders<TDoc>.Filter.Eq(f => f[MetadataConsts.SourceUrl], doc[MetadataConsts.SourceUrl]);
            var updateOptions = new UpdateOptions() { IsUpsert = true };
            await this.Accessor.ReplaceManyAsync(docs, filterBuilder, updateOptions);
            var result = new List<string>();
            result.AddRange(metadatas.Select(e => e.IIId));
            return result;
        }
        /// <summary>更新</summary>
        /// <param name="metadatas">元数据集合</param>
        /// <returns>返回受影响的元数据IIId集合</returns>
        public async Task<IEnumerable<string>> UpdateAsync(MetadataCollection metadatas)
        {
            var result = new List<string>();
            await Task.Delay(1);
            return result;
        }
        /// <summary>删除</summary>
        /// <param name="metadatas">元数据集合</param>
        /// <returns>返回受影响的元数据IIId集合</returns>
        public async Task<IEnumerable<string>> DeleteAsync(MetadataCollection metadatas)
        {
            var urlFilter = Builders<TDoc>.Filter.In(f => f[MetadataConsts.SourceUrl], metadatas.Select(e => e.Url.As<object>()));
            var urlProjection = Builders<TDoc>.Projection.Include(f => f[MetadataConsts.SourceUrl]);
            var iiidProjection = Builders<TDoc>.Projection.Include(f => f[MetadataConsts.IIId]);
            var projections = Builders<TDoc>.Projection.Combine(urlProjection, iiidProjection);
            var docs = await this.Accessor.GetManyAsync(urlFilter, null, projections);
            var result = new List<string>();
            foreach (var metadata in metadatas)
            {
                var doc = docs.FirstOrDefault(e => e[MetadataConsts.SourceUrl] == metadata.Url);
                if (doc == null)
                {
                    MetadataExceptionCodes.UrlNotExist.ThrowUserFriendly("删除元数据失败！", "元数据URL不存在！");
                }
                result.Add(doc[MetadataConsts.IIId].As<string>());
            }
            await this.Accessor.DeleteAsync(urlFilter);
            return result;
        }
    }
}
