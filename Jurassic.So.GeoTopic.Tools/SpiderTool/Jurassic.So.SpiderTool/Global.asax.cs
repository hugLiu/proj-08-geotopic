using Jurassic.AppCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Jurassic.Com.Tools;
using System.Globalization;
using Jurassic.AppCenter.Resources;
using Jurassic.AppCenter.Logs;
using Jurassic.So.SpiderTool.IService;
using Jurassic.So.SpiderTool.Service.Mapper;
using Ninject;
using Jurassic.CommonModels.Organization;
using Jurassic.CommonModels;
using Jurassic.CommonModels.EFProvider;
using Jurassic.So.SpiderTool.Service.Mapper;
using Jurassic.WebFrame;
using Jurassic.So.SpiderTool.Service;
using Jurassic.So.SpiderTool.Service.Module;
using Jurassic.So.Infrastructure;

namespace Jurassic.So.SpiderTool
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    /// <summary>
    /// 应用程序必须继承MvcApplication
    /// </summary>
    public class Application1 : MvcApplication
    {
        protected override IEnumerable<string> ControllerNameSpaces
        {
            get
            {
                //声明自身Controller所在的命名空间
                return new string[] { typeof(Jurassic.So.SpiderTool.Application1).Namespace + ".Controllers"};
            }
        }

        protected override void Application_Start()
        {
            base.Application_Start();
            //AutoMapper初始化
            AutoMapperUtil.LoadConfig();
            ScheduleContext.GetContext().SpiderSchedule.SchedulePlans();
        }
        
        protected override void AddBindings(IKernel ninjectKernel)
        {
            base.AddBindings(ninjectKernel);
            ninjectKernel.Rebind<ModelContext>().ToSelf()
              .WithPropertyValue("Schema", ""); //要支持Oralce数据库，需要""中是Oralce库的Schema名称

            ninjectKernel.Load(new ServiceModule());
            ninjectKernel.Bind<SpiderSchedule>().ToSelf();

            //todo: 额外的注入代码
            //ninjectKernel.Load(new DataServiceManagement.Service.Module.ServiceModule());
        }
    }
}