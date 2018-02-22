namespace Jurassic.So.Index
{
    /// <summary>服务模拟配置接口</summary>
    public interface IServiceMockConfig
    {
        /// <summary>根路径</summary>
        string RootPath { get; }
        /// <summary>模拟数据存取相对路径</summary>
        string DataDir { get; }
        /// <summary>模拟数据存取路径</summary>
        string DataPath { get; }
    }
}
