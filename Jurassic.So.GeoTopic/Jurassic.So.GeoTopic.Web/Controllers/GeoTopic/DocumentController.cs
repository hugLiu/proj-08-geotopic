using DocumentDisplay;
using Jurassic.So.GeoTopic.Web.Models;
using Jurassic.So.Infrastructure;
using Jurassic.WebFrame;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Jurassic.So.GeoTopic.Web.Controllers.GeoTopic
{
    /// <summary>
    /// 文档显示类 控制器
    /// </summary>
    [Authorize]
    public class DocumentController : BaseController
    {
        private static readonly WebServiceController Services = new WebServiceController();

        /// <summary>
        /// 文档显示   ------包含整套逻辑，给GTUI调用
        /// </summary>fileFormat
        /// <param name="param"></param>
        /// <returns></returns>
        public JsonResult DocumentManage(string param)
        {

            //第一步： 调用GetMatch方法获取 url及其他信息
            var matchData = Match(param);
            var ptItems = new List<PtRenderItem>();
            foreach (var item in matchData)
            {
                var url = item["source"]["url"]?.ToString() ?? "";
                var iiid = item["iiid"]?.ToString() ?? "";
                var ptItem = new PtRenderItem
                {
                    BO = string.Empty,
                    Title = string.Empty,
                    PT = string.Empty,
                    FileCount = string.Empty,
                    Iiid = iiid
                };

                //这些逻辑有待 提问，这个应该是在 GTrender 里面处理
                var files = new List<FilesItem>();
                //第一步： 调用GetMatch方法获取 url及其他信息
                var retrieveData = Retrieve(url);
                foreach (var itemRetrieve in retrieveData)
                {                    
                    //第二步： 调用Retrieve方法获取 url及其他信息
                    var ticket = itemRetrieve["ticket"]?.ToString() ?? "";
                    var fileFormat = itemRetrieve["format"]?.ToString() ?? "";

                    //第三步，调用GetData方法
                    var st = GetFile(fileFormat, iiid, url, ticket, "entity");
                    var fileItem = new FilesItem
                    {
                        Format = st[0],
                        Url = st[1],
                        FileUrl = st[3],
                        Size = retrieveData.Count.ToString(),
                        FileName = itemRetrieve["name"]?.ToString() ?? "",
                        Type = "url",
                        Data = ""
                    };
                    //---------经过文档转换之后的format,url
                    //暂时没考虑 流的情况
                    //暂时没考虑 流的情况
                    //ptItem.Files = new List<Models.FilesItem>();
                    files.Add(fileItem);
                    //ptItem.Files.Add(fileItem);
                }
                ptItem.Files = files;
                ptItems.Add(ptItem);
            }
            var ptModel = new PtRenderModel {PtRender = ptItems};
            return Json(ptModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="format"></param>
        /// <param name="iiid"></param>
        /// <param name="url"></param>
        /// <param name="ticket"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public string[] GetFile(string format, string iiid, string url, string ticket, string dataType)
        {
            var docu = new DocumentCookie();
            var pram = new Dictionary<string, string>();
            //    -----------------------模拟数据没有iiid，暂时用url+ticket 生成MD5编码替代
            if (string.IsNullOrEmpty(iiid))
            {
                iiid = (url + ticket).CreateMD5Hash();
            }
            pram.Add("url", url);
            pram.Add("ticket", ticket);
            var result = GetData(url, ticket);
            return docu.DocumentManage(iiid, format, result, dataType);
        }

        /// <summary>
        /// Match接口方法
        /// </summary>
        /// <param name="pmdata"></param>
        /// <returns></returns>
        public JArray Match(string pmdata)
        {
            var result = Services.GetMatch(pmdata);
            var data = JObject.Parse(result.Content);
            return (JArray)data["metadatas"];
        }

        /// <summary>
        /// 获取Match数据方法
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public JsonResult GetMatchData(string param)
        {
            var result = Services.GetMatch(param);
            return Json(result.Content, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获得Search Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public JsonResult GetSearchData(string param)
        {
            var result = Services.GetSearch(param);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 调用Retrieve 接口获取 ticket 等信息
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public JArray Retrieve(string url)
        {
            var pram = new Dictionary<string, string> {{"url", url}};
            var result = Services.GetRetrieve(pram);
            var data = JArray.Parse(result.Data.ToString());
            //注意石亚茹接口 和 胡波接口不一样
            return data;
        }

        /// <summary>
        /// 调用GetData 接口 获取文件实体
        /// </summary>
        /// <param name="url"></param>
        /// <param name="ticket"></param>
        /// <returns></returns>
        public object GetData(string url, string ticket)
        {
            //Dictionary<string, string> pram = new Dictionary<string, string>();
            //pram.Add("url", url);
            //pram.Add("ticket", ticket);
            var result = Services.GetDataEntity(url, ticket);
            if (result is FileStreamResult) return result.As<FileStreamResult>().FileStream;
            if (result is ContentResult) return result.As<ContentResult>().Content;
            throw new System.Exception("未知错误(GetData)");
        }


        /// <summary>
        /// 文件下载
        /// </summary>
        /// <param name="param"></param>
        public void DownLoadFile(string param)
        {
            var data = JObject.Parse(param);
            if (data["type"].ToString() != "iiid") return;
            var path = "/ResourcesFiles/" + data["format"] + "/" + data["iiid"] + "/" + data["iiid"] + "." + data["format"];

            //string path = HttpContext.CurrentHandler.Server.MapPath(filePath); //路径
            //string fileName = filePath.Split('/')[filePath.Split('/').Length - 1];
            if (!System.IO.File.Exists(path)) return;
            var docu = new DocumentCookie();
            docu.DownLoadFile(path);
        }

    }
}