using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Jurassic.PKS.Service;
using Jurassic.PKS.WebAPI.Models;
using Jurassic.So.Infrastructure;
using Jurassic.So.Infrastructure.Logging;
using Jurassic.So.Web;

namespace GTAPI.API
{
    /// <summary>GTAPI方法参数验证器</summary>
    public class ApiActionParameterValidationAttribute : ActionFilterAttribute
    {
        /// <summary>Occurs before the action method is invoked.</summary>
        /// <param name="actionContext">The action context.</param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var actionArguments = actionContext.ActionArguments;
            if (actionArguments.Count == 0) return;
            foreach (var parameterDescriptor in actionContext.ActionDescriptor.GetParameters())
            {
                if (!typeof(IRequest).IsAssignableFrom(parameterDescriptor.ParameterType)) continue;
                object parameterValue;
                if (!actionArguments.TryGetValue(parameterDescriptor.ParameterName, out parameterValue) || parameterValue == null)
                {
                    ExceptionCodes.MissingParameterValue.ThrowUserFriendly($"请求参数{parameterDescriptor.ParameterName}不允许为null！", "缺少请求参数！");
                }
            }
            var modelState = actionContext.ModelState;
            if (!modelState.IsValid)
            {
                var httpError = new HttpError(modelState, true);
                var message = BuildErrorDetails(httpError.ModelState);
                ExceptionCodes.MissingParameterValue.ThrowUserFriendly(message, "缺少请求参数！");
            }
        }
        /// <summary>Occurs after the action method is invoked.</summary>
        /// <param name="actionExecutedContext">The action executed context.</param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
        }
        /// <summary>生成错误明细</summary>
        /// <param name="actionExecutedContext">The action executed context.</param>
        private string BuildErrorDetails(HttpError modelStateError)
        {
            var details = modelStateError.Values.Cast<string[]>().SelectMany(e => e).ToArray();
            return string.Join(Environment.NewLine, details);

        }
    }
}
