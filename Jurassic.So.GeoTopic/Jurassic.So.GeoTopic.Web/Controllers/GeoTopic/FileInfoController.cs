using Jurassic.PKS.Service;
using Jurassic.So.Business;
using Jurassic.So.GeoTopic.DocumentConverter;
using Jurassic.So.Infrastructure;
using Jurassic.WebFrame;
using System.IO;
using System.Web.Mvc;
using Jurassic.So.GeoTopic.DataService.Models;
using System.Collections.Generic;

namespace Jurassic.So.GeoTopic.Web.Controllers.GeoTopic
{
    /// <summary>
    /// 
    /// </summary>
    public class FileInfoController : GTBaseController
    {
        private static readonly WebServiceController Services = new WebServiceController();

        /// <summary>
        /// 页面中获取实体文件的方法
        /// </summary>
        /// <param name="url"></param>
        /// <param name="ticket"></param>
        /// <returns></returns>
        public ActionResult GetStream(string url, string ticket)
        {
            url = System.Web.HttpUtility.UrlDecode(url);
            ticket = System.Web.HttpUtility.UrlDecode(ticket);
            ////DocumentCookie docu = new DocumentCookie();
            //DataFormat dataFormat = (DataFormat)System.Enum.Parse(typeof(DataFormat), format.ToUpper());
            var result = Services.GetDataEntity(url, ticket);
            if (!(result is FileStreamResult)) return result;
            //if (System.Enum.IsDefined(typeof(Data.ConvertFormat), format.ToUpper()))
            //{
            //    var data = result.As<FileStreamResult>().FileStream;
            //    //var stream = docu.DocumentManage(CreateMD5Hash(url + ticket), format, data);
            //    //result = File(stream, DataFormat.HTML.ToMimeTypeValue());

            //}

            var fileStream = result.As<FileStreamResult>();//.FileStream;
            var dataFormat = fileStream.ContentType.ToDataFormatFromMime();
            switch (dataFormat)
            {
                case DataFormat.DOC:
                case DataFormat.DOCX:
                //PdfConverter pdfConverter = new PdfConverter(stream, dataFormat);
                //MemoryStream pdfStream = pdfConverter.GetConvertedStream() as MemoryStream;
                //byte[] pdfContents = pdfStream.GetBuffer();
                //result = File(pdfContents, DataFormat.PDF.ToMimeTypeValue());
                //break;
                case DataFormat.XLS:
                case DataFormat.XLSX:
                case DataFormat.PPT:
                case DataFormat.PPTX:
                    var pdfConverter = new PdfConverter(fileStream.FileStream, dataFormat);
                    var pdfStream = pdfConverter.GetConvertedStream() as MemoryStream;
                    if (pdfStream != null)
                    {
                        var pdfContents = pdfStream.GetBuffer();
                        result = File(pdfContents, DataFormat.PDF.ToMimeTypeValue());
                    }
                    //HTMLConverter htmlConverter = new HTMLConverter(stream, dataFormat);
                    //MemoryStream htmlStream = htmlConverter.GetConvertedStream() as MemoryStream;
                    //byte[] htmlContents = htmlStream.GetBuffer();
                    //result = File(htmlContents, DataFormat.HTML.ToMimeTypeValue());
                    break;
                case DataFormat.TIF:
                    var imageConverter = new ImageConverter(fileStream.FileStream, dataFormat);
                    var imageStream = imageConverter.GetConvertedStream() as MemoryStream;
                    if (imageStream != null)
                    {
                        var imageContents = imageStream.GetBuffer();
                        result = File(imageContents, DataFormat.JPG.ToMimeTypeValue());
                    }
                    break;

            }
            return result;
        }

        /// <summary>
        /// 下载方法
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="url"></param>
        /// <param name="ticket"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public ActionResult DownLoad(string id, string title, string url, string ticket, string name)
        {
            title = System.Web.HttpUtility.UrlDecode(title ?? "");
            url = System.Web.HttpUtility.UrlDecode(url);
            ticket = System.Web.HttpUtility.UrlDecode(ticket);
            //format = System.Web.HttpUtility.UrlDecode(format);
            name = System.Web.HttpUtility.UrlDecode(name);
            var content = new Dictionary<string, string>();
            content["id"] = id;
            content["title"] = title;
            content["url"] = url;
            content["ticket"] = ticket;
            content["name"] = name;
            AddUserBehavior(UserBehaviorType.Download, name, content);
            //DataFormat fileFormat = (DataFormat)System.Enum.Parse(typeof(DataFormat), format.ToUpper());
            return Services.GetDataEntity(url, ticket, name);
        }

    }
}