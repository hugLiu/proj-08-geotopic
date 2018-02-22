using Jurassic.So.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.Data
{
    /// <summary>JSON数据列</summary>
    [Serializable]
    [DataContract]
    public class JsonDataColumn
    {
        /// <summary>构造函数</summary>
        public JsonDataColumn() { }
        /// <summary>构造函数</summary>
        public JsonDataColumn(DataColumn column)
        {
            this.Name = column.ColumnName;
            this.Title = column.Caption;
            this.Type = ToJsonDataType(column.DataType);
        }
        /// <summary>名称</summary>
        [DataMember(Name = "name")]
        public string Name { get; private set; }
        /// <summary>标题</summary>
        [DataMember(Name = "title")]
        public string Title { get; private set; }
        /// <summary>类型</summary>
        [DataMember(Name = "type")]
        public JsonDataType Type { get; private set; }
        /// <summary>转成JSON数据类型</summary>
        public JsonDataType ToJsonDataType(Type type)
        {
            switch (System.Type.GetTypeCode(type))
            {
                case TypeCode.Boolean:
                    return JsonDataType.Boolean;
                case TypeCode.DateTime:
                    return JsonDataType.Date;
                case TypeCode.Char:
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    return JsonDataType.Number;
                default:
                    return JsonDataType.String;
            }
        }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}
