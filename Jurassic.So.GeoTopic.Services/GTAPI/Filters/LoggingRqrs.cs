using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Jurassic.So.Infrastructure.Logging;
using Newtonsoft.Json;

namespace GTAPI.Filters
{
    /// <summary>
    /// 记录请求和应答
    /// </summary>
    public class LoggingRqrs : ActionFilterAttribute
    {
        /// <summary>
        /// 记录请求和应答
        /// </summary>
        /// <remarks>
        /// 会记录来自框架部分的方法，不只是Api部分的，框架也属于访问，所以暂时没有去掉。
        /// </remarks>
        public override void OnActionExecuted(HttpActionExecutedContext filterExecutedContext)
        {
            if (filterExecutedContext.Response != null)
            {
                var request = string.Empty;
                try
                {
                    request = GetRequest(filterExecutedContext);
                }
                catch (Exception)
                {
                    request = "获取请求数据失败";
                }
                var response = string.Empty;
                try
                {
                    response = filterExecutedContext.Response.Content != null ?
                        filterExecutedContext.Response.Content.ReadAsStringAsync().Result :
                        filterExecutedContext.Response.StatusCode.ToString();
                    if (response.Length > 1000) response = response.Substring(0, 1000);
                }
                catch (Exception)
                {
                    response = "获取应答数据失败";
                }
                var userName = string.Empty;
                try
                {
                    dynamic controller = filterExecutedContext.ActionContext.ControllerContext.Controller;
                    userName = controller.User.Identity.Name;
                }
                catch (Exception)
                {
                    response = "获取登录用户失败";
                }
                try
                {
                    Logger.LogRpRq(response, request, userName);
                    if (IsData(filterExecutedContext.Request))
                    {
                        Logger.DataRp(response, request, userName);
                    }
                }
                catch (Exception) { }
            }
            base.OnActionExecuted(filterExecutedContext);
        }
        /// <summary>
        /// 获取请求数据
        /// </summary>
        private string GetRequest(HttpActionExecutedContext filterContext)
        {
            if (filterContext.Request == null) return "";
            var requestUrl = filterContext.Request.RequestUri.ToString();
            var sbParameter = new StringBuilder();
            var parameters = filterContext.ActionContext.ActionArguments;
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    sbParameter.Append(parameter.Key)
                        .Append(":")
                        .Append(JsonConvert.SerializeObject(parameter.Value))
                        .AppendLine();
                }
            }
            return $"{requestUrl}{Environment.NewLine}Parameters:{sbParameter.ToString()}";
            //Logger.LogRequest(_request);
        }
        /// <summary>
        /// 是否数据
        /// </summary>
        private bool IsData(HttpRequestMessage request)
        {
            return request.RequestUri.LocalPath.IndexOf("/DataService/Retrieve", StringComparison.OrdinalIgnoreCase) > -1;
        }
    }
}