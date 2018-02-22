using System.Web;
using System.Web.Mvc;

namespace Jurassic.So.Index.MockWebUI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
