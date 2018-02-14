using Jurassic.PKS.Service.Submission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Jurassic.So.GeoTopic.Web.Models
{
    /// <summary>
    /// 提交信息
    /// </summary>
    [Serializable]
    [DataContract]
    public class SubmissionInfoRequset
    {
        /// <summary>
        /// 提交命令类型
        /// </summary>
        [DataMember(Name = "action")]
        public SubmissionAction Action { get; set; }

        /// <summary>
        /// 成果的唯一标记Key
        /// </summary>
        [DataMember(Name = "naturekey")]
        public string NatureKey { get; set; }

        /// <summary>
        /// 成果文件ID集合
        /// </summary>
        [DataMember(Name = "fileIDs")]
        public List<string> FileIDs { get; set; }

        /// <summary>
        /// 元数据集合，遵循元数据规范
        /// </summary>
        [DataMember(Name = "kmd")]
        public object KMD { get; set; }

        /// <summary>
        /// 选项参数
        /// </summary>
        [DataMember(Name = "option")]
        public SubmissionOption Option { get; set; }
    }
}
