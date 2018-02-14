using System.Web.Http;
using Ninject;
using Jurassic.WebFrame;
using Jurassic.WebFrame.Models;
using Jurassic.WebApi.Providers;
using Jurassic.So.Data;
using Jurassic.So.Search.Mongo;
using Jurassic.So.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System;

namespace GTAPI
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    /// <summary>
    /// 应用程序必须继承MvcApplication
    /// </summary>
    public class Application1 : MvcApplication
    {
        // 以下代码在WebFrame 2.0版本以后不再需要，在WebFrame中已经根据加载的程序集自动生成
        /// <summary>
        // 加载命名空间
        /// </summary>
        protected override IEnumerable<string> ControllerNameSpaces
        {
            get
            {
                //声明自身Controller所在的命名空间，此处加载了所有组件的命名空间
                //实际项目可根据请况取舍
                var list = base.ControllerNameSpaces.ToList();
                list.Add(typeof(Application1).Namespace + ".Controllers");
                return list;
            }
        }
        

        protected override void Application_Start()
        {
            base.Application_Start();

            //AutoMapper初始化
            AutoMapperUtil.LoadConfig();
            //AutoMapperInitialization.Initialization();
            //RedefineViewLocator();
            //ObjectServiceAutoMapper.Initialization();
            //SearchServiceAutoMapper.Initialization();
            //SemanticsServiceAutoMapper.Initialization();
            WebAPIConfig.Register(GlobalConfiguration.Configuration);
            //NinjectRegister.RegisterFovWebApi(GlobalConfiguration.Configuration); //为WebApi注册IOC容器
            var cors = new System.Web.Http.Cors.EnableCorsAttribute(
                origins: "*",
                headers: "*",
                methods: "*"
                );
            cors.PreflightMaxAge = 86400;
            cors.SupportsCredentials = true;
            GlobalConfiguration.Configuration.EnableCors(cors);
            //  GlobalConfiguration.Configuration.BindParameter(typeof(MatchRequest), new FilterModelBinder());
            WebApiProvidersConfig.RefreshTokenLifeTime = 60;
            WebApiProvidersConfig.TokenLifeTime = 30;
            //var kmd = new KMD();
            //JsonMetadata.InitMetadataDefinitions(KMD.DefaultKmdConfiguration);
        }

        /// <summary>
        /// 追加或重新定义视图搜索路径
        /// </summary>
        protected void RedefineViewLocator()
        {
            //var viewEngine = (ViewEngines.Engines[0] as RazorViewEngine);
            //if (viewEngine == null) return;
            //viewEngine.ViewLocationFormats =
            //    new string[] { 
            //        "~/Views/{1}/{0}.cshtml",
            //        "~/Views/Shared/{0}.cshtml",
            //        "~/Views/Demo/{1}/{0}.cshtml"};
        }

        protected override void AddBindings(IKernel ninjectKernel)
        {
            base.AddBindings(ninjectKernel);
            //注入系统服务提供者
            ninjectKernel.Bind<IServiceProvider>().ToConstant(ninjectKernel);

            //要支持Oralce数据库，请在""中填写Oralce库的Schema名称
            //ninjectKernel.Rebind<ModelContext>().ToSelf()
            //   .WithPropertyValue("Schema", "");//.WithPropertyValue("Schema", "WEBFRAME");

            //如果要修改上传根目录，请恢复以下代码,并修改第二个参数
            //ninjectKernel.Rebind<IFileLocator>().To(typeof(FileLocator))
            //       .WithConstructorArgument("rootPath", "D:\\Temp"); 

            //如果要开启多标签，或修改默认皮肤，请恢复以下代码,并修改第二个参数
            ninjectKernel.Rebind<UserConfig>().ToSelf()
                .WithPropertyValue("ShowTab", false) //如果需要系统默认以多标签形式显示页，请设置为true
                .WithPropertyValue("Theme", "blue") //系统默认皮肤
                .WithPropertyValue("GridLineStyle", GridLineStyle.Horizental); //表格线设置

            //todo: 额外的注入代码
        }
    }
}