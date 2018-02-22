using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Jurassic.PKS.Service;
using Jurassic.PKS.Service.Adapter;
using Jurassic.PKS.Service.Data;
using Jurassic.PKS.WebAPI.Models.Adapter;
using Jurassic.So.Data.Entities;
using Jurassic.So.Infrastructure;

namespace Jurassic.So.Data.Center
{
    /// <summary>模拟数据服务</summary>
    public class DataCenter : IData
    {
        /// <summary>构造函数</summary>
        public DataCenter() { }
        /// <summary>获得已注册的适配器信息</summary>
        protected GT_AdapterInfo GetAdapterInfo(string adapterId)
        {
            using (var context = new DataServiceDBContext())
            {
                return context.GT_AdapterInfo
                    .Include(e => e.GT_SpiderScope)
                    //未挂起且正常运行
                    .Where(e => e.Hangup == 0 && e.Status == 1 && e.AdapterName == adapterId)
                    .FirstOrDefault();
            }
        }
        /// <summary>获得已注册的适配器信息集合</summary>
        public AdapterInfoCollection GetAdapterInfos()
        {
            using (var context = new DataServiceDBContext())
            {
                var values = context.GT_AdapterInfo
                    .Include(e => e.GT_SpiderScope)
                    .Where(e => e.Hangup == 0 && e.Status == 1)//未挂起且正常运行
                    .ToList();
                return values.MapTo<AdapterInfo>().ToList(new AdapterInfoCollection());
            }
        }
        /// <summary>获得已注册的适配器信息集合</summary>
        public async Task<AdapterInfoCollection> GetAdapterInfosAsync()
        {
            return await Task.Run(() => GetAdapterInfos()).ConfigureAwait(false);
        }
        /// <summary>验证配器信息</summary>
        protected void ValidateAdapterInfo(GT_AdapterInfo gt_AdapterInfo, string adapterId, string scope)
        {
            if (gt_AdapterInfo == null)
            {
                DataExceptionCode.AdapterNotExist.ThrowUserFriendly($"适配器[{adapterId}]不存在！", "适配器不存在！");
            }
            var gt_ScopeInfo = gt_AdapterInfo.GT_SpiderScope.FirstOrDefault(e => e.SpiderScope == scope);
            if (gt_AdapterInfo == null)
            {
                DataExceptionCode.AdapterScopeNotExist.ThrowUserFriendly($"适配器域[{scope}]不存在！", "适配器域不存在！");
            }
        }
        /// <summary>分批爬取元数据</summary>
        public virtual SpiderResult Spider(string adapterId, string scope, Pager pager, string incrementValue)
        {
            return SpiderAsync(adapterId, scope, pager, incrementValue).Result;
        }
        /// <summary>分批爬取元数据</summary>
        public async Task<SpiderResult> SpiderAsync(string adapterId, string scope, Pager pager, string incrementValue)
        {
            var gt_AdapterInfo = await Task.Run(() => GetAdapterInfo(adapterId)).ConfigureAwait(false);
            ValidateAdapterInfo(gt_AdapterInfo, adapterId, scope);
            var adapter = new ApiWrappedAdapter(gt_AdapterInfo.AdapterURL);
            var request = new AdapterSpiderRequest();
            request.Scope = scope;
            request.IncrementValue = incrementValue;
            request.Pager = pager;
            return await adapter.SpiderAsync(request).ConfigureAwait(false);
        }
        /// <summary>获取实体数据的结构(schema)</summary>
        public virtual DataSchemaCollection Retrieve(string url)
        {
            return RetrieveAsync(url).Result;
        }
        /// <summary>获取实体数据的结构(schema)</summary>
        public async Task<DataSchemaCollection> RetrieveAsync(string url)
        {
            var adpUrl = ADPUrl.Analyze(url);
            var gt_AdapterInfo = await Task.Run(() => GetAdapterInfo(adpUrl.Adapter)).ConfigureAwait(false);
            ValidateAdapterInfo(gt_AdapterInfo, adpUrl.Adapter, adpUrl.Scope);
            var adapter = new ApiWrappedAdapter(gt_AdapterInfo.AdapterURL);
            var request = new AdapterRetrieveRequest();
            request.Scope = adpUrl.Scope;
            request.NatureKey = adpUrl.NatureKey;
            return await adapter.RetrieveAsync(request).ConfigureAwait(false);
        }
        /// <summary>根据凭证获取实体数据项</summary>
        public virtual DataResult GetData(string url, string ticket, Pager pager)
        {
            return GetDataAsync(url, ticket, pager).Result;
        }
        /// <summary>根据凭证获取实体数据项</summary>
        public async Task<DataResult> GetDataAsync(string url, string ticket, Pager pager)
        {
            var adpUrl = ADPUrl.Analyze(url);
            var gt_AdapterInfo = await Task.Run(() => GetAdapterInfo(adpUrl.Adapter)).ConfigureAwait(false);
            ValidateAdapterInfo(gt_AdapterInfo, adpUrl.Adapter, adpUrl.Scope);
            var adapter = new ApiWrappedAdapter(gt_AdapterInfo.AdapterURL);
            var request = new AdapterGetDataRequest();
            request.Ticket = ticket;
            var pager2 = pager.GetPagerOrDefault();
            request.From = pager2.From;
            request.Size = pager2.Size;
            return await adapter.GetDataAsync(request).ConfigureAwait(false);
        }
    }
}
