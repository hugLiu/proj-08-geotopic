namespace Jurassic.Submission.Abstract
{
    public enum SubmissionAction
    {
        /// <summary>
        /// 提交一个成果文件及其元数据到目标系统中
        /// </summary>
        Create,
        /// <summary>
        /// 更新成果元数据（成果文件保持不变）
        /// </summary>
        Replace,
        /// <summary>
        /// 从目标系统中删除一个成果文件及其元数据
        /// </summary>
        Delete
    }
}