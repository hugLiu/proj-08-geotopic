using Jurassic.AppCenter;
using Jurassic.AppCenter.AppServices;
using Jurassic.AppCenter.Config;
using Jurassic.AppCenter.SmartClient.Infrastructure.Interface.Services;
using Jurassic.AppCenter.SmartClient.Infrastructure.Library.Services;
using Microsoft.Practices.CompositeUI;
using Microsoft.Practices.CompositeUI.Services;
using Microsoft.Practices.ObjectBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jurassic.AppCenter.Logs;
using Jurassic.Log4;
//using Jurassic.Com.Tools;
//using Jurassic.Com.Wcf;
//using Jurassic.AppCenter.SmartClient.AutoUpdateModule;

namespace Jurassic.So.GeoTopic.SubmissionTool.Services
{
    public class StartupService : IStartupService
    {
        private WorkItem _rootWorkItem;

        [InjectionConstructor]
        public StartupService([ServiceDependency] WorkItem rootWorkItem)
        {
            _rootWorkItem = rootWorkItem;
        }

        public void AddServices()
        {
            #region add用户服务
            var jLogManager = new JLogManager();
            _rootWorkItem.Services.Add<IJLogManager>(jLogManager);
            LogHelper.Init(jLogManager, "default");
            _rootWorkItem.Services.AddNew<LoginService, ILoginService>();
            AppManager.Instance.StateProvider = new AppStateProvider();
            #endregion
        }
    }
}
