using Jurassic.So.Infrastructure;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Jurassic.So.Index.MockWebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            UnityWebApiActivator.Start();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AutoMapperUtil.LoadConfig();
            //TODO: 模拟服务注入，发布前请先注释本代码
            //ninjectKernel.Load(new MockApiInjectModule());
            //ninjectKernel.Load(new MockIndexerInjectModule());
        }
    }
}
