using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.So.Data.Center;
using Jurassic.So.Data.Entities;
using Jurassic.So.Index;
using Jurassic.So.Infrastructure;
using Jurassic.PKS.Service.Index;
using Jurassic.PKS.Service;
using Jurassic.PKS.Service.Adapter;
using Jurassic.PKS.Service.Data;

namespace Jurassic.So.Data
{
    /// <summary>模拟数据服务</summary>
    public class MockDataService : DataCenter
    {
        /// <summary>构造函数</summary>
        public MockDataService(IServiceMockConfig config, IIndexer indexer)
        {
            this.Config = config;
            this.Indexer = indexer;
        }
        /// <summary>配置</summary>
        private IServiceMockConfig Config { get; set; }
        /// <summary>索引服务</summary>
        private IIndexer Indexer { get; set; }
        /// <summary>数据文件</summary>
        private string DataFile
        {
            get { return this.Config.DataPath + "MockDataItems.json"; }
        }
        /// <summary>分批爬取元数据</summary>
        public override SpiderResult Spider(string adapterId, string scope, Pager pager, string incrementValue)
        {
            GT_AdapterInfo ds_AdapterInfo = null;
            using (var context = new DataServiceDBContext())
            {
                ds_AdapterInfo = context.GT_AdapterInfo
                    .Include(e => e.GT_SpiderScope)
                    //未挂起且正常运行
                    .Where(e => e.Hangup == 0 && e.Status == 1 && e.AdapterName == adapterId)
                    .FirstOrDefault();
            }
            if (ds_AdapterInfo == null)
            {
                DataExceptionCode.AdapterNotExist.ThrowUserFriendly("适配器不存在！", $"适配器[{adapterId}]不存在！");
            }
            var ds_ScopeInfo = ds_AdapterInfo.GT_SpiderScope
                .Where(e => e.SpiderScope == scope)
                .FirstOrDefault();
            if (ds_AdapterInfo == null)
            {
                DataExceptionCode.AdapterScopeNotExist.ThrowUserFriendly("适配器域不存在！", $"适配器域[{scope}]不存在！");
            }
            var adapter = new MockAdapter(this.Config);
            adapter.Load(ds_AdapterInfo);
            var items = adapter.Spider(scope, incrementValue, pager);
            //this.Indexer.UpdateOrInsertMany(items);
            return items;
        }
        /// <summary>获取实体数据的结构(schema)</summary>
        public override DataSchemaCollection Retrieve(string url)
        {
            var adpUrl = ADPUrl.Analyze(url);
            var adapterInfo = new GT_AdapterInfo();
            adapterInfo.AdapterName = "CNKI测试";
            adapterInfo.AdapterURL = @"\LocalAdapters\CNKI测试20160817-051225\";
            adapterInfo.InvokeType = 0;
            var adapter = new MockAdapter(this.Config);
            adapter.Load(adapterInfo);
            return adapter.Retrieve(adpUrl.Scope, adpUrl.NatureKey);
        }
        /// <summary>根据凭证获取实体数据项</summary>
        public override DataResult GetData(string url, string ticket, Pager pager)
        {
            var adpUrl = ADPUrl.Analyze(url);
            var adapterInfo = new GT_AdapterInfo();
            adapterInfo.AdapterName = "CNKI测试";
            adapterInfo.AdapterURL = @"\LocalAdapters\CNKI测试20160817-051225\";
            adapterInfo.InvokeType = 0;
            var adapter = new MockAdapter(this.Config);
            adapter.Load(adapterInfo);
            return adapter.GetData(ticket, pager);
        }
    }
}
