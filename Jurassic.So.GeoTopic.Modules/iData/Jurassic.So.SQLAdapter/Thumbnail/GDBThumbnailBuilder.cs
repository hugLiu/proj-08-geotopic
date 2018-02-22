using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Jurassic.So.Infrastructure;
using Jurassic.So.Infrastructure.Logging;

namespace Jurassic.So.Adapter
{
    /// <summary>
    /// 针对GDB文件生成缩略图
    /// </summary>
    public class GDBThumbnailBuilder : IThumbnailBuilder
    {
        /// <summary>
        /// 生成图片缩略图的二进制数据
        /// </summary>
        public Stream Build(Stream inputStream, Size size)
        {
            var files = new List<string>();
            try
            {
                var tempUri = new Uri(Assembly.GetExecutingAssembly().CodeBase);
                var tempPath = Path.GetDirectoryName(tempUri.LocalPath);
                var gdbFile = Path.Combine(tempPath, Guid.NewGuid().ToString() + ".gdb");
                var bmpFile = Path.Combine(tempPath, Guid.NewGuid().ToString() + ".bmp");
                File.WriteAllBytes(gdbFile, inputStream.ToByteArray());
                files.Add(gdbFile);
                var thread = new Thread(ConvertGDBToBmp);
                thread.SetApartmentState(ApartmentState.STA);
                var state = new string[] { gdbFile, bmpFile };
                thread.Start(state);
                thread.Join();
                if (!File.Exists(bmpFile)) return null;
                files.Add(bmpFile);
                using (var outputStream = new FileStream(bmpFile, FileMode.Open))
                {
                    var builder = new ImageThumbnailBuilder();
                    return builder.Build(outputStream, size);
                }
            }
            finally
            {
                files.ForEach(File.Delete);
            }
        }
        /// <summary>GDB转换为图片</summary>
        private void ConvertGDBToBmp(object state)
        {
            var files = state.As<string[]>();
            var gdbFile = files[0];
            var bmpFile = files[1];
            frmGDBToBmp form = null;
            try
            {
                form = new frmGDBToBmp();
                var joGis = form.axJoGIS4;
                var isok = joGis.LoadMapFile(gdbFile);
                if (!isok) return;
                joGis.ExportBMP(bmpFile);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            finally
            {
                if (form != null)
                {
                    form.axJoGIS4.Dispose();
                    form.Dispose();
                }
            }
        }
    }
}
