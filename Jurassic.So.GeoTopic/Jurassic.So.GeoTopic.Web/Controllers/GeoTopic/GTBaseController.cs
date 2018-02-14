using Jurassic.PKS.Service;
using Jurassic.So.GeoTopic.DataService.Models;
using Jurassic.So.Infrastructure;
using Jurassic.WebFrame;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Jurassic.So.Business;
using Jurassic.AppCenter;
using Jurassic.So.GeoTopic.DataService.Service;
using System;

namespace Jurassic.So.GeoTopic.Web.Controllers
{
    /// <summary>
    /// GT基类控制器
    /// </summary>
    public abstract class GTBaseController : BaseController
    {
        /// <summary>构造函数</summary>
        protected GTBaseController() { }

        #region 服务成员
        /// <summary>服务提供者</summary>
        /// <remarks>不要使用MVC控制器提供的Resolver,内部使用的是缓存未使用Scope</remarks>
        protected IDependencyResolver ServiceProvider { get { return DependencyResolver.Current; } }
        /// <summary>获得注入服务</summary>
        protected TService GetService<TService>()
        {
            return (TService)this.ServiceProvider.GetService(typeof(TService));
        }
        #endregion

        #region 权限管理
        /// <summary>当前用户默认角色ID</summary>
        protected string CurrentUserRoleId
        {
            get { return this.CurrentUser.RoleIds.First(); }
        }
        /// <summary>当前用户默认角色名称</summary>
        protected string CurrentUserRoleName
        {
            get { return GetUserRoleName(this.CurrentUserRoleId); }
        }
        /// <summary>获取角色名称</summary>
        protected string GetUserRoleName(string roleId)
        {
            return AppManager.Instance.RoleManager.GetById(roleId).Name;
        }
        /// <summary>获取角色ID</summary>
        protected string GetUserRoleId(string roleName)
        {
            return AppManager.Instance.RoleManager.GetByName(roleName).Id;
        }
        /// <summary>获取全部角色</summary>
        protected IList<AppRole> GetRoles()
        {
            return AppManager.Instance.RoleManager.GetAll();
        }
        /// <summary>获取全部角色名称</summary>
        protected string[] GetRoleNames()
        {
            return GetRoles().Select(e => e.Name).ToArray();
        }
        #endregion

        #region 用户行为
        /// <summary>
        /// 添加用户操作信息
        /// </summary>
        protected UserBehaviorModel AddUserBehavior(string behaviorType, string title, string id)
        {
            var content = new Dictionary<string, string>();
            content["id"] = id;
            return AddUserBehavior(behaviorType, title, content);
        }
        /// <summary>
        /// 添加用户操作信息
        /// </summary>
        protected UserBehaviorModel AddUserBehavior(string behaviorType, string title, object content)
        {
            var service = GetService<IUserBehaviorService>();
            var model = new UserBehaviorModel();
            model.BehaviorType = behaviorType;
            model.Type = "list";
            model.Title = title;
            model.Content = content.ToJson();
            model.UserId = this.CurrentUserId.ToInt32();
            model.CreatedBy = this.CurrentUser.Name;
            model.CreatedDate = DateTime.Now;
            model.UpdatedBy = this.CurrentUser.Name;
            model.UpdatedDate = DateTime.Now;
            service.AddOptionMess(model);
            return model;
        }
        /// <summary>
        /// 添加用户操作信息
        /// </summary>
        protected UserBehaviorModel AddUserBehavior(UserBehaviorModel model)
        {
            var service = GetService<IUserBehaviorService>();
            model.UserId = this.CurrentUserId.ToInt32();
            model.CreatedBy = this.CurrentUser.Name;
            model.CreatedDate = DateTime.Now;
            model.UpdatedBy = this.CurrentUser.Name;
            model.UpdatedDate = DateTime.Now;
            service.AddOptionMess(model);
            return model;
        }
        #endregion
    }
}