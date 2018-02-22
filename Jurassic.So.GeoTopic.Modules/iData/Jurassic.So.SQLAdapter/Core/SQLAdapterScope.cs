using System.Linq;
using Jurassic.So.ETL;
using Jurassic.So.Infrastructure;
using Jurassic.PKS.Service;
using Jurassic.PKS.Service.Adapter;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace Jurassic.So.Adapter
{
    /// <summary>适配器域</summary>
    [ETLComponent("SQLAdapterScope", Title = "SQL适配器域")]
    public class SQLAdapterScope : IAdapterScope
    {
        #region 构造函数
        /// <summary>构造函数</summary>
        public SQLAdapterScope()
        {
        }
        #endregion

        #region 数据成员
        /// <summary>配置</summary>
        public AdapterConfigurationSectionHandler Configuration { get; set; }
        /// <summary>适配器域信息</summary>
        public Scope ScopeInfo { get; set; }
        /// <summary>爬取分页大小</summary>
        public int SpiderSize { get; set; }
        /// <summary>公共任务</summary>
        public Dictionary<string, ETLTask> CommonTasks { get; set; }
        /// <summary>变量字典</summary>
        public Dictionary<string, object> Variables { get; set; }
        #endregion

        #region ETL包
        /// <summary>获取元数据总数包</summary>
        public ETLPackage SpiderCountPackage { get; set; }
        /// <summary>获取元数据包</summary>
        public ETLPackage SpiderPackage { get; set; }
        /// <summary>获取数据内容项包</summary>
        public ETLPackage RetrievePackage { get; set; }
        /// <summary>获取数据内容包</summary>
        public ETLPackage GetDataPackage { get; set; }
        #endregion

        #region 服务方法
        /// <summary>生成ETL执行上下文</summary>
        protected void BuildExecuteContext(ETLExecuteContext context)
        {
            context.Variables.AddRange(this.Variables);
            this.Configuration.SetAdapterScopeVariables(context.Variables, this);
        }
        /// <summary>验证增量值</summary>
        protected object ValidateIncrementValue(string incrementValue)
        {
            if (incrementValue.IsNullOrEmpty()) return null;
            switch (this.ScopeInfo.IncrementType)
            {
                case IncrementType.None:
                    AdapterExceptionCode.InvalidIncrementValue.ThrowUserFriendly($"增量值[{incrementValue}]不允许！", "无效的增量值！");
                    break;
                case IncrementType.ID:
                    int iIncrementValue;
                    if (!int.TryParse(incrementValue, out iIncrementValue))
                    {
                        AdapterExceptionCode.InvalidIncrementValue.ThrowUserFriendly($"增量值[{incrementValue}]必须是整数值！", "无效的增量值！");
                    }
                    return iIncrementValue;
                case IncrementType.Date:
                    var dtIncrementValue = incrementValue.ToISODate();
                    if (!dtIncrementValue.HasValue)
                    {
                        AdapterExceptionCode.InvalidIncrementValue.ThrowUserFriendly($"增量值[{incrementValue}]必须是ISO日期值！", "无效的增量值！");
                    }
                    return dtIncrementValue.Value;
            }
            return null;
        }
        /// <summary>验证爬取分页参数</summary>
        protected Pager ValidateSpiderPaper(Pager pager)
        {
            var paper2 = pager.GetPagerOrDefault();
            if (paper2.From < 0) paper2.From = 0;
            if (paper2.Size < 0) paper2.Size = 0;
            if (paper2.Size <= 0) paper2.Size = this.SpiderSize;
            if (paper2.Size <= 0) paper2.Size = this.Configuration.DefaultSpiderSize;
            return paper2;
        }
        /// <summary>验证获取数据分页参数</summary>
        protected Pager ValidateGetDataPaper(Pager pager)
        {
            var paper2 = pager.GetPagerOrDefault();
            if (paper2.From < 0) paper2.From = 0;
            if (paper2.Size < 0) paper2.Size = 0;
            return paper2;
        }
        /// <summary>获得元数据总数</summary>
        public int GetMetadataTotal(ETLExecuteContext context, string incrementValue)
        {
            var incrementValue2 = ValidateIncrementValue(incrementValue);
            BuildExecuteContext(context);
            this.Configuration.SetAdapterScopeVariable_IncrementValue(context.Variables, incrementValue2);
            var result = this.SpiderCountPackage.Execute(context);
            var firstRow = result.Rows.First();
            var firstColumn = result.Columns.Values.First();
            return (int)Convert.ChangeType(firstRow[firstColumn], typeof(int));
        }
        /// <summary>获得元数据集合</summary>
        public MetadataCollection GetMetadatas(ETLExecuteContext context, string incrementValue, Pager pager)
        {
            var incrementValue2 = ValidateIncrementValue(incrementValue);
            var paper2 = ValidateSpiderPaper(pager);
            BuildExecuteContext(context);
            this.Configuration.SetAdapterScopeVariable_IncrementValue(context.Variables, incrementValue2);
            this.Configuration.SetAdapterScopeVariable_Pager(context.Variables, paper2);
            var result = this.SpiderPackage.Execute(context);
            return result.Rows.Cast<IMetadata>().ToList(new MetadataCollection());
        }
        /// <summary>根据域和成果键获取成果的内容项集合</summary>
        public DataSchemaCollection Retrieve(ETLExecuteContext context, string natureKey)
        {
            BuildExecuteContext(context);
            this.Configuration.SetAdapterScopeVariable_NatureKey(context.Variables, natureKey);
            var result = this.RetrievePackage.Execute(context);
            return result.Rows.Cast<DataSchema>().ToList(new DataSchemaCollection());
        }
        /// <summary>根据数据项票据获取成果的数据项</summary>
        public DataResult GetData(ETLExecuteContext context, string natureKey, string ticket, Pager pager)
        {
            var paper2 = ValidateGetDataPaper(pager);
            BuildExecuteContext(context);
            this.Configuration.SetAdapterScopeVariable_NatureKey(context.Variables, natureKey);
            this.Configuration.SetAdapterScopeVariable_Ticket(context.Variables, ticket);
            this.Configuration.SetAdapterScopeVariable_Pager(context.Variables, paper2);
            var result = this.GetDataPackage.Execute(context);
            return result.Rows.FirstOrDefault().As<DataResult>();
        }
        #endregion
    }
}
