using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;

namespace Jurassic.ServiceModels
{
    /// <summary>元数据接口</summary>
    public interface IMetadata
    {
        /// <summary>ID，唯一标识元数据</summary>
        string IIId { get; set; }
        /// <summary>索引日期</summary>
        DateTime IndexedDate { get; set; }
        /// <summary>适配器URL，元数据源地址</summary>
        string Url { get; }
        /// <summary>标题</summary>
        string Title { get; }
        /// <summary>描述</summary>
        string Description { get; }
        /// <summary>创建者</summary>
        string Creator { get; }
        /// <summary>创建日期</summary>
        DateTime CreatedDate { get; }
        /// <summary>缩略图</summary>
        Image Thumbnail { get; }
        /// <summary>全文</summary>
        string Fulltext { get; }

        /// <summary>获得某个元数据值</summary>
        /// <param name="key">元数据名称，如source.url或者dc.title[0].type</param>
        object GetValue(string key);

        /// <summary>转换为索引信息</summary>
        string ToIndex();
    }
}
