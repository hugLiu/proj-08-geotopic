using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.So.GeoTopic.DataService.Models;
using Jurassic.So.GeoTopic.Database.Service;
using Jurassic.So.GeoTopic.Database;
using Jurassic.So.GeoTopic.Database.Models;
using System.Data.Entity;
using Jurassic.So.GeoTopic.DataService.Tool;

namespace Jurassic.So.GeoTopic.DataService
{
    public class UserProfileService : IUserProfileService
    {
        public IUserProfileEFRepository UserProfile;
        public IGT_TopicEFRepository GeokTopic;
        public UserProfileService(IUserProfileEFRepository userProfile, IGT_TopicEFRepository geokTopic)
        {
            UserProfile = userProfile;
            this.GeokTopic = geokTopic;
        }

        public List<UserProfileModel> GetUserIndexs()
        {
            var userProfile = UserProfile.GetAll();
            return userProfile.Select(AutoMapper.Mapper.Map<UserProfile, UserProfileModel>).ToList();
        }


        //这个是当用户点击的时候，根据用户id获得选中的checkbox的值
        public List<KTopicModel> GetTopicsByUserId(string userId)
        {
            //var _userId = int.Parse(userId);
            //var query = UserProfile.GetQuery().Include("Geo_kTopic").Where(t=>t.UserId==_userId);
            //var result = query.ToList();
            //var result2 = result[0].Geo_kTopic;
            //return result2.Select(AutoMapper.Mapper.Map<Geo_kTopic, KTopicModel>).ToList();
            return null;
        }
    }
}
