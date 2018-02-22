using Jurassic.So.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization;
using Jurassic.PKS.Service;
using System.IO;
using Jurassic.So.Business;
using System.Linq;

namespace Jurassic.So.Adapter
{
    /// <summary>扁平元数据，保存元数据实际值，尽量避免转换</summary>
    [Serializable]
    [DataContract]
    public class FlatMetadata : Dictionary<string, object>, IMetadata
    {
        /// <summary>构造函数</summary>
        public FlatMetadata() { }
        /// <summary>索引数据ID</summary>
        [IgnoreDataMember, JsonIgnore]
        public string IIId
        {
            get { return GetValue_String(MetadataConsts.IIId); }
            set { SetValue(MetadataConsts.IIId, value); }
        }
        /// <summary>索引数据日期</summary>
        [IgnoreDataMember, JsonIgnore]
        public DateTime? IndexedDate
        {
            get { return GetValue_Date(MetadataConsts.IndexedDate); }
            set { SetValue_Date(MetadataConsts.IndexedDate, value); }
        }
        /// <summary>缩略图</summary>
        [IgnoreDataMember, JsonIgnore]
        public Image Thumbnail
        {
            get { return GetValue_Image(MetadataConsts.Thumbnail); }
        }
        /// <summary>全文</summary>
        [IgnoreDataMember, JsonIgnore]
        public string Fulltext
        {
            get { return GetValue_String(MetadataConsts.Fulltext); }
        }
        /// <summary>适配器URL</summary>
        [IgnoreDataMember, JsonIgnore]
        public string Url
        {
            get { return GetValue_String(MetadataConsts.SourceUrl); }
        }
        /// <summary>标题</summary>
        [IgnoreDataMember, JsonIgnore]
        public string Title
        {
            get { return GetValue_String(MetadataConsts.FormalTitle); }
        }
        /// <summary>描述</summary>
        [IgnoreDataMember, JsonIgnore]
        public string Description
        {
            get { return GetValue_String(MetadataConsts.Description); }
        }
        /// <summary>创建者</summary>
        [IgnoreDataMember, JsonIgnore]
        public string Creator
        {
            get { return GetValue_String(MetadataConsts.Author); }
        }
        /// <summary>创建日期</summary>
        [IgnoreDataMember, JsonIgnore]
        public DateTime? CreatedDate
        {
            get { return GetValue_Date(MetadataConsts.CreatedDate); }
        }
        /// <summary>元数据定义</summary>
        public static KMDConfiguration MetadataDefinitions { get; set; }
        /// <summary>获得某个元数据定义</summary>
        private MetadataDefinition GetDefinition(string key)
        {
            return MetadataDefinitions[key];
        }
        /// <summary>获得某个元数据值</summary>
        public object GetValue(string key)
        {
            var definition = GetDefinition(key);
            switch (definition.Type)
            {
                case TagType.DateString:
                    return GetValue_Date(key);
                case TagType.StringArray:
                case TagType.Base64StringArray:
                    return GetValue_ArrayFirst(key);
                default: return GetValue_String(key);
            }
        }
        /// <summary>获得某个元数据值</summary>
        protected object GetValueInternal(string key)
        {
            object value = null;
            base.TryGetValue(key, out value);
            return value;
        }
        /// <summary>获得某个元数据值</summary>
        protected string GetValue_String(string key)
        {
            return (string)GetValueInternal(key);
        }
        /// <summary>获得某个元数据值</summary>
        protected DateTime? GetValue_Date(string key)
        {
            var value = GetValueInternal(key);
            if (value == null) return null;
            return (DateTime)value;
        }
        /// <summary>获得某个元数据值，数组第一个值</summary>
        protected string GetValue_ArrayFirst(string key)
        {
            var value = GetValueInternal(key);
            if (value == null) return null;
            return value.As<IList<string>>().FirstOrDefault();
        }
        /// <summary>获得某个元数据值</summary>
        protected Image GetValue_Image(string key)
        {
            var value = GetValue_ArrayFirst(key);
            return value == null ? null : Image.FromStream(new MemoryStream(Convert.FromBase64String(value)));
        }
        /// <summary>设置某个元数据值</summary>
        public void SetValue(string key, object value)
        {
            if (value == null)
            {
                base.Remove(key);
                return;
            }
            var definition = GetDefinition(key);
            switch (definition.Type)
            {
                case TagType.DateString:
                    SetValue_Date(key, value);
                    break;
                case TagType.StringArray:
                case TagType.Base64StringArray:
                    SetValue_Array(key, value);
                    break;
                default:
                    base[key] = value;
                    break;
            }
        }
        /// <summary>设置某个元数据值</summary>
        protected void SetValue_Date(string key, DateTime? value)
        {
            if (!value.HasValue)
            {
                base.Remove(key);
                return;
            }
            base[key] = value;
        }
        /// <summary>设置某个元数据值</summary>
        protected void SetValue_Date(string key, object value)
        {
            var type = value.GetType();
            if (type == typeof(DateTime))
            {
                //不需要处理
            }
            else if (type == typeof(DateTime?))
            {
                var value2 = (DateTime?)value;
                if (!value2.HasValue)
                {
                    base.Remove(key);
                    return;
                }
                value = value2.Value;
            }
            else
            {
                value = value.ToString().ToISODate();
            }
            base[key] = value;
        }
        /// <summary>设置某个元数据值</summary>
        protected void SetValue_Array(string key, object value)
        {
            var value2 = value;
            if (value is IList<string>) value2 = value.As<IList<string>>().First();
            var array = (List<string>)GetValueInternal(key);
            if (array == null)
            {
                array = new List<string>();
                base.Add(key, array);
            }
            else if (array.Contains(value2.ToString(), StringComparer.OrdinalIgnoreCase))
            {
                return;//值已经存在
            }
            array.Add(value2.ToString());
        }
        /// <summary>转换为索引信息</summary>
        public string ToIndex()
        {
            return this.ToMetadata().ToJson();
        }
        /// <summary>转换为层次化元数据</summary>
        public IMetadata ToMetadata()
        {
            var metadata = new JsonMetadata();
            ToMetadata(metadata);
            return metadata;
        }
        /// <summary>转换为层次化元数据</summary>
        public void ToMetadata(JsonMetadata metadata)
        {
            foreach (var pair in this)
            {
                metadata.SetValue(pair.Key, pair.Value);
            }
        }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}
