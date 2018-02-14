using Jurassic.ServiceModels;
using System.Collections.Generic;

namespace Jurassic.Submission
{
    /// <summary>
    /// 提交信息
    /// </summary>
    public class SubmissionInfo
    {
        /// <summary>
        /// 提交命令类型
        /// </summary>
        public SubmissionAction Action { get; set; }

        /// <summary>
        /// 成果的唯一标记Key
        /// </summary>
        public string NatureKey { get; set; }

        /// <summary>
        /// 成果文件ID集合
        /// </summary>
        public List<string> FilelDs { get; set; }

        /// <summary>
        /// 元数据集合，遵循元数据规范
        /// </summary>
        public IMetadata KMD { get; set; }

        /// <summary>
        /// 选项参数
        /// </summary>
        public SubmissionOption Option { get; set; }
    }
}