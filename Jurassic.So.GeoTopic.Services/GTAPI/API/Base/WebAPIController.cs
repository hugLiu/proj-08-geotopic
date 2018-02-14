using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Jurassic.So.Infrastructure;
using Jurassic.WebApi.Providers;
using System.Reflection;
using Jurassic.PKS.WebAPI.Models;
using System.Threading.Tasks;
using Jurassic.PKS.Service;

namespace GTAPI.API
{
    /// <summary>GTAPI控制器基类</summary>
    //[WebApiFilterAuthorize]
    [ApiActionParameterValidation]
    public abstract class WebAPIController : BaseApiController
    {
        #region 服务能力方法
        /// <summary>获得服务信息</summary>
        protected virtual ServiceInfo GetServiceInfo()
        {
            throw new NotImplementedException();
        }
        /// <summary>获得服务能力数据</summary>
        protected virtual Task<ServiceCapabilities> GetServiceCapabilities()
        {
            return Task.FromResult(new ServiceCapabilities());
        }
        /// <summary>获得服务支持能力数据</summary>
        /// <returns>服务支持能力数据</returns>
        [HttpGet]
        public async Task<ServiceCapabilities> GetCapabilities()
        {
            var result = await GetServiceCapabilities();
            result.ServiceInfo = GetServiceInfo();
            if (result.ServiceInfo.Name.IsNullOrEmpty())
            {
                result.ServiceInfo.Name = this.ControllerContext.ControllerDescriptor.ControllerName;
            }
            if (result.ServiceInfo.Developer.IsNullOrEmpty())
            {
                result.ServiceInfo.Developer = "武汉侏罗纪搜索业务部";
            }
            var requests = new List<MethodInfo>();
            var controllerType = this.GetType();
            var bflags = BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public;
            while (true)
            {
                requests.AddRange(controllerType.GetMethods(bflags));
                if (controllerType == typeof(WebAPIController)) break;
                controllerType = controllerType.BaseType;
            }
            result.Requests = requests.Select(e => e.Name).ToArray();
            return result;
        }
        #endregion
    }
}