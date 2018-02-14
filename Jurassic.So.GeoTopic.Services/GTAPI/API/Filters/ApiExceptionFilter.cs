using Jurassic.So.Infrastructure;
using Jurassic.So.Web;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Web.Http.Filters;
using Jurassic.So.Infrastructure.Logging;
using Jurassic.PKS.Service;

namespace GTAPI.API
{
    /// <summary>GTAPI异常处理拦截器</summary>
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        /// <summary>构造函数</summary>
        public ApiExceptionFilter()
        {
            this.ContentType = new MediaTypeHeaderValue(MimeTypeConst.Exception);
            this.Bearer = new AuthenticationHeaderValue("Bearer");
        }
        /// <summary>异常配置</summary>
        public WebExceptionConfig Config { get; set; }
        /// <summary>内容类型</summary>
        private MediaTypeHeaderValue ContentType { get; set; }
        /// <summary>授权头值</summary>
        private AuthenticationHeaderValue Bearer { get; set; }
        /// <summary>处理异常</summary>
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            //异常错误信息应该都来源于配置，但是配置的异常需要修改模块的代码，改动较多。
            var ex = actionExecutedContext.Exception;
            var controllerContext = actionExecutedContext.ActionContext.ControllerContext;
            try
            {
                string url = actionExecutedContext.Request.RequestUri.AbsoluteUri;
                dynamic nameObj = controllerContext.Controller;
                var userName = nameObj.User.Identity.Name;
                Logger.ErrorRp(ex, url, userName);
            }
            catch (Exception)
            {
                // ignored
            }
            var exceptionModel = ex.ToModel();
            var controller = controllerContext.ControllerDescriptor.ControllerName;
            var webExceptionModel = this.Config.MapTo(ex, controller, exceptionModel);
            var response = actionExecutedContext.Request.CreateResponse(webExceptionModel.StatusCode, exceptionModel);
            if (!webExceptionModel.ReasonPhrase.IsNullOrEmpty()) response.ReasonPhrase = webExceptionModel.ReasonPhrase;
            if (ex is AuthenticationException)
            {
                response.Headers.WwwAuthenticate.Add(this.Bearer);
            }
            response.Content.Headers.ContentType = this.ContentType;
            actionExecutedContext.Response = response;
        }
    }
}
