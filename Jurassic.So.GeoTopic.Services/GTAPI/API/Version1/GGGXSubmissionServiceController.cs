using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Xml;
using Jurassic.PKS.Service;
using Jurassic.PKS.Service.GF;
using Jurassic.PKS.WebAPI.GF;

namespace GTAPI.API.Version1
{
    /// <summary>
    /// 3GX提交服务API
    /// </summary>
    public class GGGXSubmissionServiceController : WebAPIController
    {
        private ISubmission SubmissionService { get; }
        /// <summary>
        /// 依赖注入
        /// </summary>
        public GGGXSubmissionServiceController(ISubmission submission)
        {
            this.SubmissionService = submission;
        }
        /// <summary>
        /// 提交3GX数据到主数据库中
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<SubmissionResult> Submit(SubmissionInfoRequest request)
        {
            var info = new SubmissionInfo();
            info.GGGXData = new XmlDocument();
            info.GGGXData.LoadXml(request.GGGXData);
            info.Option = request.Option;
            return SubmissionService.Submit(info);
        }
        /// <summary>获得服务信息</summary>
        protected override ServiceInfo GetServiceInfo()
        {
            return new ServiceInfo()
            {
                Description = "提交成果文件及其元数据到目标系统中"
            };
        }
    }
}
