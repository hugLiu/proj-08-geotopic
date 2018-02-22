using System.IO;
using System.Threading.Tasks;

namespace Jurassic.Submission.Abstract
{
    /// <summary>
    /// 成果提交服务
    /// </summary>
    public interface ISubmission
    {
        /// <summary>
        /// 上传一个成果文件到目标系统中(以异步的方式)
        /// </summary>
        /// <param name="file">待上传的文件</param>
        /// <returns></returns>
        Task<string> UploadAsync(FileInfo file);

        /// <summary>
        /// 提交成果的文件集和元数据集到目标系统中(以异步的方式)
        /// </summary>
        /// <param name="info">提交信息</param>
        /// <returns></returns>
        Task<string> SubmitAsync(SubmissionInfo info);

        /// <summary>
        /// 上传一个成果文件到目标系统中
        /// </summary>
        /// <param name="file">待上传的文件</param>
        /// <returns>文件ID</returns>
        string Upload(FileInfo file);

        /// <summary>
        /// 提交成果的文件集和元数据集到目标系统中
        /// </summary>
        /// <param name="info">提交信息</param>
        /// <returns>成果ID</returns>
        string Submit(SubmissionInfo info);
    }
}