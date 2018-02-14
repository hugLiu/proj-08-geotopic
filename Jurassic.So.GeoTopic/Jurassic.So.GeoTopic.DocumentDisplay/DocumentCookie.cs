using System;
using System.IO;
using System.Net;
using System.Web;
using System.Data;
using Aspose.Cells; 
using Jurassic.So.Infrastructure;
using Jurassic.PKS.Service;
using Jurassic.So.Business;

namespace DocumentDisplay
{
    public class DocumentCookie
    {
        //项目根路径
        private static string root = HttpContext.Current.Server.MapPath("~");

        //格式文件夹路径
        private static string formatDirPath = "";

        //iiid文件夹路径
        private static string iiidDirPath = "";

        //缓存文件路径
        private static string iiidFilePath = "";

        //缓存文件对应的html路径
        private static string htmlFilePath = "";

        //返回给前端显示url路径
        private static string fileUrl = "";

        private static DocumentConvert convert = new DocumentConvert();

        public FileStream DocumentManage(string iiid, string format, object data )
        {
            if (convert == null)
            {
                convert = new DocumentConvert();
            }

            iiidFilePath = root + "ResourcesFiles\\" + iiid + "." + format;
            htmlFilePath = root + "ResourcesFiles\\" + iiid + "." + "html";
            if (!Directory.Exists(root + "ResourcesFiles"))
            {
                Directory.CreateDirectory(root + "ResourcesFiles");
            }
            try
            {
                using (var file = new FileStream(iiidFilePath, FileMode.Create))
                {
                    data.As<Stream>().Position = 0;
                    data.As<Stream>().CopyTo(file);
                }
            }
            catch (Exception ex)
            {

                throw (ex);
            }
            DataFormat fileFormat = (DataFormat)System.Enum.Parse(typeof(DataFormat), format.ToUpper());
            switch (fileFormat)
            {
                case DataFormat.DOC:
                    convert.DocToHtml(iiidFilePath, htmlFilePath);
                    break;
                case DataFormat.DOCX:
                    convert.DocToHtml(iiidFilePath, htmlFilePath);
                    break;

                case DataFormat.XLS:
                    convert.XlsToHtml(iiidFilePath, htmlFilePath);
                    break;

                case DataFormat.XLSX:
                    convert.XlsToHtml(iiidFilePath, htmlFilePath);

                    break;

                case DataFormat.PPT:
                    convert.PptToHtml(iiidFilePath, htmlFilePath);
                    break;

                case DataFormat.PPTX:
                    convert.PptToHtml(iiidFilePath, htmlFilePath);
                    break;

                case DataFormat.TIF:
                    htmlFilePath = root + "ResourcesFiles\\" + ".jpg";
                    convert.ConvertImg(iiidFilePath, htmlFilePath);
                    break;
            }
            try
            {
                FileStream fs = new FileStream(htmlFilePath, FileMode.Open, FileAccess.Read);
                //fs.Close();
                return fs;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                //  if (File.Exists(iiidFilePath))
                //   File.Delete(iiidFilePath);
                // if (File.Exists(htmlFilePath))
                // File.Delete(htmlFilePath);
            }
        }
        /// <summary> 
        /// 判断类型
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public string[]  DocumentManage(string iiid, string format, object data, string dataType)
        {
            if (convert == null)
            {
                convert = new DocumentConvert();
            }
            format = format.ToLower();
            iiidFilePath = root + "ResourcesFiles\\" + format + "\\" + iiid + "\\" + iiid + "." + format;
            htmlFilePath = root + "ResourcesFiles\\" + format + "\\" + iiid + "\\" + iiid + "." + "html";
            string[] st =new string[4];
            st[3]= "/ResourcesFiles/" + format + "/" + iiid + "/" + iiid + "."+format;
            st[2] = "success";
            switch (format)
            {
                case "html":
                    break;
                case "dataset":
                    st[1] = "";//System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(data));
                    st[0] = "dataset";
                    break;

                //case DataFormat.:
                //    st[1] = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(data));
                //    st[0] = "datatable";
                //    break;

                case  "txt":
                    st[0] = "txt";
                    GetIiidFile(iiid, format, data, dataType);
                    st[1] = "/ResourcesFiles/" + format + "/" + iiid + "/" + iiid + ".txt";
                    break;

                case "gdb":
                    st[0] = "gdb";
                    GetIiidFile(iiid, format, data, dataType);
                    st[1] = "/ResourcesFiles/" + format + "/" + iiid + "/" + iiid + ".gdb";
                    break;

                case "jpg":
                    st[0] = "jpg";
                    GetIiidFile(iiid, format, data, dataType);
                    st[1] = "/ResourcesFiles/" + format + "/" + iiid + "/" + iiid + ".jpg";
                    break;

                case "doc":
                    fileUrl = "/ResourcesFiles/" + format + "/" + iiid + "/" + iiid + ".html";
                    st[0] = "html";
                    GetIiidFile(iiid, format, data, dataType);
                    if (File.Exists(htmlFilePath))
                        st[1] = fileUrl;
                    else
                    {
                        convert.DocToHtml(iiidFilePath, htmlFilePath);
                        st[1] = fileUrl;
                    }
                    break;

                case "docx":
                    fileUrl = "/ResourcesFiles/" + format + "/" + iiid + "/" + iiid + ".html";
                    st[0] = "html";
                    GetIiidFile(iiid, format, data, dataType);
                    if (File.Exists(htmlFilePath))
                        st[1] = fileUrl;
                    else
                    {
                        convert.DocToHtml(iiidFilePath, htmlFilePath);
                        st[1] = fileUrl;
                    }
                    break;

                case "xls":
                    fileUrl = "/ResourcesFiles/" + format + "/" + iiid + "/" + iiid + ".html";
                    st[0] = "html";
                    GetIiidFile(iiid, format, data, dataType);
                    if (File.Exists(htmlFilePath))
                        st[1] = fileUrl;
                    else
                    {
                        convert.XlsToHtml(iiidFilePath, htmlFilePath);
                        st[1] = fileUrl;
                    }
                    break;

                case "xlsx":
                    fileUrl = "/ResourcesFiles/" + format + "/" + iiid + "/" + iiid + ".html";
                    st[0] = "html";
                    GetIiidFile(iiid, format, data, dataType);
                    if (File.Exists(htmlFilePath))
                        st[1] = fileUrl;
                    else
                    {
                        convert.XlsToHtml(iiidFilePath, htmlFilePath);
                        st[1] = fileUrl;
                    }
                    break;

                case "ppt":
                    fileUrl = "/ResourcesFiles/" + format + "/" + iiid + "/" + iiid + ".html";
                    st[0] = "html";
                    GetIiidFile(iiid, format, data, dataType);
                    //if (File.Exists(htmlFilePath))
                    //    st[1] = fileUrl;
                    //else
                    //{
                     convert.PptToHtml(iiidFilePath, htmlFilePath);
                  
                    st[1] = fileUrl;
                  //  }
                    break;

                case "pptx":
                    fileUrl = "/ResourcesFiles/" + format + "/" + iiid + "/" + iiid + ".html";
                    st[0] = "html";
                    GetIiidFile(iiid, format, data, dataType);
                    if (File.Exists(htmlFilePath))
                        st[1] = fileUrl;
                    else
                    {
                        convert.PptToHtml(iiidFilePath, htmlFilePath);
                        st[1] = fileUrl;
                    }
                    break;

                case "pdf":
                    fileUrl = "/ResourcesFiles/" + format + "/" + iiid + "/" + iiid + ".pdf";
                    st[0] = "pdf";
                    GetIiidFile(iiid, format, data, dataType);
                     st[1] = fileUrl;
                    break;

                default:
                    st[0] = "";
                    st[1] = "";
                    st[2] = "未识别格式！";
                    break;
            }
            return st;
        }

 
        /// <summary>
        /// 获取文件 并不缓存  （缓存方法在下面）
        /// </summary>
        public string GetIiidFile(string iiid, string format, object data, string dataType)
        {
            formatDirPath = root + "ResourcesFiles\\" + format;
            iiidDirPath = formatDirPath + "\\" + iiid;
            iiidFilePath = iiidDirPath + "\\" + iiid + "." + format;
            if (File.Exists(iiidFilePath))  //如果以前有缓存就删除
            {
                File.Delete(iiidFilePath);
                if (File.Exists(htmlFilePath)) 
                {
                    File.Delete(htmlFilePath);
                }
                //return iiidFilePath;
            }

            if (!Directory.Exists(formatDirPath)) //判断格式文件夹路径
            {
                Directory.CreateDirectory(formatDirPath);
            }

            if (!Directory.Exists(iiidDirPath))// 判断iiid文件夹路径
            {
                Directory.CreateDirectory(iiidDirPath);
            }

            if (dataType.ToLower() == "url")  //文件是url路径
            {
                using (WebClient webClient = new WebClient())
                {
                    //拿不到文件会报错，这是URL已经生成，没有实体会显示不出文件，使用这里的异常不需要处理什么。
                    try
                    {
                        webClient.DownloadFile(data.ToString(), iiidFilePath);
                    }
                    catch (Exception ex)
                    {
                        return "文件获取错误";
                    }
                }
            }

            if (dataType.ToLower() == "entity")  //文件是实体 字符串
            {
               
                try
                {
                    using (var a = new FileStream(iiidFilePath, FileMode.Create))
                    {
                        data.As<Stream>().Position = 0;
                        data.As<Stream>().CopyTo(a);
                    }
                }
                  //{
                  //File.WriteAllBytes()
                  // byte[] bytes = Convert.FromBase64String(data);//将64字节流转化为数组
                  //File.WriteAllBy/tes(iiidFilePath, bytes);//将字节数组转为文件并保存
                    catch (Exception ex)
                    {
                        return "文件获取错误";

                    }
                return iiidFilePath;
            }
            return iiidFilePath;
        }


        /// <summary>
        /// 获取文件 并缓存
        /// </summary>
        public string SaveIiidFile(string iiid, string format, string data, string dataType)
        {
            formatDirPath = root + "ResourcesFiles\\" + format;
            iiidDirPath = formatDirPath + "\\" + iiid;
            iiidFilePath = iiidDirPath + "\\" + iiid + "." + format;
            if (File.Exists(iiidFilePath))
            {
                return iiidFilePath;
            }

            if (!Directory.Exists(formatDirPath)) //判断格式文件夹路径
            {
                Directory.CreateDirectory(formatDirPath);
            }

            if (!Directory.Exists(iiidDirPath))// 判断iiid文件夹路径
            {
                Directory.CreateDirectory(iiidDirPath);
            }

            if (dataType.ToLower() == "url")  //文件是url路径
            {
                using (WebClient webClient = new WebClient())
                {
                    //拿不到文件会报错，这是URL已经生成，没有实体会显示不出文件，使用这里的异常不需要处理什么。
                    try
                    {
                        webClient.DownloadFile(data, iiidFilePath);
                    }
                    catch (Exception)
                    {
                        return "文件获取错误";
                    }
                }
            }

            if (dataType.ToLower() == "entity")  //文件是实体 字符串
            {
                try
                {
                    byte[] bytes = Convert.FromBase64String(data);//将64字节流转化为数组
                    File.WriteAllBytes(iiidFilePath, bytes);//将字节数组转为文件并保存
                }
                catch (Exception ex)
                {
                    return "文件获取错误";

                }
                return iiidFilePath;
            }
            return iiidFilePath;
        }


        /// <summary> 
        /// datatable转xlsx
        /// </summary> 
        /// <param name="ds">要导出的数据</param> 
        /// <param name="tableName">表格标题</param> 
        /// <param name="path">保存路径</param> 
        public string taleToXlsx(DataTable dt, string title, string savePath)
        {
            try
            {
                Aspose.Cells.Workbook workbook = new Aspose.Cells.Workbook(); //工作簿 
                //System.Data.DataTable dt = ds.Tables[0];
                Aspose.Cells.Worksheet sheet = workbook.Worksheets[0]; //工作表 
                Aspose.Cells.Cells cells = sheet.Cells;//单元格 

                //为标题设置样式     
                Aspose.Cells.Style styleTitle = workbook.Styles[workbook.Styles.Add()];//新增样式 
                styleTitle.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;//文字居中 
                styleTitle.Font.Name = "宋体";//文字字体 
                styleTitle.Font.Size = 18;//文字大小 
                styleTitle.Font.IsBold = true;//粗体 

                //样式2 
                Aspose.Cells.Style style2 = workbook.Styles[workbook.Styles.Add()];//新增样式 
                style2.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;//文字居中 
                style2.Font.Name = "宋体";//文字字体 
                style2.Font.Size = 14;//文字大小 
                style2.Font.IsBold = true;//粗体 
                style2.IsTextWrapped = true;//单元格内容自动换行 
                style2.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
                style2.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
                style2.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
                style2.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;

                //样式3 
                Style style3 = workbook.Styles[workbook.Styles.Add()];//新增样式 
                style3.HorizontalAlignment = TextAlignmentType.Center;//文字居中 
                style3.Font.Name = "宋体";//文字字体 
                style3.Font.Size = 12;//文字大小 
                style3.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
                style3.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
                style3.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
                style3.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;

                int Colnum = dt.Columns.Count;//表格列数 
                int Rownum = dt.Rows.Count;//表格行数 

                //生成行1 标题行    
                cells.Merge(0, 0, 1, Colnum);//合并单元格 
                cells[0, 0].PutValue(title);//填写内容 
                cells[0, 0].SetStyle(styleTitle);
                cells.SetRowHeight(0, 38);

                //生成行2 列名行 
                for (int i = 0; i < Colnum; i++)
                {
                    cells[1, i].PutValue(dt.Columns[i].ColumnName);
                    cells[1, i].SetStyle(style2);
                    cells.SetRowHeight(1, 25);
                }

                //生成数据行 
                for (int i = 0; i < Rownum; i++)
                {
                    for (int k = 0; k < Colnum; k++)
                    {
                        cells[2 + i, k].PutValue(dt.Rows[i][k].ToString());
                        cells[2 + i, k].SetStyle(style3);
                    }
                    cells.SetRowHeight(2 + i, 24);
                }

                workbook.Save(savePath);
                return "";
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>
        /// 文件下载
        /// </summary>
        /// <param name="filePath"></param>
        public void DownLoadFile(string filePath)
        {
            string path = HttpContext.Current.Server.MapPath(filePath); //路径
            string fileName = filePath.Split('/')[filePath.Split('/').Length - 1];
            if (File.Exists(path))  //如果文件存在
            {
                FileInfo fileInfo = new FileInfo(path);
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.ClearHeaders();
                //加上HttpUtility.UrlEncode()方法，防止文件下载时，文件名乱码，（保存到磁盘上的文件名称应为“中文名.gif”）
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(fileName));
                //Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                HttpContext.Current.Response.AddHeader("Content-Transfer-Encoding", "binary");
                //Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
                //Response.TransmitFile(path);  
                HttpContext.Current.Response.WriteFile(fileInfo.FullName);
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
        }
    }
}
