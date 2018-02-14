using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.So.GeoTopic.DataService.Models;
using Jurassic.So.GeoTopic.Database.Service;
using Jurassic.So.GeoTopic.Database;
using System.Data.Entity;
using Jurassic.So.GeoTopic.Database.Models;
using Jurassic.So.GeoTopic.DataService.Tool;

namespace Jurassic.So.GeoTopic.DataService.Service.Implementation
{
    public class webpages_RolesService : Iwebpages_RolesService
    {
        public Iwebpages_RoleEFRepository Webpages_Roles;
        public IUserProfileEFRepository UserProfile;
        public IGT_TopicEFRepository GtTopic;
        public webpages_RolesService(Iwebpages_RoleEFRepository webpages_Roles, IUserProfileEFRepository userProfile, IGT_TopicEFRepository gtTopic)
        {
            Webpages_Roles = webpages_Roles;
            this.UserProfile = userProfile;
            this.GtTopic = gtTopic;
        }
        public List<webpages_RolesModel> GetRoleIndexs()
        {
            var Roles = Webpages_Roles.GetAll();
            return Roles.Select(AutoMapper.Mapper.Map<webpages_Roles, webpages_RolesModel>).ToList();
        }


        public List<UserProfileModel> GetUsersByTopicId(string RoleId)
        {
            var _roleId = int.Parse(RoleId); //UserProfile
             var query = Webpages_Roles.GetQuery().Include("UserProfile").Where(t=>t.RoleId==_roleId).ToList();
            var Users = query[0].UserProfile;
            return Users.Select(AutoMapper.Mapper.Map<UserProfile,UserProfileModel>).ToList();
        }

        /// <summary>
        /// 通过角色的ID获取对应的权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public List<KTopicModel> GetTopicsByRoleId(string roleId)
        {
            var _roleId = int.Parse(roleId);
            var query = Webpages_Roles.GetQuery().Include("GT_Topic").Where(t => t.RoleId == _roleId).ToList();
            var query_Topics = query[0].GT_Topic;
            return query_Topics.Select(AutoMapper.Mapper.Map<GT_Topic, KTopicModel>).ToList();
        }

        /// <summary>
        /// 保存权限的更改
        /// </summary>
        /// <param name="RoleId"></param>
        /// <param name="topicIds"></param>
        public void CheckboxChange(int RoleId, string currentChecked)
        {
            var _currentChecked = currentChecked;
            //这个是最新获取的topicIds 必须把数据库中的实体进行处理
            List<GT_Topic> models = JsonUtil.JsonToObject(currentChecked, typeof(List<GT_Topic>)) as List<GT_Topic>;
            //.Include("Geo_kTopic")
           
                var _roleId = RoleId;

                var a = Webpages_Roles.GetQuery().Include("GT_Topic").FirstOrDefault(t => t.RoleId == _roleId);
                //models里面有的tops里面没有的要加进去 models里面没有的tops里面有的要删除掉
                //当mids为空的时候会报错
                var mids = models.Select(m => m.Id).ToArray();  //权限树中选中用户的Id
                                                                //var tops = a.Geo_kTopic.Select(n => n.Id).ToArray();  //数据库中当前用户下的Id
                if (mids.Length == 0)
                {
                    a.GT_Topic.Clear();
                }
                else
                {
                //a.GT_Topic = this.GtTopic.FindList<GT_Topic>(e => mids.Contains(e.Id), t=>t.Id, 0).ToList();
                a.GT_Topic = this.GtTopic.FindList(e => mids.Contains(e.Id), t=>t.Id, 0).ToList();
            }
            Webpages_Roles.Update(a);
            
        }
    }
}
