using Jurassic.PKS.Service;
using Jurassic.So.GeoTopic.DataService.Models;
using Jurassic.So.Infrastructure;
using Jurassic.WebFrame;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Jurassic.So.Business;

namespace Jurassic.So.GeoTopic.Web.Controllers
{
    /// <summary>
    /// 前台获取详情页面数据
    /// </summary>
    public class PTDetailController : GTBaseController
    {
        private static readonly WebServiceController Services = new WebServiceController();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public JsonResult GetDetailData(string iiid)
        {
            var metadataDefinitions = KMD.DefaultKmdConfiguration;
            if (metadataDefinitions == null)
            {
                lock (Services)
                {
                    metadataDefinitions = KMD.DefaultKmdConfiguration;
                    if (metadataDefinitions == null)
                    {
                        var metadataDefinition = Services.GetMetadataDefinition();
                        metadataDefinitions = new KMDConfiguration(metadataDefinition.Data.As<string>());
                        KMD.DefaultKmdConfiguration = metadataDefinitions;
                    }
                }
            }
            var iiidKey = metadataDefinitions[MetadataConsts.IIId].Mapping.Get;
            var prams = new Dictionary<string, string> { { iiidKey, iiid } };
            var metadata = Services.GetMateData(prams);
            var kmd = new KMD(metadata.Content);
            AddUserBehavior(UserBehaviorType.Visited, kmd.Title, iiid);
            var detailData = new DetailDataModel(kmd, metadataDefinitions);
            return Json(detailData.ToJson(), JsonRequestBehavior.AllowGet);
        }
    }
}