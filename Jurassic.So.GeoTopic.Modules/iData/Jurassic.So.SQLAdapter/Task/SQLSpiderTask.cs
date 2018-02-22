using Jurassic.So.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.So.ETL;
using System.Xml.Linq;
using Jurassic.PKS.Service.Adapter;
using Jurassic.PKS.Service;

namespace Jurassic.So.Adapter
{
    /// <summary>元数据提取任务</summary>
    [ETLComponent("SQLSpiderTask", Title = "元数据提取任务")]
    public class SQLSpiderTask : ETLWrappedTask
    {
        /// <summary>构造函数</summary>
        public SQLSpiderTask() { }
        /// <summary>增量类型</summary>
        public string IncrementType { get; set; }
        /// <summary>增量列列信息</summary>
        private ETLColumnInfo IncrementTypeColumnInfo { get; set; }
        /// <summary>增量列</summary>
        public string IncrementColumn { get; set; }
        /// <summary>增量列列信息</summary>
        private ETLColumnInfo IncrementColumnColumnInfo { get; set; }
        /// <summary>增量值</summary>
        public string IncrementValue { get; set; }
        /// <summary>增量值列信息</summary>
        private ETLColumnInfo IncrementValueColumnInfo { get; set; }
        /// <summary>行号列</summary>
        public string RowNumberColumn { get; set; }
        /// <summary>行号列列信息</summary>
        private ETLColumnInfo RowNumberColumnColumnInfo { get; set; }
        /// <summary>分页值</summary>
        public string Pager { get; set; }
        /// <summary>分页值列信息</summary>
        private ETLColumnInfo PagerColumnInfo { get; set; }
        /// <summary>主键列，增量列不存在时必须定义主键列，可以是多个，用逗号分隔</summary>
        public string PrimaryKey { get; set; }
        /// <summary>数据库任务</summary>
        private ETLDbSqlTask SqlTask
        {
            get { return this.WrappedTask.As<ETLDbSqlTask>(); }
        }
        /// <summary>初始化</summary>
        private void Initialize()
        {
            var childTask = this.SqlTask;
            childTask.InjectCommand = InjectCommand;
        }
        /// <summary>注入命令</summary>
        private void InjectCommand(DbCommand command, ETLExecuteContext context, IETLRow inputRow, IETLColumn inputColumn, object inputParameter)
        {
            var incrementType = this.IncrementTypeColumnInfo.GetValue(context, inputRow, null, inputParameter).ETLToEnum<IncrementType>();
            var incrementColumn = this.IncrementColumnColumnInfo.GetValue(context, inputRow, null, inputParameter).As<string>();
            var incrementValue = this.IncrementValueColumnInfo.GetValue(context, inputRow, null, inputParameter);
            var rowNumberColumn = this.RowNumberColumnColumnInfo.GetValue(context, inputRow, null, inputParameter).As<string>();
            var pager = this.PagerColumnInfo.GetValue(context, inputRow, null, inputParameter).As<Pager>();
            var sqlTask = this.SqlTask;
            var query = sqlTask.CommandText;
            var pattern = string.Empty;
            var usePager = UsePager(pager);
            if (usePager && rowNumberColumn.IsNullOrEmpty())
            {
                rowNumberColumn = "Spider_RowNo";
            }
            var useIncrement = UseIncrement(incrementType, incrementColumn, incrementValue);
            if (useIncrement)
            {
                var incrementColumnName = sqlTask.NormalizeObjectName(incrementColumn);
                pattern = "select T.*";
                if (usePager)
                {
                    var rowNumberField = sqlTask.BuildRowNumberField(rowNumberColumn);
                    pattern += "," + rowNumberField.FormatBy("T." + incrementColumnName);
                }
                pattern += " from ( {0} ) T where T.{1} > {2}";
                var parameter = new ETLParameterInfo();
                parameter.Name = "Spider_IncrementValue";
                parameter.Type = incrementType.ToRuntimeType();
                parameter.Value = incrementType.ToRuntimeTypeValue(incrementValue);
                var dbParameter = sqlTask.AddParameter(parameter, context, inputRow, inputColumn, inputParameter);
                query = pattern.FormatBy(query, incrementColumnName, dbParameter.ParameterName);
                command.Parameters.Add(dbParameter);
            }
            if (UsePager(pager))
            {
                var rowNumberColumnName = sqlTask.NormalizeObjectName(rowNumberColumn);
                if (!useIncrement)
                {
                    pattern = "select T.*";
                    var rowNumberField = sqlTask.BuildRowNumberField(rowNumberColumnName);
                    if (incrementColumn.IsNullOrEmpty())
                    {
                        var primaryKey = this.PrimaryKey.Split(',')
                             .Select(e => "T." + sqlTask.NormalizeObjectName(e.Trim()))
                             .ToArray();
                        var orderBy = string.Join(",", primaryKey);
                        pattern += "," + rowNumberField.FormatBy(orderBy);
                    }
                    else
                    {
                        var incrementColumnName = sqlTask.NormalizeObjectName(incrementColumn);
                        pattern += "," + rowNumberField.FormatBy("T." + incrementColumnName);
                    }
                    pattern += " from ( {0} ) T";
                    query = pattern.FormatBy(query);
                }
                pattern = "select T.* from ( {0} ) T where T.{1} > {2} AND T.{1}<=( {3} + {4} )";
                var fromParameter1 = new ETLParameterInfo();
                fromParameter1.Name = "Spider_PagerFrom1";
                fromParameter1.Type = pager.From.GetType();
                fromParameter1.Value = pager.From;
                var fromDbParameter1 = sqlTask.AddParameter(fromParameter1, context, inputRow, inputColumn, inputParameter);
                var fromParameter2 = new ETLParameterInfo();
                fromParameter2.Name = "Spider_PagerFrom2";
                fromParameter2.Type = pager.From.GetType();
                fromParameter2.Value = pager.From;
                var fromDbParameter2 = sqlTask.AddParameter(fromParameter2, context, inputRow, inputColumn, inputParameter);
                var sizeParameter = new ETLParameterInfo();
                sizeParameter.Name = "Spider_PagerSize";
                sizeParameter.Type = pager.Size.GetType();
                sizeParameter.Value = pager.Size;
                var sizeDbParameter = sqlTask.AddParameter(sizeParameter, context, inputRow, inputColumn, inputParameter);
                query = pattern.FormatBy(query, rowNumberColumnName, fromDbParameter1.ParameterName, fromDbParameter2.ParameterName, sizeDbParameter.ParameterName);
                command.Parameters.Add(fromDbParameter1);
                command.Parameters.Add(fromDbParameter2);
                command.Parameters.Add(sizeDbParameter);
            }
            command.CommandText = query;
        }
        /// <summary>是否能使用增量方式</summary>
        private bool UseIncrement(PKS.Service.Adapter.IncrementType incrementType, string incrementColumn, object incrementValue)
        {
            if (incrementType == PKS.Service.Adapter.IncrementType.None) return false;
            if (incrementColumn.IsNullOrEmpty()) return false;
            if (incrementValue == null || incrementValue.ToString().Length == 0) return false;
            return true;
        }
        /// <summary>是否能使用分页</summary>
        private bool UsePager(Pager pager)
        {
            if (pager == null) return false;
            if (pager.From < 0) return false;
            if (pager.Size <= 0) return false;
            return true;
        }

        #region XML配置方法
        /// <summary>加载</summary>
        public override void LoadXml(ETLXmlConfiguration config, XElement node)
        {
            base.LoadXml(config, node);
            this.IncrementType = config.GetElementValue(node, nameof(this.IncrementType));
            this.IncrementTypeColumnInfo = this.IncrementType.ETLToColumnInfo();
            this.IncrementColumn = config.GetElementValue(node, nameof(this.IncrementColumn));
            this.IncrementColumnColumnInfo = this.IncrementColumn.ETLToColumnInfo();
            this.IncrementValue = config.GetElementValue(node, nameof(this.IncrementValue));
            this.IncrementValueColumnInfo = this.IncrementValue.ETLToColumnInfo();
            this.RowNumberColumn = config.GetElementValue(node, nameof(this.RowNumberColumn));
            this.RowNumberColumnColumnInfo = this.RowNumberColumn.ETLToColumnInfo();
            this.Pager = config.GetElementValue(node, nameof(this.Pager));
            this.PagerColumnInfo = this.Pager.ETLToColumnInfo();
            this.PrimaryKey = config.GetElementValue(node, nameof(this.PrimaryKey));
            Initialize();
        }
        /// <summary>生成</summary>
        public override void BuildXml(ETLXmlConfiguration config, XElement node)
        {
            base.BuildXml(config, node);
            config.SetElementValue(node, nameof(this.IncrementType), this.IncrementType);
            config.SetElementValue(node, nameof(this.IncrementColumn), this.IncrementColumn);
            config.SetElementValue(node, nameof(this.IncrementValue), this.IncrementValue);
            config.SetElementValue(node, nameof(this.RowNumberColumn), this.RowNumberColumn);
            config.SetElementValue(node, nameof(this.Pager), this.Pager);
            config.SetElementValue(node, nameof(this.PrimaryKey), this.PrimaryKey);
        }
        #endregion
    }
}
