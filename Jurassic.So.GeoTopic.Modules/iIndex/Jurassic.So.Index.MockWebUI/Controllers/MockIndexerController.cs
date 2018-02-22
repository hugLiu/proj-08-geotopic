using Jurassic.WebFrame;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;
using Jurassic.So.Infrastructure;
using Jurassic.So.Business;
using Jurassic.PKS.Service.Index;
using Jurassic.PKS.Service;

namespace Jurassic.So.Index
{
    /// <summary>模拟索引服务控制器</summary>
    public class MockIndexerController : BaseController
    {
        /// <summary>索引服务</summary>
        private IIndexer Indexer { get; set; }
        /// <summary>构造函数</summary>
        public MockIndexerController(IIndexer indexer)
        {
            this.Indexer = indexer;
        }
        /// <summary>列表页</summary>
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>索引操作信息</summary>
        [Serializable]
        public class OutputIndexInfo
        {
            /// <summary>构造函数</summary>
            public OutputIndexInfo(IMetadata item)
            {
                this.IndexedDate = item.IndexedDate.Value;
                this.S_Url = item.Url;
                var metadata = item.As<Metadata>().DeepClone();
                //metadata.RemoveValue(MetadataConsts.IndexQuality);
                metadata.RemoveValue(MetadataConsts.IndexedDate);
                metadata.RemoveValue(MetadataConsts.SourceUrl);
                metadata.RemoveValue(MetadataConsts.IIId);
                this.Metadata = metadata.ToJson();
            }
            /// <summary>索引质量,默认值为"100%"</summary>
            public string IndexQuality { get; set; }
            /// <summary>索引数据日期</summary>
            public DateTime IndexedDate { get; set; }
            /// <summary>索引URL</summary>
            public string S_Url { get; set; }
            /// <summary>数据</summary>
            public string Metadata { get; set; }
        }
        /// <summary>发送索引信息</summary>
        public ActionResult GetAll()
        {
            var items = ((MockIndexer)this.Indexer).Items;
            var models = items.Select(e => new OutputIndexInfo(e)).ToArray();
            return Json(models);
        }
        /// <summary>新增或编辑</summary>
        public ActionResult Edit(string id)
        {
            var items = ((MockIndexer)this.Indexer).Items;
            IMetadata item;
            if (id.IsNullOrEmpty())
            {
                item = new Metadata();
                item.IndexedDate = DateTime.Now;
                //item.Metadata.S_Url = string.Empty;
            }
            else
            {
                item = items.FirstOrDefault(e => e.Url == id);
                //if (item == null) throw new ArgumentException($"s:url[{id}]不存在！");
            }
            var model = new OutputIndexInfo(item);
            return View(model);
        }
        /// <summary>发送索引信息</summary>
        [HttpPost]
        private async Task<ActionResult> SendIndex(IndexInfo indexInfo)
        {
            //var pairs = new List<KeyValuePair<string, object>>();
            //foreach (var pair in indexInfo.Metadatas)
            //{
            //    var value = pair.Value;
            //    var varray = value as string[];
            //    if (varray == null) continue;
            //    if (varray.Length == 1)
            //    {
            //        pairs.Add(new KeyValuePair<string, object>(pair.Key, varray[0]));
            //    }
            //}
            //foreach (var pair in pairs)
            //{
            //    indexInfo.Metadata[pair.Key] = pair.Value;
            //}
            //var keys = indexInfo.Metadata.Keys.Where(key => key.EndsWith("[0]")).ToArray();
            //foreach (var key in keys)
            //{
            //    var key2 = key.Substring(0, key.Length - 3);
            //    var values = new List<object>();
            //    values.Add(indexInfo.Metadata[key]);
            //    var i = 1;
            //    while (true)
            //    {
            //        var key3 = key2 + "[" + i.ToString() + "]";
            //        if (!indexInfo.Metadata.Keys.Any(e => e == key3)) break;
            //        values.Add(indexInfo.Metadata[key3]);
            //        indexInfo.Metadata.Remove(key3);
            //        i++;
            //    }
            //    indexInfo.Metadata[key2] = values;
            //    indexInfo.Metadata.Remove(key);
            //}
            //await this.Indexer.SendIndexAsync(indexInfo);
            //return Json(new { sucess = true }, JsonRequestBehavior.AllowGet);
            return null;
        }
    }
}
