using Jurassic.AppCenter;
using Jurassic.PKS.Service;
using Jurassic.PKS.Service.Submission;
using Jurassic.So.Infrastructure;
using Jurassic.WebFrame;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Jurassic.So.GeoTopic.Web.Controllers.GeoTopic
{

    /// <summary>
    /// 文件提交类
    /// </summary>
    public class SubmitController: BaseController
    {
        private static readonly WebServiceController Services = new WebServiceController();
        private static string _metadataDefinition = string.Empty;

        /// <summary>
        /// 获取元数据 MetadataDefinition
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public JsonResult GetMetadata (string param)
        {
            var result = Services.GetMetadataDefinition();
            _metadataDefinition = result.Data as string;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 提交方法
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public JsonResult Submited(string param)
        {
            var data = JObject.Parse(param);
            var subInfo = new Models.SubmissionInfoRequset
            {
                Action = GetAction(data["action"].ToString()),
                NatureKey = data["naturekey"].ToString(),
                FileIDs = data["fileIDs"].ToObject<List<string>>(),                
                KMD = GetKMD((JArray) data["formList"]),
                Option = new SubmissionOption
                {
                    Authentic = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["Authentic"]),
                    AutoComplement = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["AutoComplement"]),
                    UploadedBy = User.Identity.GetUserName(),
                    UploadedDate = DateTime.Now,
                    Contact = System.Configuration.ConfigurationManager.AppSettings["Contact"],
                    Application = System.Configuration.ConfigurationManager.AppSettings["Application"],
                    Task = System.Configuration.ConfigurationManager.AppSettings["Task"]
                }
            };
            var result = Services.PostSubmit(subInfo.ToJson());
            return Json(result.Content, JsonRequestBehavior.AllowGet);
        }

        private SubmissionAction GetAction(string action)
        {
            switch (action)
            {
                case "submit":
                    return SubmissionAction.Create;
                case "delete":
                    return SubmissionAction.Delete;
                case "update":
                    return SubmissionAction.Replace;
                default:
                    return SubmissionAction.Create;
            }        
        }

        /// <summary>
        /// 构建元数据
        /// </summary>
        /// <param name="formlist">前台表单数据</param>
        /// <returns></returns>
        private object GetKMD(JArray formlist)
        {
            if (string.IsNullOrEmpty(_metadataDefinition))
            {
                _metadataDefinition = Services.GetMetadataDefinition().Data as string;
            }
            var kmdConfiguration = new KMDConfiguration(_metadataDefinition);
            var kmd = new KMD("{}", kmdConfiguration);
            foreach (var t in formlist)
            {
                var item = t.CastTo<JArray>();
                foreach (var t1 in item)
                {
                    if (t1["uitype"].ToString() == UiType.Datetime.ToString() ||
                        t1["uitype"].ToString() == UiType.Date.ToString())
                    {
                        var strIsoDate = GetISODate(t1["text"].ToString());
                        if (strIsoDate != "")
                            kmd.SetProperty(t1["name"].ToString(), strIsoDate);
                    }
                    else if (t1["uitype"].ToString() == UiType.Tageditor.ToString())
                    {
                        var strs = t1["text"].ToString().JsonTo<string[]>();
                        if (strs != null)
                            kmd.SetProperty(t1["name"].ToString(), strs);
                    }
                    else
                    {
                        if (t1["text"].ToString() != "")
                            kmd.SetProperty(t1["name"].ToString(), t1["text"].ToString());
                    }
                }
            }
            return kmd.ToIndex().JsonTo<object>();
        }

        /// <summary>
        /// 将Date字符串转为ISODate字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string GetISODate(string str)
        {
            DateTime dt;
            return DateTime.TryParse(str, out dt) ? dt.ToISODateString() : str;
        }
    }
}