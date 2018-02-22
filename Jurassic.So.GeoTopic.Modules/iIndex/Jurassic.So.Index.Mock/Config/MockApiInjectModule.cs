using Ninject.Modules;

namespace Jurassic.So.Index
{
    /// <summary>模拟API注入模块</summary>
    public class MockApiInjectModule : NinjectModule
    {
        /// <summary>加载注入</summary>
        public override void Load()
        {
            Bind<IServiceMockConfig>().To<ServiceMockConfig>();
        }
    }
}
