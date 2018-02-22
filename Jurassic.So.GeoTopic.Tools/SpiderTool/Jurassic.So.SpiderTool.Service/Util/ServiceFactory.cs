using Jurassic.CommonModels;
using Jurassic.So.SpiderTool.IService;
using Ninject;

namespace Jurassic.So.SpiderTool.Service.Util
{
    public class ServiceFactory
    {
	    public IWrapedDataService NewWrapedDataService()
	    {
		    IWrapedDataService service = SiteManager.Kernel.Get<IWrapedDataService>();
		    return service;
	    }

        public IWrapedIndexerService NewWrapedIndexerService()
        {
            IWrapedIndexerService service = SiteManager.Kernel.Get<IWrapedIndexerService>();
            return service;
        }
    }
}
