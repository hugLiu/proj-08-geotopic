using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Jurassic.PKS.Service;
using Jurassic.PKS.Service.Adapter;
using Jurassic.So.ETL;
using Jurassic.So.Infrastructure;

namespace Jurassic.So.Adapter
{
    /// <summary>扩展方法</summary>
    public static class AdapterExtension
    {
        #region 增量类型转换
        /// <summary>转换为运行时类型</summary>
        public static Type ToRuntimeType(this IncrementType type)
        {
            switch (type)
            {
                case IncrementType.ID: return typeof(Int32);
                case IncrementType.Date: return typeof(DateTime);
                default: return null;
            }
        }
        /// <summary>转换为运行时值类型</summary>
        public static object ToRuntimeTypeValue(this IncrementType type, object value)
        {
            switch (type)
            {
                case IncrementType.ID:
                    return ETLExtension.ETLConvertValue(value, typeof(int));
                case IncrementType.Date:
                    return ETLExtension.ETLConvertValue(value, typeof(DateTime));
                default: return null;
            }
        }
        /// <summary>获得最大增量列值</summary>
        public static string GetMaxIncrementColumnValue(this IncrementType type, MetadataCollection metadatas)
        {
            //取出自定义标签值，避免传递给下游
            var values = metadatas.Select(e => e.As<IIncMetadataRow>()?.TakeIncrementColumnValue())
                .Where(e => e != null && e.ToString().Length > 0)
                .ToArray();
            if (type == IncrementType.None) return null;
            if (values.Length > 0)
            {
                switch (type)
                {
                    case IncrementType.ID:
                        var iMaxValue = values.Max(e => e.ToString().ToInt32());
                        return iMaxValue.ToString();
                    case IncrementType.Date:
                        var dtMaxValue = values.Max(e => e.ToString().ETLToLocalTime());
                        return dtMaxValue.ToISODateString();
                }
            }
            return "";
        }
        #endregion

        #region 元数据标签类型转换
        /// <summary>转换为运行时类型</summary>
        public static Type ToRuntimeType(this TagType type, bool isFlat)
        {
            switch (type)
            {
                case TagType.Base64StringArray:
                case TagType.StringArray:
                    return typeof(IList<String>);
                case TagType.DateString:
                    if (isFlat) return typeof(DateTime);
                    return typeof(String);
                default:
                    return typeof(String);
            }
        }
        /// <summary>转换为元数据标签列</summary>
        public static ETLColumn ToMetadataTagColumn(this MetadataTag value, bool isFlat)
        {
            var column = new ETLColumn();
            column.Name = value.Name;
            column.Type = value.Type.ToRuntimeType(isFlat);
            return column;
        }
        #endregion
    }
}
