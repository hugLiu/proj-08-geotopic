using Jurassic.So.GeoTopic.DataService;
using Jurassic.So.GeoTopic.DataService.Service;
using Jurassic.WebFrame;
using System.Web.Mvc;



namespace Jurassic.So.GeoTopic.Web.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    public class GeoTopicInUserController : BaseController
    {
        /// <summary>
        /// 用户表的接口
        /// </summary>
        public IUserProfileService userProfileService { get; set; }
        /// <summary>
        /// 角色表接口
        /// </summary>
        public Iwebpages_RolesService webpages_RolesService { get; set; }
        /// <summary>
        /// 权限树的接口
        /// </summary>
        public IPageDetailsService pageDetailsService { get; set; }
        /// <summary>
        /// 用户权限表的接口
        /// </summary>
        //public ITopicInUsersService topicDetailsService { get; set; }

        #region 构造函数 + GeoTopicInUserController(IUserProfileService userProfile, IPageDetailsService pageDetails, ITopicInUsersService topicInUser)
        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="userProfile"></param>
        /// <param name="pageDetails"></param>
        /// <param name="webpages_Roles"></param>
        public GeoTopicInUserController(IUserProfileService userProfile, IPageDetailsService pageDetails, Iwebpages_RolesService webpages_Roles)
        {
            userProfileService = userProfile;
            pageDetailsService = pageDetails;
            webpages_RolesService = webpages_Roles;
        }
        #endregion

        /// <summary>
        /// 主视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        #region 获取用户的全部信息 用户表 + GetUserMess()
        /// <summary>
        /// 获取用户的全部信息 用户表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetUserMess()
        {
            return Json(userProfileService.GetUserIndexs(), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 获得角色表的信息 + GetRoleMess()
        /// <summary>
        /// 拿到整个角色表的内容
        /// </summary>
        /// <returns></returns>
        public JsonResult GetRoleMess()
        {
            return Json(webpages_RolesService.GetRoleIndexs(), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 根据角色Id获取对应的用户 + GetUsersByRole(string RoleId)
        /// <summary>
        /// 根据角色Id获取对应的用户
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public JsonResult GetUsersByRole(string RoleId)
        {
            var Users = webpages_RolesService.GetUsersByTopicId(RoleId);
            return Json(Users, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 获取权限的所有信息 右边的树 + GetTopicsMess()
        /// <summary>
        /// 加载主题树信息 右边的树
        /// </summary>
        /// <returns></returns>
        public JsonResult GetTopicsMess()
        {
            return Json(pageDetailsService.GetGeoKTopics(), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 单击用户表的某一行之后，获取该用户的所有权限 并显示在右边的树上 + Edit(string id)
        /// <summary>
        /// 根据用户表的id来取出所有对应的权限
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult Edit(string id)
        {
            var role = webpages_RolesService.GetTopicsByRoleId(id);
            return Json(role, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 右边树的checkbox改变之后 对应的映射到数据库中 SaveTopicCheckbox(int RoleId, string currentChecked)
        /// <summary>
        /// 保存主题树 将checkbox的信息保存到用户权限表中
        /// </summary>
        /// <param name="RoleId"></param>
        /// <param name="currentChecked">当前的checkbox的选中信息---新的</param>
        public void SaveTopicCheckbox(int RoleId, string currentChecked)
        {
            webpages_RolesService.CheckboxChange(RoleId, currentChecked);
        } 
        #endregion
    }
}
