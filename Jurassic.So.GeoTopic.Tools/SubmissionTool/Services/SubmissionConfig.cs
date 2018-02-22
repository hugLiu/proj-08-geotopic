using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;
using Jurassic.PKS.Service;
using Jurassic.PKS.WebAPI.Submission;
using Jurassic.So.GeoTopic.SubmissionTool.Views;
using Jurassic.So.Infrastructure;
using Microsoft.Practices.CompositeUI;

namespace Jurassic.So.GeoTopic.SubmissionTool.Services
{
    /// <summary>提交配置</summary>
    [XmlRoot("Submission")]
    public sealed class SubmissionConfig
    {
        /// <summary>元数据标签服务URL</summary>
        [XmlElement]
        public string MetadataTagsUrl { get; set; }
        /// <summary>元数据标签文件</summary>
        [XmlElement]
        public string MetadataTagsFile { get; set; }
        /// <summary>元数据标签配置</summary>
        [XmlArray]
        [XmlArrayItem("Tag")]
        public List<MetadataTagConfig> MetadataTags { get; set; }
        /// <summary>元数据标签配置视图</summary>
        [XmlIgnore]
        public List<MetadataTagConfig> MetadataTagsView { get; set; }
        /// <summary>元数据选项配置</summary>
        [XmlArray]
        [XmlArrayItem("Option")]
        public List<MetadataOptionConfig> MetadataOptions { get; set; }
        /// <summary>成果文件夹</summary>
        [XmlElement]
        public string PTDir { get; set; }
        /// <summary>Excel文件</summary>
        [XmlElement]
        public string ExcelFile { get; set; }
        /// <summary>提交服务URL</summary>
        [XmlElement]
        public string SubmissionUrl { get; set; }
        /// <summary>提交选项 </summary>
        [XmlElement]
        public SubmissionOptionConfig SubmissionOption { get; set; }
        /// <summary>载入方法</summary>
        public void Load()
        {
            if (this.MetadataTagsUrl == null)
            {
                this.MetadataTagsUrl = string.Empty;
            }
            if (this.MetadataTagsFile == null)
            {
                this.MetadataTagsFile = string.Empty;
            }
            if (this.PTDir == null)
            {
                this.PTDir = string.Empty;
            }
            if (this.ExcelFile == null)
            {
                var path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                var day = DateTime.Now.ToString("yyyyMMdd");
                this.ExcelFile = Path.Combine(path, $"成果元数据_{day}.xls");
            }
            if (this.MetadataTags == null)
            {
                this.MetadataTags = new List<MetadataTagConfig>();
            }
            if (this.MetadataOptions == null)
            {
                this.MetadataOptions = new List<MetadataOptionConfig>();
            }
            if (this.SubmissionUrl == null)
            {
                this.SubmissionUrl = string.Empty;
            }
            if (this.SubmissionOption == null)
            {
                this.SubmissionOption = new SubmissionOptionConfig();
                this.SubmissionOption.UploadedBy = string.Empty;
                this.SubmissionOption.Contact = string.Empty;
                this.SubmissionOption.Application = string.Empty;
                this.SubmissionOption.Task = string.Empty;
            }
        }
        /// <summary>生成方法</summary>
        public override string ToString()
        {
            return this.ToJsonString();
        }
    }

    /// <summary>元数据标签配置</summary>
    [XmlRoot("Tag")]
    public sealed class MetadataTagConfig
    {
        /// <summary>名称</summary>
        [XmlElement]
        [DisplayName("名称")]
        [ReadOnly(true)]
        public string Name { get; set; }
        /// <summary>枚举值集合</summary>
        [XmlIgnore]
        public MetadataDefinition Refer { get; set; }
        /// <summary>标题</summary>
        [XmlIgnore]
        [DisplayName("标题")]
        [ReadOnly(true)]
        public string Title { get { return this.Refer.Title; } }
        /// <summary>启用</summary>
        [XmlElement]
        [DisplayName("可见性")]
        public bool Enabled { get; set; }
        /// <summary>宽度</summary>
        [XmlElement]
        [DisplayName("宽度")]
        public int Width { get; set; }
        /// <summary>枚举值集合</summary>
        [XmlElement]
        [DisplayName("枚举值")]
        public string EnumValues { get; set; }
        /// <summary>枚举值集合</summary>
        public string[] GetEnumValues()
        {
            return this.EnumValues.Split(',');
        }
        /// <summary>生成方法</summary>
        public override string ToString()
        {
            return this.ToJsonString();
        }
    }

    /// <summary>元数据选项配置</summary>
    [XmlRoot("Option")]
    public sealed class MetadataOptionConfig
    {
        /// <summary>名称</summary>
        [XmlElement]
        [DisplayName("名称")]
        public string Name { get; set; }
        /// <summary>宽度</summary>
        [XmlElement]
        [DisplayName("宽度")]
        public int Width { get; set; }
        /// <summary>生成方法</summary>
        public override string ToString()
        {
            return this.ToJsonString();
        }
    }

    /// <summary>Excel单元属性</summary>
    public class CellProperty
    {
        /// <summary>值</summary>
        public object Value { get; set; }
        /// <summary>列跨度</summary>
        public int Span { get; set; }
        /// <summary>列宽度</summary>
        public int Width { get; set; }
        /// <summary>列宽度</summary>
        public int[] Widths { get; set; }
        /// <summary>生成方法</summary>
        public override string ToString()
        {
            return this.ToJsonString();
        }
    }
    /// <summary>提交选项配置</summary>
    public class SubmissionOptionConfig
    {
        /// <summary>是否为审核状态</summary>
        [XmlElement]
        public bool Authentic { get; set; }
        /// <summary>是否需要调用元数据自动补全功能</summary>
        [XmlElement]
        public bool AutoComplement { get; set; }
        /// <summary>上传人</summary>
        [XmlElement]
        public string UploadedBy { get; set; }
        /// <summary>
        /// 联系方式。
        /// </summary>
        /// <remarks>
        /// 可以提供多个联系方式，使用“|”分割。如”email:z3@a.com | qq:12345 | wc:3214 | tel: 18601234567”
        /// </remarks>
        [XmlElement]
        public string Contact { get; set; }
        /// <summary>
        /// 提交源系统名称。（从哪个系统提交的，如油田规划应用）
        /// </summary>
        [XmlElement]
        public string Application { get; set; }

        /// <summary>
        /// 提交的任务名称。（做什么任务的时候提交的，如资源建设、评价总结）
        /// </summary>
        [XmlElement]
        public string Task { get; set; }
    }

    /// <summary>进度属性</summary>
    public class ProgressProperty
    {
        /// <summary>消息</summary>
        public string Message { get; set; }
        /// <summary>最大值</summary>
        public int MaxValue { get; set; }
        /// <summary>内容</summary>
        public string Content { get; set; }
        /// <summary>克隆</summary>
        public ProgressProperty Clone()
        {
            var clone = this.MemberwiseClone();
            return (ProgressProperty)clone;
        }
        /// <summary>生成方法</summary>
        public override string ToString()
        {
            return this.ToJsonString();
        }
    }

    /// <summary>提交上下文</summary>
    public class SubmitContext
    {
        /// <summary>视图</summary>
        public IChildView View { get; set; }
        /// <summary>工作项</summary>
        public WorkItem WorkItem { get; set; }
        /// <summary>进度属性</summary>
        public ProgressProperty Progress { get; set; }
        /// <summary>进度值</summary>
        public volatile int ProgressValue;
        /// <summary>获得下一个进度值</summary>
        public int GetNextProgressValue()
        {
            return Interlocked.Increment(ref ProgressValue);
        }
        /// <summary>后台工人</summary>
        public BackgroundWorker Worker { get; set; }
        /// <summary>提交集合</summary>
        public ConcurrentQueue<SubmitProduct> Values { get; set; }
        /// <summary>失败集合</summary>
        public ConcurrentQueue<SubmitProduct> FailureValues { get; set; }
        /// <summary>API包装提交服务</summary>
        public ApiWrappedSubmissionService ApiSubmissionService { get; set; }
        /// <summary>移动</summary>
        public void Move()
        {
            while (this.FailureValues.Count > 0)
            {
                var value = default(SubmitProduct);
                if (!this.FailureValues.TryDequeue(out value)) continue;
                this.Values.Enqueue(value);
            }
        }
    }

    /// <summary>提交成果</summary>
    public class SubmitProduct
    {
        /// <summary>ID</summary>
        public int ID { get; set; }
        /// <summary>文件集合</summary>
        public List<SubmitProductFile> Files { get; set; }
        /// <summary>提交信息</summary>
        public SubmissionInfoRequest Info { get; set; }
    }

    /// <summary>提交成果</summary>
    public class SubmitProductFile
    {
        /// <summary>路径</summary>
        public string Path { get; set; }
        /// <summary>文件</summary>
        public string File { get; set; }
        /// <summary>绑定列表项</summary>
        public ListViewItem ListViewItem { get; set; }
    }
}
