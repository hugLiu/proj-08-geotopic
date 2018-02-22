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

namespace Jurassic.So.Adapter
{
    /// <summary>元数据数量提取任务</summary>
    [ETLComponent("SQLSpiderCountTask", Title = "元数据数量提取任务")]
    public class SQLSpiderCountTask : ETLWrappedTask
    {
        /// <summary>构造函数</summary>
        public SQLSpiderCountTask() { }
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
        /// <summary>列字典</summary>
        public IDictionary<string, IETLColumn> Columns { get; set; }
        /// <summary>数据库任务</summary>
        private ETLDbSqlTask SqlTask
        {
            get { return this.WrappedTask.As<ETLDbSqlTask>(); }
        }
        /// <summary>初始化</summary>
        private void Initialize()
        {
            var childTask = this.SqlTask;
            childTask.OutputType = ETLDataOutputType.Ignore;
            childTask.CommandType = System.Data.CommandType.Text;
            childTask.CommandOperation = ETLDbCommandOperation.ExecuteScalar;
            childTask.InjectCommand = InjectCommand;
        }
        /// <summary>准备输出</summary>
        private ETLDictionaryRowCollection PrepareOutput()
        {
            var output = new ETLDictionaryRowCollection();
            output.Columns.AddRange(this.Columns);
            return output;
        }
        /// <summary>执行</summary>
        public override void Execute(ETLExecuteContext context, IETLRow inputRow, IETLColumn inputColumn, object inputParameter)
        {
            var output = PrepareOutput();
            var inputRow2 = output.AddNewRow();
            var inputColumn2 = output.Columns.Values.First();
            var inputParameter2 = new ETLParameterInfo();
            base.Execute(context, inputRow, inputColumn, inputParameter2);
            inputRow2[inputColumn2] = inputParameter2.Value;
            PushOutput(context, output);
        }
        /// <summary>注入命令</summary>
        private void InjectCommand(DbCommand command, ETLExecuteContext context, IETLRow inputRow, IETLColumn inputColumn, object inputParameter)
        {
            var sqlTask = this.SqlTask;
            var query = sqlTask.CommandText;
            var incrementType = this.IncrementTypeColumnInfo.GetValue(context, inputRow, null, inputParameter).ETLToEnum<IncrementType>();
            var incrementColumn = this.IncrementColumnColumnInfo.GetValue(context, inputRow, null, inputParameter).As<string>();
            var incrementValue = this.IncrementValueColumnInfo.GetValue(context, inputRow, null, inputParameter);
            var pattern = "select count(*) from ({0}) T";
            if (!UseIncrement(incrementType, incrementColumn, incrementValue))
            {
                query = pattern.FormatBy(query);
            }
            else
            {
                pattern += " where T.{1}>{2}";
                var parameter = new ETLParameterInfo();
                parameter.Name = "SpiderCount_IncrementValue";
                parameter.Type = incrementType.ToRuntimeType();
                parameter.Value = incrementType.ToRuntimeTypeValue(incrementValue);
                var incrementColumnName = sqlTask.NormalizeObjectName(incrementColumn);
                var dbParameter = sqlTask.AddParameter(parameter, context, inputRow, inputColumn, inputParameter);
                query = pattern.FormatBy(query, incrementColumnName, dbParameter.ParameterName);
                command.Parameters.Add(dbParameter);
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
            this.Columns = config.LoadColumns(node);
            Initialize();
        }
        /// <summary>生成</summary>
        public override void BuildXml(ETLXmlConfiguration config, XElement node)
        {
            base.BuildXml(config, node);
            config.SetElementValue(node, nameof(this.IncrementType), this.IncrementType);
            config.SetElementValue(node, nameof(this.IncrementColumn), this.IncrementColumn);
            config.SetElementValue(node, nameof(this.IncrementValue), this.IncrementValue);
            node.Add(config.BuildColumns(this.Columns));
        }
        #endregion
    }
}
