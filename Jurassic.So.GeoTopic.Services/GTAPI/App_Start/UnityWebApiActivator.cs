using System.Web.Http;
using Microsoft.Practices.Unity.WebApi;
using Jurassic.So.Infrastructure;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(GTAPI.App_Start.UnityWebApiActivator), "Start")]

namespace GTAPI.App_Start
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
            var resolver = new UnityDependencyResolver(container);


            GlobalConfiguration.Configuration.DependencyResolver = resolver;
        }
    }
}