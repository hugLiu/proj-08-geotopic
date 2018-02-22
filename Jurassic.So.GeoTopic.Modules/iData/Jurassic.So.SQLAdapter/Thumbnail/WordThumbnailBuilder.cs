using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using Aspose.Words;

namespace Jurassic.So.Adapter
{
    /// <summary>
    /// 针对WORD文档，生成缩略图
    /// </summary>
    public class WordThumbnailBuilder : IThumbnailBuilder
    {
        /// <summary>
        /// 生成图片缩略图的二进制数据
        /// </summary>
        public Stream Build(Stream inputStream, Size size)
        {
            var document = new Document(inputStream);
            using (var bmp = new Bitmap(size.Width, size.Height))
            {
                using (var g = Graphics.FromImage(bmp))
                {
                    // 用白色清空
                    g.Clear(Color.White);
                    // 指定高质量的双三次插值法。执行预筛选以确保高质量的收缩。此模式可产生质量最高的转换图像。
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    // 指定高质量、低速度呈现。
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                    g.PageUnit = GraphicsUnit.Pixel;
                    document.RenderToSize(0, g, 0, 0, size.Width, size.Height);
                    var outputStream = new MemoryStream();
                    bmp.Save(outputStream, ImageFormat.Png);
                    return outputStream;
                }
            }
        }
    }
}
