using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Jurassic.So.ETL;
using Jurassic.So.Infrastructure;

namespace Jurassic.So.Adapter.MockGB
{
    /// <summary>MockGB适配器</summary>
    [ETLComponent("MockGBAdapter", Title = "MockGB适配器")]
    public class MockGBAdapter : SQLAdapter
    {
        /// <summary>构造函数</summary>
        public MockGBAdapter()
        {
            var configFile = Assembly.GetExecutingAssembly().GetAssemblyConfigFile();
            LoadFromConfig(configFile);
        }
    }
}
