using Jurassic.PKS.Service;
using Jurassic.So.Business;
using Jurassic.So.Infrastructure;

namespace Jurassic.So.Adapter
{
    /// <summary>元数据标签</summary>
    public class MetadataTag : MetadataDefinition
    {
        /// <summary>构造函数</summary>
        public MetadataTag()
        {
            this.Enabled = true;
        }
        /// <summary>是否可用，默认可用</summary>
        public bool Enabled { get; set; }
    }
}
