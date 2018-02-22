namespace Jurassic.So.SpiderTool.IService.Processers
{
    public enum ProcessStatus
    {
        /// <summary>
        /// 就绪
        /// </summary>
        Ready,
        /// <summary>
        /// 准备中
        /// </summary>
        Preparing,
        /// <summary>
        /// 执行中
        /// </summary>
        Running,
        /// <summary>
        /// 成功
        /// </summary>
        Successed,
        /// <summary>
        /// 失败
        /// </summary>
        Failed
    }
}