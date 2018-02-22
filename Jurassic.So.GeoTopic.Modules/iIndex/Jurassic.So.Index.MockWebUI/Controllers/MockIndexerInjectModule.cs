using Jurassic.PKS.Service.Index;
using Jurassic.So.Index;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.Index
{
    /// <summary>模拟索引服务注入模块</summary>
    public class MockIndexerInjectModule : NinjectModule
    {
        /// <summary>加载注入</summary>
        public override void Load()
        {
            Bind<IIndexer>().To<MockIndexer>();
        }
    }
}
