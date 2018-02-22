using Jurassic.PKS.Service;
using Jurassic.PKS.Service.Index;
using Jurassic.So.Business;
using Jurassic.So.Infrastructure;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.Index
{
    /// <summary>模拟索引服务</summary>
    public class MockIndexer : IIndexer
    {
        /// <summary>构造函数</summary>
        public MockIndexer(IServiceMockConfig config)
        {
            this.Config = config;
            var file = this.DataFile;
            if (File.Exists(file))
            {
                var content = File.ReadAllText(file, Encoding.UTF8);
                try
                {
                    this.Items = content
                        .JsonTo()
                        .CastTo<JArray>()
                        .Cast<JObject>()
                        .JsonToMetadataList();
                }
                catch
                {
                }
            }
            if (this.Items == null)
            {
                this.Items = new MetadataCollection();
            }
        }
        /// <summary>配置</summary>
        private IServiceMockConfig Config { get; set; }
        /// <summary>数据文件</summary>
        private string DataFile
        {
            get { return this.Config.DataPath + "MockIndexItems.json"; }
        }
        /// <summary>索引项集合</summary>
        public MetadataCollection Items { get; private set; }
        /// <summary>插入/更新/删除索引信息</summary>
        public IndexResult SendIndex(IndexInfo indexInfo)
        {
            var result = new IndexResult();
            lock (this.Items)
            {
                switch (indexInfo.Action)
                {
                    case IndexAction.Save:
                        foreach (var metadata in indexInfo.Metadatas)
                        {
                            var index = this.Items.FindIndex(e => e.Url == metadata.Url);
                            var metadata2 = metadata.As<Metadata>();
                            metadata2.IndexedDate = DateTime.Now;
                            //metadata2.IndexQuality = indexInfo.IndexQuality;
                            metadata2.IIId = metadata2.ComputeIIId();
                            if (index < 0)
                            {
                                this.Items.Add(metadata2);
                            }
                            else
                            {
                                this.Items[index] = metadata2;
                            }
                            result.IIIds.Add(metadata2.IIId);
                        }
                        break;
                    case IndexAction.Update:
                        foreach (var metadata in indexInfo.Metadatas)
                        {
                            var index = this.Items.FindIndex(e => e.Url == metadata.Url);
                            if (index < 0)
                            {
                                throw new ArgumentException($"source.url[{metadata.Url}]不存在！");
                            }
                            var metadata2 = metadata.As<Metadata>();
                            metadata2.IndexedDate = DateTime.Now;
                            //metadata2.IndexQuality = indexInfo.IndexQuality;
                            metadata2.IIId = metadata2.ComputeIIId();
                            //var store = this.Items[index];
                            //store.As<Metadata>().MergeValue(metadata2);
                            this.Items[index] = metadata2;
                            result.IIIds.Add(metadata2.IIId);
                        }
                        break;
                    case IndexAction.Delete:
                        foreach (var metadata in indexInfo.Metadatas)
                        {
                            var index = this.Items.FindIndex(e => e.Url == metadata.Url);
                            if (index < 0)
                            {
                                result.IIIds.Add("");
                                continue;
                            }
                            var metadata2 = this.Items[index].As<Metadata>();
                            result.IIIds.Add(metadata2.IIId);
                            this.Items.RemoveAt(index);
                        }
                        break;
                }
                var content = this.Items.ToJson();
                var file = this.DataFile;
                File.WriteAllText(file, content, Encoding.UTF8);
            }
            return result;
        }
        /// <summary>插入/更新/删除索引信息</summary>
        public Task<IndexResult> SendIndexAsync(IndexInfo indexInfo)
        {
            return Task.FromResult(SendIndex(indexInfo));
        }
    }
}
