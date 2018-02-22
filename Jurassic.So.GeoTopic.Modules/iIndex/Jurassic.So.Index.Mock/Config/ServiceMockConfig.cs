using Jurassic.So.Infrastructure;
using System.Web;
using System.IO;

namespace Jurassic.So.Index
{
    /// <summary>服务模拟配置接口</summary>
    public sealed class ServiceMockConfig : IServiceMockConfig
    {
        /// <summary>构造函数</summary>
        public ServiceMockConfig()
        {
            var context = HttpContext.Current;
            if (context == null) return;
            Set(context.Server.MapPath(@"\"));
        }
        /// <summary>设置</summary>
        public void Set(string rootPath)
        {
            this.RootPath = rootPath;
            this.DataPath = this.RootPath + this.DataDir + @"\";
            if (!Directory.Exists(this.DataPath)) Directory.CreateDirectory(this.DataPath);
        }
        /// <summary>根路径</summary>
        public string RootPath { get; private set; }
        /// <summary>模拟数据存取相对路径</summary>
        public string DataDir { get { return "MockServicesData"; } }
        /// <summary>模拟数据存取路径</summary>
        public string DataPath { get; private set; }
    }
}
