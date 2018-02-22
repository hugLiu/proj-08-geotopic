using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using Jurassic.So.Infrastructure;

namespace Jurassic.So.Adapter
{
    /// <summary>
    /// 针对常见的位图格式(如"bmp", "png", "jpg", "jpeg", "gif", "tif")，生成缩略图
    /// </summary>
    public class ImageThumbnailBuilder : IThumbnailBuilder
    {
        /// <summary>
        /// 生成图片缩略图的二进制数据
        /// </summary>
        public Stream Build(Stream inputStream, Size size)
        {
            var image = Image.FromStream(inputStream);
            if (image.Width <= size.Width && image.Height <= size.Height)
            {
                return inputStream;
            }
            // 根据源图及欲生成的缩略图尺寸,计算缩略图的实际尺寸及其在"画布"上的位置
            var rate = Math.Max(image.Width * 1.0d / size.Width, image.Height * 1.0d / size.Height);
            var targetWidth = (int)(image.Width / rate);
            var targetHeight = (int)(image.Height / rate);
            using (var bmp = new Bitmap(targetWidth, targetHeight))
            {
                // 创建画布
                using (var g = Graphics.FromImage(bmp))
                {
                    // 用白色清空
                    g.Clear(Color.White);
                    // 指定高质量的双三次插值法。执行预筛选以确保高质量的收缩。此模式可产生质量最高的转换图像。
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    // 指定高质量、低速度呈现。
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    // 在指定位置并且按指定大小绘制指定的 Image 的指定部分。
                    g.DrawImage(image, new Rectangle(0, 0, targetWidth, targetHeight), new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
                    var outputStream = new MemoryStream();
                    bmp.Save(outputStream, ImageFormat.Png);
                    return outputStream;
                }
            }
        }
    }
}
