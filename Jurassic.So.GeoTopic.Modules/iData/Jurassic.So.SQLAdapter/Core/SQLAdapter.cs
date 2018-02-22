using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jurassic.So.ETL;
using Jurassic.PKS.Service.Adapter;
using Jurassic.PKS.Service;
using Jurassic.So.Infrastructure;
using System.Xml.Linq;
using System.Reflection;
using System.Configuration;

namespace Jurassic.So.Adapter
{
    /// <summary>通用适配器</summary>
    [ETLComponent("SQLAdapter", Title = "SQL适配器")]
    public class SQLAdapter : AdapterBase, IXmlConfigAdapter
    {
        #region 构造函数
        /// <summary>构造函数</summary>
        public SQLAdapter() { }
        /// <summary>从配置文件加载</summary>
        protected void LoadFromConfig(string configFile, string sectionName = "")
        {
            var configuration = new AdapterConfigurationSectionHandler();
            configuration.AdapterInstance = this;
            if (sectionName.IsNullOrEmpty()) sectionName = configuration.DefalutSectionName;
            configuration.Load(configFile, sectionName);
        }
        #endregion

        #region 数据成员
        /// <summary>配置</summary>
        public AdapterConfigurationSectionHandler Configuration { get; set; }
        /// <summary>适配器信息</summary>
        public AdapterInfo AdapterInfo { get; set; }
        /// <summary>连接字典</summary>
        public Dictionary<string, ETLConnection> Connections { get; set; }
        /// <summary>变量字典</summary>
        public Dictionary<string, object> Variables { get; set; }
        /// <summary>域集合</summary>
        public List<IAdapterScope> Scopes { get; set; }
        #endregion

        #region 元数据标签
        /// <summary>元数据标签字典</summary>
        public KMDConfiguration MetadataTags { get; set; }
        #endregion

        #region 服务方法
        /// <summary>适配器信息</summary>
        public override AdapterInfo GetAdapterInfo()
        {
            return this.AdapterInfo;
        }
        /// <summary>验证适配器域</summary>
        protected IAdapterScope ValidateScope(string scope)
        {
            var adapterScope = this.Scopes.FirstOrDefault(e => e.ScopeInfo.Name == scope);
            if (adapterScope == null)
            {
                AdapterExceptionCode.AdapterScopeNotExist.ThrowUserFriendly($"适配器域[{scope}]不存在！", "适配器域不存在！");
            }
            return adapterScope;
        }
        /// <summary>生成ETL执行上下文</summary>
        protected ETLExecuteContext BuildExecuteContext()
        {
            var context = new ETLExecuteContext();
            context.Connections.AddRange(this.Connections);
            context.Variables.AddRange(this.Variables);
            this.Configuration.SetAdapterVariables(this, context.Variables);
            return context;
        }
        /// <summary>分批或增量爬取某个适配器域的成果的元数据集合</summary>
        /// <param name="scope">某个适配器域</param>
        /// <param name="incrementValue">增量值，允许为空，如果有值必须是符合该域的增量类型的值</param>
        /// <param name="pager">分页参数，允许为null，为null表示爬取全部，否则只爬取某页数据</param>
        /// <returns>爬取结果</returns>
        public override async Task<SpiderResult> SpiderAsync(string scope, string incrementValue, Pager pager)
        {
            var adapterScope = ValidateScope(scope);
            var result = new SpiderResult();
            result.Total = await GetMetadataTotal(adapterScope, incrementValue).ConfigureAwait(false);
            result.Metadatas = await GetMetadatas(adapterScope, incrementValue, pager).ConfigureAwait(false);
            result.MaxValue = adapterScope.ScopeInfo.IncrementType.GetMaxIncrementColumnValue(result.Metadatas);
            return result;
        }
        /// <summary>获得元数据总数</summary>
        protected async Task<int> GetMetadataTotal(IAdapterScope scope, string incrementValue)
        {
            var context = BuildExecuteContext();
            var task = Task.Run(() => scope.GetMetadataTotal(context, incrementValue));
            return await task.ConfigureAwait(false);
        }
        /// <summary>获得元数据集合</summary>
        protected async Task<MetadataCollection> GetMetadatas(IAdapterScope adapterScope, string incrementValue, Pager pager)
        {
            var context = BuildExecuteContext();
            var task = Task.Run(() => adapterScope.GetMetadatas(context, incrementValue, pager));
            return await task.ConfigureAwait(false);
        }
        /// <summary>根据域和成果键获取成果的内容项集合</summary>
        /// <param name="scope">某个适配器域</param>
        /// <param name="natureKey">成果键</param>
        /// <returns>成果的内容项集合</returns>
        public override async Task<DataSchemaCollection> RetrieveAsync(string scope, string natureKey)
        {
            var adapterScope = ValidateScope(scope);
            var context = BuildExecuteContext();
            var task = Task.Run(() => adapterScope.Retrieve(context, natureKey));
            var result = await task.ConfigureAwait(false);
            foreach (var item in result)
            {
                item.Ticket = BuildScopeTicket(scope, natureKey, item.Ticket);
            }
            return result;
        }
        /// <summary>根据数据项票据获取成果的数据项</summary>
        /// <param name="ticket">成果的数据项票据</param>
        /// <param name="pager">分页参数</param>
        /// <returns>成果的数据项结果</returns>
        public override async Task<DataResult> GetDataAsync(string ticket, Pager pager)
        {
            var values = ValidateScopeTicket(ticket);
            if (values == null)
            {
                AdapterExceptionCode.InvalidTicket.ThrowUserFriendly($"票据[{ticket}]无效！", "票据无效！");
            }
            var scope = values[0];
            var adapterScope = ValidateScope(scope);
            var context = BuildExecuteContext();
            var task = Task.Run(() => adapterScope.GetData(context, values[1], values[2], pager));
            return await task.ConfigureAwait(false);
        }
        /// <summary>生成数据项票据</summary>
        protected string BuildScopeTicket(string scope, string natureKey, string ticket)
        {
            return $"{scope}___{natureKey}___{ticket}";
        }
        /// <summary>解析数据项票据</summary>
        public string[] ValidateScopeTicket(string ticket)
        {
            var separator = new string[] { "___" };
            var values = ticket.Split(separator, 3, StringSplitOptions.None);
            if (values.Length != 3) return null;
            if (values.Any(e => e.Length == 0)) return null;
            return values;
        }
        #endregion
    }
}
