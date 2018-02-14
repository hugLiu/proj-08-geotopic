using System;

namespace Jurassic.Submission
{

    /// <summary>
    /// 提交选项参数
    /// </summary>
    public class SubmissionOption
    {
        /// <summary>
        /// 是否为审核状态
        /// </summary>
        public bool Authentic { get; set; }

        /// <summary>
        /// 是否需要调用元数据自动补全功能
        /// </summary>
        public bool AutoComplement { get; set; }

        /// <summary>
        /// 上传人
        /// </summary>
        public string UploadedBy { get; set; }

        /// <summary>
        /// 上传时间
        /// </summary>
        public DateTime UploadedDate { get; set; }

        /// <summary>
        /// 联系方式。
        /// </summary>
        /// <remarks>
        /// 可以提供多个联系方式，使用“|”分割。如”email:z3@a.com | qq:12345 | wc:3214 | tel: 18601234567”
        /// </remarks>
        public string Contact { get; set; }

        /// <summary>
        /// 提交源系统名称。（从哪个系统提交的，如油田规划应用）
        /// </summary>
        public string Application { get; set; }

        /// <summary>
        /// 提交的任务名称。（做什么任务的时候提交的，如资源建设、评价总结）
        /// </summary>
        public string Task { get; set; }

    }
}