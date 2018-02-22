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
    [ETLComponent("SQLMetadataExtracterTask", Title = "元数据提取任务")]
    public class SQLMetadataExtracterTask : ETLWrappedTask
    {
        /// <summary>构造函数</summary>
        public SQLMetadataExtracterTask() { }
        /// <summary>参数信息集合</summary>
        public List<ETLParameterInfo> Parameters { get; set; }
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
            var sqlTask = this.SqlTask;
            var query = sqlTask.CommandText;
            var builder = new StringBuilder();
            builder.AppendFormat("select * from ({0}) AS T", query);
            if (this.Parameters.Count > 0)
            {
                builder.Append(" where 1=1 ");
                foreach (var parameter in this.Parameters)
                {
                    var dbParameter = sqlTask.AddParameter(parameter, context, inputRow, inputColumn, inputParameter);
                    builder.AppendFormat(" AND T.{0}={1} ", sqlTask.NormalizeObjectName(parameter.Name), dbParameter.ParameterName);
                    command.Parameters.Add(dbParameter);
                }
            }
            command.CommandText = builder.ToString();
        }

        #region XML配置方法
        /// <summary>加载</summary>
        public override void LoadXml(ETLXmlConfiguration config, XElement node)
        {
            base.LoadXml(config, node);
            this.Parameters = config.LoadParameters(node);
            Initialize();
        }
        /// <summary>生成</summary>
        public override void BuildXml(ETLXmlConfiguration config, XElement node)
        {
            base.BuildXml(config, node);
            node.Add(config.BuildParameters(this.Parameters));
        }
        #endregion
    }
}
