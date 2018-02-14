using Jurassic.PKS.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.GeoTopic.DocumentConverter
{
    public class ImageConverter
    {
        private Stream fileStream = null;
        private DataFormat format;

        public ImageConverter(Stream fileStream, DataFormat format)
        {
            this.fileStream = fileStream;
            this.format = format;
        }

        private Stream TIFToJpeg()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(this.fileStream);
                image.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                return stream;
            }

        }

        public Stream GetConvertedStream()
        {
            Stream stream = null;

            switch (this.format)
            {
                case DataFormat.TIF:
                    stream = this.TIFToJpeg();
                    break;
                default:
                    stream = this.fileStream;
                    break;

            }

            return stream;
        }
    }
}
