using Jurassic.So.Business;
using Jurassic.So.Infrastructure;
using Jurassic.So.Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.Data
{
    /// <summary>数据集格式的数据结果</summary>
    [Serializable]
    public class DataSetDataResult : Dictionary<string, JsonDataTable>
    {
        /// <summary>构造函数</summary>
        public DataSetDataResult() { }
        /// <summary>构造函数</summary>
        public DataSetDataResult(DataSet dataSet)
        {
            foreach(DataTable table in dataSet.Tables)
            {
                base.Add(table.TableName, new JsonDataTable(table));
            }
        }
        /// <summary>构造函数</summary>
        public DataSetDataResult(DataTable table)
        {
            base.Add(table.TableName, new JsonDataTable(table));
        }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}
