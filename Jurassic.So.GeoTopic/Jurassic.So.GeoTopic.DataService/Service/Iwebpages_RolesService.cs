using Jurassic.So.GeoTopic.DataService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.GeoTopic.DataService.Service
{
    public interface Iwebpages_RolesService
    {
        /// <summary>
        /// 拿到角色表的所有数据
        /// </summary>
        /// <returns></returns>
        List<webpages_RolesModel> GetRoleIndexs();

        /// <summary>
        /// 通过角色Id拿到所有的用户
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        List<UserProfileModel> GetUsersByTopicId(string RoleId);

        /// <summary>
        /// 通过角色Id拿到当前角色下的所有权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        List<KTopicModel> GetTopicsByRoleId(string RoleId);

        /// <summary>
        /// 用于保存权限的更改
        /// </summary>
        /// <param name="RoleId"></param>
        /// <param name="topicIds"></param>
        void CheckboxChange(int RoleId, string currentChecked);
    }
}
