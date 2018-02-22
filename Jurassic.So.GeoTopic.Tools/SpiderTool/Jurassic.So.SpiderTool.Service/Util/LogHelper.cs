using System;
using System.Collections.Generic;
using System.IO;

namespace Jurassic.So.SpiderTool.Service.Util
{
    public class Log
    {
        private static readonly object obj = new object();
        /// <summary>
        /// 操作日志
        /// </summary>
        /// <param name="s">日志能容</param>
        public static void WriteLog(string title, string content)
        {
            List<string> contents = new List<string> {content};
            WriteLogs(title, contents, "操作日志");
        }

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="s">日志能容</param>
        public static void WriteError(string title, List<string> contents)
        {
            WriteLogs(title, contents, "错误日志");
        }

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="s">日志能容</param>
        public static void WriteError(string title, string content)
        {
            List<string> contents = new List<string> {content};
            WriteLogs(title, contents, "错误日志");
        }

        /// <summary>
        /// 操作日志
        /// </summary>
        /// <param name="s">日志能容</param>
        public static void WriteError(string title, Exception ex)
        {
            List<string> contents = new List<string>();
            if (ex.InnerException != null)
            {
                WriteError(title, ex.InnerException);
            }
            else
            {
                contents.Add($"错误信息:{ex.Message}");
                contents.Add($"错误堆栈:{ex.StackTrace}");
                contents.Add($"错误源:{ex.Source}");
                WriteLogs(title, contents, "错误日志");
            }
        }


        public static void WriteLogs(string title, List<string> contents, string type)
        {
            lock (obj)
            {
                string path = AppDomain.CurrentDomain.BaseDirectory;
                if (!string.IsNullOrEmpty(path))
                {
                    path = AppDomain.CurrentDomain.BaseDirectory + "log";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    path = path + "\\" + DateTime.Now.ToString("yyMM");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    path = path + "\\" + DateTime.Now.ToString("dd") + ".txt";
                    if (!File.Exists(path))
                    {
                        FileStream fs = File.Create(path);
                        fs.Close();
                    }
                    if (File.Exists(path))
                    {
                        StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.Default);
                        sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + " " + title);
                        sw.WriteLine("日志类型：" + type);
                        sw.WriteLine("详情：");
                        contents.ForEach(line => sw.WriteLine(line));
                        sw.WriteLine("----------------------------------------");
                        sw.Close();
                    }
                }
            }
        }
    }
}
