using System.Drawing;
using System.IO;

namespace Jurassic.So.Adapter
{
    /// <summary>缩略图生成器</summary>
    public interface IThumbnailBuilder
    {
        /// <summary>
        /// 根据原始文件的二进制数据获取缩略图
        /// </summary>
        Stream Build(Stream content, Size size);
    }
}
