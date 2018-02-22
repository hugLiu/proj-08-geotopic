using System.Threading.Tasks;
using Jurassic.So.Infrastructure;

namespace Jurassic.So.Index
{
    /// <summary>索引服务配置接口</summary>
    public interface IIndexerConfig
    {
        /// <summary>发送方法URL</summary>
        string SendIndexUrl { get; }
    }
}
