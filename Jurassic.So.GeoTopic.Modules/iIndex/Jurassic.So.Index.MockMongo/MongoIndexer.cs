using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jurassic.PKS.Service;
using Jurassic.PKS.Service.Index;
using Jurassic.So.Business;
using Jurassic.So.Infrastructure;

namespace Jurassic.So.Index.MockMongo
{
    /// <summary>Mongo索引服务</summary>
    public class MongoIndexer : IIndexer
    {
        /// <summary>构造函数</summary>
        public MongoIndexer(IMetadataRepository repository)
        {
            this.Repository = repository;
        }
        /// <summary>Mongo索引数据访问服务</summary>
        private IMetadataRepository Repository { get; set; }
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
            switch (indexInfo.Action)
            {
                case IndexAction.Save:
                    iiids = await SaveAsync(indexInfo);
                    break;
                case IndexAction.Update:
                    iiids = await UpdateAsync(indexInfo);
                    break;
                case IndexAction.Delete:
                    iiids = await DeleteAsync(indexInfo);
                    break;
                default:
                    ExceptionCodes.InvalidEnumValue.ThrowUserFriendly("无效的索引操作！", $"枚举值[{indexInfo.Action.ToString()}]无效！");
                    break;
            }
            var result = new IndexResult();
            result.IIIds.AddRange(iiids);
            return result;
        }
        /// <summary>保存</summary>
        private async Task<IEnumerable<string>> SaveAsync(IndexInfo indexInfo)
        {
            IEnumerable<string> result = null;
            try
            {
                var operation = "保存";
                OnSaving(operation, indexInfo);
                result = await this.Repository.SaveAsync(indexInfo.Metadatas);
                //OnSaved(indexInfo);
            }
            catch (Exception ex)
            {
                ex.Throw(IndexExceptionCodes.SavingIndexFailed, "保存索引失败");
            }
            return result;
        }
        /// <summary>保存前的处理</summary>
        private void OnSaving(string operation, IndexInfo indexInfo)
        {
            foreach (var metadata in indexInfo.Metadatas)
            {
                ValidateMetadata(operation, metadata);
            }
            var now = DateTime.Now;
            foreach (var metadata in indexInfo.Metadatas)
            {
                metadata.IndexedDate = now;
                metadata.IIId = metadata.ComputeIIId();
            }
        }
        /// <summary>保存后的处理</summary>
        private void OnSaved(IndexInfo indexInfo)
        {

        }
        /// <summary>更新</summary>
        private async Task<IEnumerable<string>> UpdateAsync(IndexInfo indexInfo)
        {
            IEnumerable<string> result = null;
            try
            {
                var operation = "更新";
                OnUpdating(operation, indexInfo);
                result = await this.Repository.UpdateAsync(indexInfo.Metadatas);
                //OnUpdated(indexInfo);
            }
            catch (Exception ex)
            {
                ex.Throw(IndexExceptionCodes.UpdatingIndexFailed, "更新索引失败");
            }
            return result;
        }
        /// <summary>更新前的处理</summary>
        private void OnUpdating(string operation, IndexInfo indexInfo)
        {
            foreach (var metadata in indexInfo.Metadatas)
            {
                ValidateUrl(operation, metadata);
            }
            var now = DateTime.Now;
            foreach (var metadata in indexInfo.Metadatas)
            {
                metadata.IndexedDate = now;
            }
        }
        /// <summary>更新后的处理</summary>
        private void OnUpdated(IndexInfo indexInfo)
        {
        }
        /// <summary>删除</summary>
        private async Task<IEnumerable<string>> DeleteAsync(IndexInfo indexInfo)
        {
            IEnumerable<string> result = null;
            try
            {
                var operation = "删除";
                OnDeleting(operation, indexInfo);
                result = await this.Repository.DeleteAsync(indexInfo.Metadatas);
                //OnDeleted(indexInfo);
            }
            catch (Exception ex)
            {
                ex.Throw(IndexExceptionCodes.DeletingIndexFailed, "删除索引失败");
            }
            return result;
        }
        /// <summary>删除前的处理</summary>
        private void OnDeleting(string operation, IndexInfo indexInfo)
        {
            foreach (var metadata in indexInfo.Metadatas)
            {
                ValidateUrl(operation, metadata);
            }
        }
        /// <summary>删除后的处理</summary>
        private void OnDeleted(IndexInfo indexInfo)
        {
        }
        /// <summary>验证元数据</summary>
        private void ValidateMetadata(string operation, IMetadata metadata)
        {
            ValidateUrl(operation, metadata);
        }
        /// <summary>验证元数据中URL是否存在</summary>
        private void ValidateUrl(string operation, IMetadata metadata)
        {
            if (!metadata.Url.IsNullOrEmpty()) return;
            var message = $"{operation}索引信息失败！";
            var details = "元数据URL不允许为null或空！";
            ExceptionCodes.MissingParameterValue.ThrowUserFriendly(message, details);
        }
    }
}
