using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Data;
using Aspose.Cells;
using System.Threading.Tasks;
using System.IO;

namespace DocumentDisplay
{
    public class DocumentConvert
    {
        //Aspose对象
        private static Aspose.Pdf.Document pdfDoc;
        private static Aspose.Words.Document wordsDoc;
        private static Aspose.Cells.Workbook cellsDoc;
        private static Aspose.Slides.Pptx.PresentationEx pptx;
        //private string msg;

        /// <summary>
        /// doc, docx转为html
        /// </summary>
        /// <returns></returns>
        public string DocToHtml(string filePath, string savePath)
        {
                try
                {
                    wordsDoc = new Aspose.Words.Document(filePath);
                    wordsDoc.Save(savePath, Aspose.Words.SaveFormat.Html);
                }
                catch (Exception)
                {
                    if (File.Exists(savePath)) //转化失败，则删除垃圾文件
                        File.Delete(savePath);
                }
            return savePath;
        }

        public string DocToHtml(Stream stream, string savePath,string format)
        {
            try
            {
                //wordsDoc = new Aspose.Words.Document(filePath);
                var result= wordsDoc.Save(stream, Aspose.Words.SaveFormat.Html);
            }
            catch (Exception)
            {
                if (File.Exists(savePath)) //转化失败，则删除垃圾文件
                    File.Delete(savePath);
            }
            return savePath;
        }


        /// <summary>
        /// ppt , pptx转html
        /// </summary>
        /// <param name="format"></param>
        /// <param name="iiid"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public string PptToHtml(string filePath, string savePath)
        {
            try
            {
                pptx = new Aspose.Slides.Pptx.PresentationEx(filePath);
                pptx.Save(savePath, Aspose.Slides.Export.SaveFormat.Html);
                return savePath;
            }
            catch (Exception)  //若没有转化成功 需要删除已生成的html文件,前台报错
            {
                return "转html失败！";
            }
        }

        /// <summary>
        /// ppt , pptx转html
        /// </summary>
        /// <param name="format"></param>
        /// <param name="iiid"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public string PptToHtml(Stream stream, string savePath)
        {
            try
            {
               pptx = new Aspose.Slides.Pptx.PresentationEx(stream);
               pptx.Save(savePath, Aspose.Slides.Export.SaveFormat.Html);

                return savePath;
            }
            catch (Exception ex)  //若没有转化成功 需要删除已生成的html文件,前台报错
            {
                if (File.Exists(savePath))
                    File.Delete(savePath);
                return "转html失败！";
            }
        }

        /// <summary>
        /// xls,xlsx转html
        /// </summary>
        /// <param name="format"></param>
        /// <param name="iiid"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public string XlsToHtml(string filePath, string savePath)
        {
            try
            {
                cellsDoc = new Aspose.Cells.Workbook(filePath);
                cellsDoc.Save(savePath, Aspose.Cells.SaveFormat.Html);
               // return "/ResourcesFiles/" + format + "/" + iiid + "/" + iiid + ".html";
            }
            catch (Exception)
            {
                if (File.Exists(savePath))
                    File.Delete(savePath);
                return "转thml失败！";
            }
            return "";
        }

        /// <summary>
        /// 转化图片为Jpeg格式 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="newPath"></param>
        public void ConvertImg(string filePath, string savePath)
        {
            Bitmap origBitmap = new Bitmap(filePath);
            Bitmap outputImage = new Bitmap(origBitmap);
            outputImage.Save(savePath, System.Drawing.Imaging.ImageFormat.Jpeg);
            outputImage.Dispose();
            origBitmap.Dispose();
        }

        /// <summary>
        /// 转化图片为Jpeg格式 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="newPath"></param>
        public void ConvertImg(Stream stream)
        {
            Bitmap origBitmap = new Bitmap(stream);
            Bitmap outputImage = new Bitmap(origBitmap);
            outputImage.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
            outputImage.Dispose();
            origBitmap.Dispose();
        }

        /// <summary>
        /// datatable转html 样式基于bootsrap
        /// </summary>
        /// <param name="ds">DataSet对象</param>
        /// <param name="saveFilePath">文件导出路径</param>
        /// <returns>布尔类型</returns>
        public bool ExportHtml(DataTable dt, string saveFilePath)
        {
            if (dt.Rows.Count <= 0) throw new Exception("DataSet为空");
            bool result = false;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Remove(0, sb.Length);
            try
            {
                sb.Append("<html><head><title></title>");
                sb.Append("<style> table td {  word-break: keep-all; white-space: nowrap; }");
                sb.Append("table th {text-align: center; word-break: keep-all; white-space: nowrap; }</style></head>");
                sb.Append("<body><table class=\"table table-bordered\"><thead><tr class=\"active\">");

                foreach (DataColumn col in dt.Columns)
                {
                    sb.AppendFormat("<th>{0}</th>", col.Caption);
                }
                sb.Append("</tr> </thead>");
                foreach (DataRow row in dt.Rows)
                {
                    sb.Append("<tr scope=\"row\">");
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        sb.AppendFormat("<td>{0}</td>", row[i].ToString());
                    }
                    sb.Append("</tr>");
                }
                sb.Append("</table></body></html>");
                if (File.Exists(saveFilePath))
                {
                    File.Delete(saveFilePath);
                }
                FileStream myFs = new FileStream(saveFilePath, FileMode.Create);
                myFs.Close();
                using (StreamWriter sw = new StreamWriter(saveFilePath, false, System.Text.Encoding.UTF8))
                {
                    sw.WriteLine(sb);
                    sw.Close();
                    result = true;
                }
            }
            catch (Exception)
            {
                sb = null;
            }
            return result;
        }

    }
}
