using Jurassic.So.SpiderTool.IService;

namespace Jurassic.So.SpiderTool.Service.Module
{
    public class ServiceModule : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            Bind<IAdapterManageService>().To<AdapterManageService>();
            Bind<IAdapterInfoService>().To<AdapterInfoService>();
            Bind<IPlanService>().To<PlanService>();
            Bind<ITaskLogInfoService>().To<TaskLogInfoService>();
            Bind<ITaskService>().To<TaskService>();
			Bind<IWrapedDataService>().To<WrapedDataService>();
            Bind<IWrapedIndexerService>().To<WrapedIndexerService>();
        }
    }
}
