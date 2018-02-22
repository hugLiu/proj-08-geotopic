//using System.Web.Optimization;

using System.Web.Mvc;
using Jurassic.CommonModels.EFProvider;
using Jurassic.Semantics.IService;
using Jurassic.Semantics.Service.Mapper;
//using Ninject;
//using Jurassic.CommonModels.Organization;
//using Jurassic.CommonModels;
//using Jurassic.CommonModels.EFProvider;
using Jurassic.WebFrame;
//using Jurassic.WebFrame.Helpers;
using Jurassic.Semantics.Service;
using Ninject;

namespace Jurassic.SemanticsManagement
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    /// <summary>
    /// 应用程序必须继承MvcApplication
    /// </summary>
    public class Application1 : MvcApplication
    {
        //protected string[] ControllerNamespaces
        //{
        //    get
        //    {
        //        //声明自身Controller所在的命名空间
        //        return new string[]
        //        {
        //            typeof (Jurassic.SemanticsManagement.Application1).Namespace + ".Controllers",
        //            "Jurassic.WebUpload.Controllers",
        //            "Jurassic.WebReport.Controllers"
        //        };
        //    }
        //}

        protected override void Application_Start()
        {
            base.Application_Start();

            //todo: 额外的初始化代码
            AutoMapperInitialization.Initialization();

        }
        protected void RedefineViewLocator()
        {
            var viewEngine = (ViewEngines.Engines[0] as RazorViewEngine);
            if (viewEngine == null) return;
            viewEngine.ViewLocationFormats =
                new string[] { 
                    "~/Views/{1}/{0}.cshtml", 
                    "~/Views/Shared/{0}.cshtml",
                    "~/Views/Demo/{1}/{0}.cshtml"};
        }

        protected override void AddBindings(IKernel ninjectKernel)
        {
            base.AddBindings(ninjectKernel);

            ninjectKernel.Rebind<ModelContext>().ToSelf()
                .WithPropertyValue("Schema", ""); //要支持Oralce数据库，需要""中是Oralce库的Schema名称

            //todo: 额外的注入代码 
            ninjectKernel.Bind<IBOTManageService>().To<BOTManagementService>();
            ninjectKernel.Bind<IBoManageService>().To<BoManageService>();
            ninjectKernel.Bind<IBoRelationManageService>().To<BoRelationManageService>();

            ninjectKernel.Bind<ICcTermService>().To<CcTermService>();
            ninjectKernel.Bind<IGlossaryManageService>().To<GlossaryManageService>();
            ninjectKernel.Bind<IKeyWordsManageService>().To<KeyWordsManageService>();
            ninjectKernel.Bind<ISemanticsManageService>().To<SemanticsManageService>();
            ninjectKernel.Bind<ISyncToElasticSearch>().To<SyncToElasticSearch>();
            ninjectKernel.Bind<ISemanticsManageService>().To<SemanticsManageService>();
            ninjectKernel.Bind<IConceptClassService>().To<ConceptClassService>();
            ninjectKernel.Bind<ISemanticsTypeService>().To<SemanticsTypeService>();

        }
    }
}