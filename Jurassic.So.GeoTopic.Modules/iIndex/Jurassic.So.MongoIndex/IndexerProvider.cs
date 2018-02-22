using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jurassic.PKS.Service;
using Jurassic.PKS.Service.Index;
using Jurassic.So.Business;
using Jurassic.So.Infrastructure;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;

namespace Jurassic.So.Index.Mongo
{
    /// <summary>
    /// 索引服务Provider
    /// </summary>
    public class IndexerProvider
    {
        /// <summary>Mongo索引数据访问服务</summary>
        private readonly MongoAccessAuto _access = new MongoAccessAuto();
        private const string Url = MetadataConsts.SourceUrl;
        private const string IiId = MetadataConsts.IIId;
        private const string MongoIiId = "iiid";
        private const string IndexedxDate = "indexeddate";
        #region KMD 实现索引服务【保存 更新 删除】
        /// <summary>
        /// 保存索引
        /// </summary>
        /// <param name="metadatas">元数据集合</param>
        /// <returns></returns>
        public async Task<List<string>> SaveAsync(MetadataCollection metadatas)
        {
            var docs = MetadataColToBsonDocList(metadatas);     //转化为bsondocument集合 
            await SaveAsync(docs);
            var result = docs.Select(s => s[MongoIiId].ToString()).ToList();
            return result;
        }
        /// <summary>
        /// 更新索引
        /// </summary>
        /// <param name="metadatas">元数据集合</param>
        /// <returns></returns>
        public async Task<List<string>> UpdateAsync(MetadataCollection metadatas)
        {
            var docs = MetadataColToBsonDocList(metadatas);     //转化为bsondocument集合 
            var mergedKmds = new List<KMD>();
            //var mergeDocs = new List<BsonDocument>();
            try
            {
                foreach (var doc in docs)
                {
                    var currentKmd = new KMD(doc.ToDictionary().ToJsonString());
                    var filter = Builders<BsonDocument>.Filter.Eq(f => f[MongoIiId], doc[MongoIiId]);
                    var oldDoc = await _access.GetOneAsync(filter, Builders<BsonDocument>.Projection.Exclude(ex => ex["_id"]));
                    if (oldDoc == null)
                    {
                        mergedKmds.Add(currentKmd);
                        //mergeDocs.Add(doc);
                    }
                    else
                    {
                        var oldKmd = new KMD(oldDoc.ToDictionary().ToJsonString());
                        var newKmd = oldKmd.Merge(currentKmd);
                        mergedKmds.Add(newKmd);
                        // mergeDocs.Add(doc.Merge(oldDoc,true));
                    }
                }
                //await SaveAsync(mergeDocs);
                await SaveAsync(KmdsToBsonDocList(mergedKmds));
            }
            catch (Exception ex)
            {
                IndexExceptionCodes.UpdatingIndexFailed.ThrowUserFriendly(ex.Message, "更新索引失败");
            }
            var result = docs.Select(s => s[MongoIiId].ToString()).ToList();
            return result;
        }

        /// <summary>
        /// 删除索引
        /// </summary>
        /// <param name="metadatas">元数据集合</param>
        /// <returns></returns>
        public async Task<List<string>> DeleteAsync(MetadataCollection metadatas)
        {
            var docs = MetadataColToBsonDocList(metadatas);     //转化为bsondocument集合 
            await DeleteAsync(docs);
            var result = docs.Select(s => s[MongoIiId].ToString()).ToList();
            return result;
        }
        #endregion

        #region Metadata 实现索引服务【保存 更新 删除】

        /// <summary>
        /// Dictionary转化为BsonDocument
        /// </summary>
        /// <param name="metadata">基类为dictionary的元数据</param>
        /// <returns></returns>
        private BsonDocument DicConvertToBsonDoc<T>(T metadata) where T : Dictionary<string, object>
        {
            if (!metadata.ContainsKey(IndexedxDate))
                metadata.Add(IndexedxDate, DateTime.Now.ToISODateString());
            else if (metadata[IndexedxDate].ToString().IsNullOrEmpty())
                metadata[IndexedxDate] = DateTime.Now.ToISODateString();

            var url = JObject.Parse(metadata.ToString()).SelectToken("$." + Url);
            if (url != null)
            {
                metadata.Add(MongoIiId, url.ToString().ToMD5());
                if (!metadata.ContainsKey(MongoIiId))
                {
                    metadata.Add(MongoIiId, url.ToString().ToMD5());
                }
                else if (metadata[MongoIiId].ToString().IsNullOrEmpty())
                {
                    metadata[MongoIiId] = url.ToString().ToMD5();
                }
            }
            else
                MetadataExceptionCodes.UrlNotExist.ThrowUserFriendly($"{Url}键值不存在！", $"{Url}为必要键，请查看索引数据是否包含！");

            var bsonDoc = new BsonDocument();
            return bsonDoc.AddRange(metadata); ;
        }
        /// <summary>保存</summary>
        public async Task<List<string>> Save2Async(MetadataCollection metadatas)
        {
            var metas = metadatas.Select(e => e.As<Metadata>()).ToList();
            var docs = metas.ConvertAll(DicConvertToBsonDoc);
            var result = new List<string>();
            await SaveAsync(docs);
            result = metadatas.Select(s => s.IIId).ToList();
            return result;
        }

        /// <summary>更新</summary>
        public async Task<IEnumerable<string>> Update2Async(MetadataCollection metadatas)
        {
            List<string> result = null;
            var kmds = metadatas.Select(e => e.As<Metadata>()).ToList();
            var docs = kmds.ConvertAll(DicConvertToBsonDoc);
            var updateOptions = new UpdateOptions { IsUpsert = true };
            try
            {
                foreach (var doc in docs)
                {
                    var filter = Builders<BsonDocument>.Filter.Eq(f => f[IiId], doc[IiId]);
                    var update = Builders<BsonDocument>.Update.Combine(BuildUpdateDefinition(doc, null));
                    await _access.UpdateAsync(filter, update, updateOptions);
                }
                result = metadatas.Select(s => s.IIId).ToList();
            }
            catch (Exception ex)
            {
                IndexExceptionCodes.UpdatingIndexFailed.ThrowUserFriendly(ex.Message, "更新索引失败");
            }
            return result;
        }

        /// <summary>
        /// 构建更新操作定义 
        /// </summary>
        /// <param name="bc">bsondocument文档</param>
        /// <returns></returns>
        private List<UpdateDefinition<BsonDocument>> BuildUpdateDefinition(BsonDocument bc, string parent)
        {
            var updates = new List<UpdateDefinition<BsonDocument>>();
            foreach (var element in bc.Elements)
            {
                var key = parent == null ? element.Name : $"{parent}.{element.Name}";
                var subUpdates = new List<UpdateDefinition<BsonDocument>>();
                //子元素是对象
                if (element.Value.IsBsonDocument)
                {
                    updates.AddRange(BuildUpdateDefinition(element.Value.ToBsonDocument(), key));
                }
                //子元素是对象数组
                else if (element.Value.IsBsonArray)
                {
                    var arrayDocs = element.Value.AsBsonArray;
                    var i = 0;
                    foreach (var doc in arrayDocs)
                    {
                        if (doc.IsBsonDocument)
                        {
                            updates.AddRange(BuildUpdateDefinition(doc.ToBsonDocument(), key + $".{i}"));
                        }
                        else
                        {
                            updates.Add(Builders<BsonDocument>.Update.Set(f => f[key], element.Value));
                            continue;
                        }
                        i++;
                    }
                }
                //子元素是其他
                else
                {
                    updates.Add(Builders<BsonDocument>.Update.Set(f => f[key], element.Value));
                }
            }
            return updates;
        }

        /// <summary>删除</summary>
        public async Task<IEnumerable<string>> Delete2Async(MetadataCollection metadatas)
        {
            var metas = metadatas.Select(e => e.As<Metadata>()).ToList();
            var docs = metas.ConvertAll(DicConvertToBsonDoc);
            var result = new List<string>();
            await DeleteAsync(docs);
            result = metadatas.Select(s => s.IIId).ToList();
            return result;
        }

        #endregion

        #region mongodb BsonDocument 实现索引服务的【保存 更新 删除】作为基础方法

        /// <summary>
        /// 元数据集合转化为层级结构的BsonDocumnet
        /// </summary>
        /// <param name="metadatas">元数据集合（KMD扁平存储）</param>
        /// <returns></returns>
        private List<BsonDocument> MetadataColToBsonDocList(MetadataCollection metadatas)
        {
            var kmds = metadatas.Select(e => e.As<KMD>()).ToList(); //转化为kmd集合
            var indexs = kmds.ConvertAll(KmdToIndex);            //转化为index集合
            var docs = indexs.ConvertAll(JsonConvertToBsonDoc); //转化为bsondocument集合
            return docs;
        }
        /// <summary>
        /// kmd集合转化为
        /// </summary>
        /// <param name="kmds"></param>
        /// <returns></returns>
        private List<BsonDocument> KmdsToBsonDocList(List<KMD> kmds)
        {
            var indexs = kmds.ConvertAll(KmdToIndex);            //转化为index集合
            var docs = indexs.ConvertAll(JsonConvertToBsonDoc); //转化为bsondocument集合
            return docs;
        }
        /// <summary>
        /// 为KMD设置IIID
        /// </summary>
        /// <param name="kmd">元数据扁平结构</param>
        /// <returns></returns>
        private string KmdToIndex(KMD kmd)
        {
            if (kmd.GetProperty(MetadataConsts.IndexedDate) == null)
                kmd.SetProperty(MetadataConsts.IndexedDate, DateTime.Now.ToISODateString());
            if (kmd.GetProperty(IiId) == null)
            {
                if (kmd.GetProperty(Url) == null)
                    MetadataExceptionCodes.UrlNotExist.ThrowUserFriendly($"{Url}键值不存在！", $"{Url}为必要键，请查看索引数据是否包含！");
                kmd.SetProperty(IiId, kmd.GetProperty(Url).ToString().ToMD5());
            }
            return kmd.ToIndex();
        }

        /// <summary>
        /// json转化为BsonDocument
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        private BsonDocument JsonConvertToBsonDoc(string json)
        {
            var token = JToken.Parse(json);
            var dict = token.ToDictionary();
            return BsonDocument.Create(dict);
        }

        /// <summary>
        /// 保存BsonDocument
        /// </summary>
        /// <param name="docs"></param>
        /// <returns></returns>
        private async Task SaveAsync(List<BsonDocument> docs)
        {
            var updateOptions = new UpdateOptions { IsUpsert = true };
            Func<BsonDocument, FilterDefinition<BsonDocument>> filterBuilder = doc => Builders<BsonDocument>.Filter.Eq(f => f[MongoIiId], doc[MongoIiId]);
            try
            {
                await _access.ReplaceManyAsync(docs, filterBuilder, updateOptions);
            }
            catch (Exception ex)
            {
                IndexExceptionCodes.SavingIndexFailed.ThrowUserFriendly(ex.Message, "保存索引失败");
            }
        }

        /// <summary>
        /// 删除BsonDocument
        /// </summary>
        /// <param name="docs"></param>
        /// <returns></returns>
        private async Task DeleteAsync(IEnumerable<BsonDocument> docs)
        {
            var urlFilter = Builders<BsonDocument>.Filter.In(f => f[MongoIiId], docs.Select(s => s[MongoIiId]));
            try
            {
                await _access.DeleteAsync(urlFilter);
            }
            catch (Exception ex)
            {
                IndexExceptionCodes.DeletingIndexFailed.ThrowUserFriendly(ex.Message, "删除元数据失败!");
            }
        }
        #endregion
    }
}
