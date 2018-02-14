
using System.Collections.Generic;
using System.Web.Mvc;
using Jurassic.So.GeoTopic.DataService;
using Jurassic.So.GeoTopic.DataService.Tool;
using Ninject;
using Jurassic.CommonModels.EFProvider;
using Jurassic.So.GeoTopic.Database.Service;
using Jurassic.WebFrame;
using System.Data.Entity;
using Jurassic.So.GeoTopic.Database;
using Jurassic.So.GeoTopic.DataService.Service;
using Jurassic.So.GeoTopic.DataService.Service.Implementation;
using Jurassic.So.GeoTopic.Database.Service.Implementation;
using Jurassic.So.Infrastructure;
using Ninject.Web.Common;
using Jurassic.CommonModels;
using Ninject.Web.Mvc;
using System.Linq;
using System;

namespace Jurassic.So.GeoTopic.Web
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    /// <summary>
    /// 应用程序必须继承MvcApplication
    /// </summary>
    public class GTApplication : MvcApplication
    {
        protected override IEnumerable<string> ControllerNameSpaces
        {
            get
            {
                var list = base.ControllerNameSpaces.ToList();
                list.Add(typeof(GTApplication).Namespace + ".Controllers");
                return list;
            }
            //get
            //{
            //    //声明自身Controller所在的命名空间
            //    return new string[] { typeof(Application1).Namespace + ".Controllers" };
            //}
        }

        protected override void Application_Start()
        {
            base.Application_Start();

            //todo: 额外的初始化代码
            // RedefineViewLocator();
            //初始化映射模型
            AutoMapperUtil.LoadConfig();
        }

        /// <summary>
        /// 追加或重新定义视图搜索路径
        /// </summary>
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
        /// <summary>
        /// 加入注入服务绑定
        /// </summary>
        protected override void AddBindings(IKernel ninjectKernel)
        {
            base.AddBindings(ninjectKernel);
            //注入系统服务提供者
            ninjectKernel.Bind<IServiceProvider>().ToConstant(ninjectKernel);

            //要支持Oralce数据库，请在""中填写Oralce库的Schema名称
            ninjectKernel.Rebind<ModelContext>().ToSelf()
              .WithPropertyValue("Schema", "");

            //如果要修改上传根目录，请恢复以下代码,并修改第二个参数
            //ninjectKernel.Rebind<IFileLocator>().To(typeof(FileLocator))
            //       .WithConstructorArgument("rootPath", "D:\\Temp"); 

            //如果要开启多标签，或修改默认皮肤，请恢复以下代码,并修改第二个参数
            //ninjectKernel.Rebind<UserConfig>().ToSelf()
            //.WithPropertyValue("ShowTab", false) //如果需要系统默认以多标签形式显示页，请设置为true
            //.WithPropertyValue("Theme", "blue");  //系统默认皮肤

            //todo: 额外的注入代码
            ninjectKernel.Bind<GeoDbContext>().ToSelf().InRequestScope();
            ninjectKernel.Bind<IPageDetailsService>().To<PageDetailsService>().InRequestScope();
            ninjectKernel.Bind<IDicMaintainService>().To<DicMaintainService>().InRequestScope();
            ninjectKernel.Bind<IImportParamsService>().To<ImportParamsService>().InRequestScope();
            ninjectKernel.Bind<IGT_TopicEFRepository>().To<GT_TopicEFRepository>().InRequestScope();
            ninjectKernel.Bind<IGT_TopicIndexEFRepository>().To<GT_TopicIndexEFRepository>().InRequestScope();
            ninjectKernel.Bind<IGT_IndexDefinitionEFRepository>().To<GT_IndexDefinitionEFRepository>().InRequestScope();
            ninjectKernel.Bind<IGT_TopicCardEFRepository>().To<GT_TopicCardEFRepository>().InRequestScope();
            ninjectKernel.Bind<IGT_UrlTemplateEFRepository>().To<GT_UrlTemplateEFRepository>().InRequestScope();
            ninjectKernel.Bind<IGeoTopicService>().To<GeoTopicService>().InRequestScope();
            ninjectKernel.Bind<IUserProfileEFRepository>().To<UserProfileEFRepository>().InRequestScope();
            ninjectKernel.Bind<IUserProfileService>().To<UserProfileService>().InRequestScope();
            ninjectKernel.Bind<IGT_RenderTypeEFRepository>().To<GT_RenderTypeEFRepository>().InRequestScope();
            ninjectKernel.Bind<IGT_RenderUrlEFRepository>().To<GT_RenderUrlEFRepository>().InRequestScope();
            ninjectKernel.Bind<IDictTypeService>().To<DictTypeService>().InRequestScope();
            ninjectKernel.Bind<Iwebpages_RolesService>().To<webpages_RolesService>().InRequestScope();
            ninjectKernel.Bind<Iwebpages_RoleEFRepository>().To<webpages_RoleEFRepository>().InRequestScope();
            ninjectKernel.Bind<IGT_RemarkEFRepository>().To<GT_RemarkEFRepository>().InRequestScope();
            ninjectKernel.Bind<IRemarkService>().To<RemarkService>().InRequestScope();
            ninjectKernel.Bind<IWrapedDataSearchService>().To<WrapedDataSearchService>().InRequestScope();
            ninjectKernel.Bind<IUserBehaviorService>().To<UserBehaviorService>().InRequestScope();
            ninjectKernel.Bind<IUserBehaviorEFRepository>().To<UserBehaviorEFRepository>().InRequestScope();
            ninjectKernel.Bind<IGT_CodeEFRepository>().To<GT_CodeEFRepository>().InRequestScope();
            ninjectKernel.Bind<IGT_CodeTypeEFRepository>().To<GT_CodeTypeEFRepository>().InRequestScope();
            ninjectKernel.Bind<ICodeOperateService>().To<CodeOperateService>().InRequestScope();
        }
    }
}