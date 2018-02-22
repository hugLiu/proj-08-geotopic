using System.Web.Http;
using Jurassic.So.Infrastructure;
using System.Web.Mvc;
using Jurassic.So.Web;
using GTAPI.API;

namespace Jurassic.So.Index.MockWebUI
{
    /// <summary>Provides the bootstrapping for integrating Unity with WebApi when it is hosted in ASP.NET</summary>
    public static class UnityWebApiActivator
    {
        /// <summary>Integrates Unity when the application starts.</summary>
        public static void Start()
        {
            // Use UnityHierarchicalDependencyResolver if you want to use a new child container for each IHttpController resolution.
            // var resolver = new UnityHierarchicalDependencyResolver(UnityConfig.GetConfiguredContainer());
            var container = IocContext.Instance;

            //MVC注入
            DependencyResolver.SetResolver(new Microsoft.Practices.Unity.Mvc.UnityDependencyResolver(container));

            GlobalConfiguration.Configuration.DependencyResolver = new Microsoft.Practices.Unity.WebApi.UnityDependencyResolver(container);

            GlobalConfiguration.Configuration.Routes.MapHttpRoute(
                name: "DefaultApiWithVersion",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional });

            var webExceptionConfig = new WebExceptionConfig();
            webExceptionConfig.LoadConfig();
            GlobalConfiguration.Configuration.Filters.Add(new ApiExceptionFilter() { Config = webExceptionConfig });
        }
    }
}