using Aspose.Cells;
using Jurassic.PKS.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.GeoTopic.DocumentConverter
{
    public class PdfConverter
    {
        private Stream fileStream = null;
        private DataFormat format;

        public PdfConverter(Stream fileStream, DataFormat format)
        {
            this.fileStream = fileStream;
            this.format = format;
        }

        private Stream WordToPdf()
        {
            Aspose.Words.Document word = new Aspose.Words.Document(this.fileStream);
            using (MemoryStream stream = new MemoryStream())
            {
                Aspose.Words.Saving.PdfSaveOptions options = new Aspose.Words.Saving.PdfSaveOptions();
                options.UseHighQualityRendering = true;
                //options.Compliance = Aspose.Words.Saving.PdfCompliance.PdfA1b;
                word.Save(stream, options);
                stream.Position = 0;
                return stream;
            }
        }

        private Stream ExcelToPdf()
        {
            Aspose.Cells.Workbook excel = new Aspose.Cells.Workbook(this.fileStream);
            using (MemoryStream stream = new MemoryStream())
            {
                PdfSaveOptions options = new PdfSaveOptions(Aspose.Cells.SaveFormat.Pdf) { AllColumnsInOnePagePerSheet = true };
                options.OnePagePerSheet = true;
                options.SecurityOptions = new Aspose.Cells.Rendering.PdfSecurity.PdfSecurityOptions();
                options.SecurityOptions.PrintPermission = false;
                options.SecurityOptions.ModifyDocumentPermission = false;
                options.SecurityOptions.AssembleDocumentPermission = false;
                options.SecurityOptions.ExtractContentPermission = false;
                options.SecurityOptions.ExtractContentPermissionObsolete = false;
                excel.Save(stream, options);
                stream.Position = 0;
                return stream;
            }
        }

        private Stream PPTToPdf()
        {
            using (Aspose.Slides.Pptx.PresentationEx ppt = new Aspose.Slides.Pptx.PresentationEx(this.fileStream))
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    ppt.Save(stream, Aspose.Slides.Export.SaveFormat.Pdf);
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
                    stream = this.WordToPdf();
                    break;
                case DataFormat.XLS:
                case DataFormat.XLSX:
                    stream = this.ExcelToPdf();
                    break;
                case DataFormat.PPT:
                case DataFormat.PPTX:
                    stream = this.PPTToPdf();
                    break;
                default:
                    stream = this.fileStream;
                    break;

            }

            return stream;
        }
    }
}
