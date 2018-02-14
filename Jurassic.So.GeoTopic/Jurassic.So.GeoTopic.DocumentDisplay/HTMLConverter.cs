using Aspose.Cells;
using Aspose.Words.Saving;
using Jurassic.PKS.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.GeoTopic.DocumentConverter
{
    public class HTMLConverter
    {
        private Stream fileStream = null;
        private DataFormat format;

        public HTMLConverter(Stream fileStream, DataFormat format)
        {
            this.fileStream = fileStream;
            this.format = format;
        }

        private Stream WordToHtml()
        {
            Aspose.Words.Document word = new Aspose.Words.Document(this.fileStream);
            using (MemoryStream stream = new MemoryStream())
            {
                Aspose.Words.Saving.HtmlSaveOptions options = new Aspose.Words.Saving.HtmlSaveOptions(Aspose.Words.SaveFormat.Html);
                options.ImageSavingCallback = new HandleImageSaving();
                word.Save(stream, options);
                stream.Position = 0;
                return stream;
            }  
        }

        public class HandleImageSaving : IImageSavingCallback
        {
            void IImageSavingCallback.ImageSaving(ImageSavingArgs e)
            {
                e.ImageStream = new MemoryStream();
                e.KeepImageStreamOpen = false;
            }
        }

        private Stream ExcelToHtml()
        {
            Aspose.Cells.Workbook excel = new Aspose.Cells.Workbook(this.fileStream);
            using (MemoryStream stream = new MemoryStream())
            {
                excel.Save(stream, Aspose.Cells.SaveFormat.Html);
                stream.Position = 0;
                return stream;
            }
        }

        private Stream PPTToHtml()
        {
            using (Aspose.Slides.Pptx.PresentationEx ppt = new Aspose.Slides.Pptx.PresentationEx(this.fileStream))
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    ppt.Save(stream, Aspose.Slides.Export.SaveFormat.Html);
                    stream.Position = 0;
                    return stream;
                }
            }
        }

        public Stream GetConvertedStream()
        {
            Stream stream = null;

            switch (this.format)
            {
                case DataFormat.DOC:
                case DataFormat.DOCX:
                    stream = this.WordToHtml();
                    break;
                case DataFormat.XLS:
                case DataFormat.XLSX:
                    stream = this.ExcelToHtml();
                    break;
                case DataFormat.PPT:
                case DataFormat.PPTX:
                    stream = this.PPTToHtml();
                    break;
                default:
                    stream = this.fileStream;
                    break;

            }

            return stream;
        }

    }
}
