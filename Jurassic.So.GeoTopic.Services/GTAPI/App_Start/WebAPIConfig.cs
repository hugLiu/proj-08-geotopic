using System.Web.Http;
using GTAPI.Filters;
using GTAPI.API;
using Jurassic.So.Web;

namespace GTAPI
{
    public class WebAPIConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var webExceptionConfig = new WebExceptionConfig();
            webExceptionConfig.LoadConfig();
            config.Filters.Add(new ApiExceptionFilter() { Config = webExceptionConfig });
            config.Filters.Add(new LoggingRqrs());
            //config.SuppressDefaultHostAuthentication();
            //config.EnableCors();
          //  config.BindParameter(typeof(List<int>), new ListModelBinder());
            config.EnableSystemDiagnosticsTracing();
        }
    }
}