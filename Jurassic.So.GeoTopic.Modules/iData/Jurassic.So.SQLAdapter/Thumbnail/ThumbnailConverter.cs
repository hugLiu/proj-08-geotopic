using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Jurassic.PKS.Service;
using Jurassic.So.Business;
using Jurassic.So.ETL;
using Jurassic.So.Infrastructure;
using Jurassic.So.Infrastructure.Logging;

namespace Jurassic.So.Adapter
{
    /// <summary>缩略图转换器</summary>
    [ETLComponent("Thumbnail", Title = "缩略图转换器", Description = "转换源列的值中到缩略图并设置到目标列！")]
    public class ThumbnailConverter : ETLConverter
    {
        /// <summary>尺寸</summary>
        private Size ThumbnailSize { get; set; }
        /// <summary>GDB缩略图生成器</summary>
        private GDBThumbnailBuilder GDBThumbnailBuilder { get; set; }
        /// <summary>执行转换</summary>
        public override void Execute(ETLExecuteContext context, IETLRow input, IETLRow output, object inputParameter)
        {
            var format = GetFormat(context, input, output, inputParameter);
            if (!format.HasValue) return;
            var stream = GetContent(context, input, output, inputParameter);
            if (stream == null || stream.Length == 0) return;
            var thumbnail = CreateThumbnail(format.Value, stream);
            if (thumbnail == null) return;
            var outputValue = Convert.ToBase64String(thumbnail.ToByteArray());
            this.DefaultOutputColumn.SetValue(context, output, inputParameter, outputValue);
        }
        /// <summary>获得格式</summary>
        private DataFormat? GetFormat(ETLExecuteContext context, IETLRow input, IETLRow output, object inputParameter)
        {
            var formatValue = this.InputColumns[0].GetValue(context, input, output, inputParameter);
            if (!ValidatePolicy(formatValue)) return null;
            return formatValue?.ToString().ToDataFormat();
        }
        /// <summary>获得内容</summary>
        private Stream GetContent(ETLExecuteContext context, IETLRow input, IETLRow output, object inputParameter)
        {
            var contentValue = this.InputColumns[1].GetValue(context, input, output, inputParameter);
            if (!ValidatePolicy(contentValue)) return null;
            if (contentValue == null) return null;
            var stream = contentValue.As<Stream>();
            if (stream != null) return stream;
            var content = contentValue.As<byte[]>();
            if (content == null) return null;
            return new MemoryStream(content);
        }
        /// <summary>生成缩略图</summary>
        private Stream CreateThumbnail(DataFormat format, Stream stream)
        {
            try
            {
                IThumbnailBuilder builder;
                switch (format)
                {
                    case DataFormat.JPG:
                    case DataFormat.PNG:
                    case DataFormat.TIF:
                    case DataFormat.BMP:
                        builder = new ImageThumbnailBuilder();
                        break;
                    //case DataFormat.DOC:
                    //case DataFormat.DOCX:
                    //    builder = new WordThumbnailBuilder();
                    //    break;
                    //case DataFormat.XLS:
                    //case DataFormat.XLSX:
                    //    builder = new WordThumbnailBuilder();
                    //    break;
                    //case DataFormat.PDF:
                    //    builder = new WordThumbnailBuilder();
                    //    break;
                    case DataFormat.GDB:
                        lock (this)
                        {
                            if (this.GDBThumbnailBuilder == null)
                            {
                                this.GDBThumbnailBuilder = new GDBThumbnailBuilder();
                            }
                            builder = this.GDBThumbnailBuilder;
                        }
                        break;
                    default:
                        return null;
                }
                return builder.Build(stream, this.ThumbnailSize);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }
        /// <summary>完成执行</summary>
        public override void Finish(ETLExecuteContext context)
        {

        }

        #region 配置方法
        /// <summary>加载</summary>
        public override void LoadXml(ETLXmlConfiguration config, XElement node)
        {
            base.LoadXml(config, node);
            var values = this.Pattern.Split(',');
            this.ThumbnailSize = new Size(values[0].ToInt32(), values[1].ToInt32());
        }
        #endregion

    }
}
