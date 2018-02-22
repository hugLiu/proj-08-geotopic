using Infragistics.Win.Misc;
using Infragistics.Win.UltraWinToolbars;
using Jurassic.AppCenter.SmartClient.Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jurassic.So.GeoTopic.SubmissionTool.Services;

namespace Jurassic.So.GeoTopic.SubmissionTool.Views
{
    public interface IChildView
    {
        /// <summary>元数据标签服务URL</summary>
        string MetadataTagsUrl { get; set; }
        /// <summary>元数据标签文件</summary>
        string MetadataTagsFile { get; set; }
        /// <summary>是否只显示可用的元数据标签</summary>
        bool MetadataTagsOnlyShowEnabled { get; set; }
        /// <summary>绑定元数据标签</summary>
        void BindMetadataTags(object dataSource);
        /// <summary>成果文件夹</summary>
        string PTDir { get; set; }
        /// <summary>Excel文件</summary>
        string ExcelFile { get; set; }
        /// <summary>提交服务URL</summary>
        string SubmissionUrl { get; set; }
        /// <summary>是否为审核状态</summary>
        bool SubmitAuthentic { get; set; }
        /// <summary>是否需要调用元数据自动补全功能</summary>
        bool SubmitAutoComplement { get; set; }
        /// <summary>上传人</summary>
        string SubmitUploadedBy { get; set; }
        /// <summary>
        /// 联系方式。
        /// </summary>
        /// <remarks>
        /// 可以提供多个联系方式，使用“|”分割。如”email:z3@a.com | qq:12345 | wc:3214 | tel: 18601234567”
        /// </remarks>
        string SubmitContact { get; set; }
        /// <summary>
        /// 提交源系统名称。（从哪个系统提交的，如油田规划应用）
        /// </summary>
        string SubmitApplication { get; set; }
        /// <summary>
        /// 提交的任务名称。（做什么任务的时候提交的，如资源建设、评价总结）
        /// </summary>
        string SubmitTask { get; set; }
        /// <summary>显示正在提交的产品</summary>
        void ShowProducts(SubmitProduct[] products);
        /// <summary>刷新产品状态</summary>
        void RefreshProductStatus(SubmitProduct product, string status);
    }
}
