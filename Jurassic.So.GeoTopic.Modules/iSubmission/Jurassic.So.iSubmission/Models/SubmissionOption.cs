using System;

namespace Jurassic.Submission.Abstract
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
        /// 联系方式
        /// </summary>
        public string Contact { get; set; }

    }
}