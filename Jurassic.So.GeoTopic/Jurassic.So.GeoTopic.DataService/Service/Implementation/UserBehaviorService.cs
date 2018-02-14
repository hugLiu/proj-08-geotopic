using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jurassic.So.GeoTopic.DataService.Models;
using Jurassic.So.GeoTopic.Database.Service;
using System.Data.Entity;
using Jurassic.So.GeoTopic.Database;
using Jurassic.So.Infrastructure;

namespace Jurassic.So.GeoTopic.DataService.Service.Implementation
{
    public class UserBehaviorService : IUserBehaviorService
    {
        private IUserBehaviorEFRepository UserBehavior { get; set; }
        private IUserProfileEFRepository UserProfile { get; set; }
        public UserBehaviorService(IUserBehaviorEFRepository userBehavior, IUserProfileEFRepository userProfile)
        {
            UserBehavior = userBehavior;
            UserProfile = userProfile;

        }
        /// <summary>
        /// 通过用户Id拿到当前用户的所有下载历史信息
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public List<UserBehaviorModel> GetDownloadMessByUserId(int userId, int count)
        {
            return QueryOptionMessByUserId(userId, count, UserBehaviorType.Download);
        }

        /// <summary>
        /// 通过用户Id拿到当前用户的收藏信息
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public List<UserBehaviorModel> GetFavoriteMessByUserId(int userId, int count)
        {
            return QueryOptionMessByUserId(userId, count, UserBehaviorType.Favorite);
        }

        /// <summary>
        /// 通过用户Id拿到当前用户的所有收藏信息
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public List<UserBehaviorModel> GetFavoriteMessByUserId(int userId)
        {
            return UserBehavior.GetQuery()
                .Where(t => !t.IsDelete && t.UserId == userId && t.BehaviorType == UserBehaviorType.Favorite)
                .MapTo<UserBehaviorModel>()
                .ToList();
        }

        /// <summary>
        /// 根据用户id查询用户操作信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="count"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<UserBehaviorModel> QueryOptionMessByUserId(int userId, int count, string type)
        {
            return UserBehavior.GetQuery()
                .Where(o => !o.IsDelete && o.UserId == userId && o.BehaviorType == type)
                .OrderBy(t => t.UpdatedDate)
                .Take(count)
                .MapTo<UserBehaviorModel>()
                .ToList();
        }
        /// <summary>
        /// 根据操作id和用户id删除操作信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        public void DeleteOptionMessById(int id, string userName)
        {
            var model = UserBehavior.Find(o => !o.IsDelete && o.Id == id);
            if (model == null) return;
            model.IsDelete = true;
            model.UpdatedBy = userName;
            model.UpdatedDate = DateTime.Now;
            UserBehavior.Update(model);
        }
        /// <summary>
        /// 添加用户操作信息
        /// </summary>
        /// <param name="model"></param>
        public void AddOptionMess(UserBehaviorModel model)
        {
            GT_UserBehavior entity = null;
            if (model.BehaviorType == UserBehaviorType.Favorite)
            {
                if (model.Id > 0)
                {
                    entity = UserBehavior.Find(o => !o.IsDelete && o.Id == model.Id);
                }
                if (entity == null)
                {
                    entity = UserBehavior.Find(o => !o.IsDelete && o.UserId == model.UserId && o.BehaviorType == model.BehaviorType && o.Content == model.Content);
                }
                if (entity != null) return;
            }
            entity = model.MapTo<GT_UserBehavior>();
            UserBehavior.Add(entity);
            model.Id = entity.Id;
        }
        /// <summary>
        /// 获得某类某页行为列表
        /// </summary>
        /// <param name="model"></param>
        public List<UserBehaviorModel> GetList(int userId, string type, int from, int size)
        {
            return UserBehavior.GetQuery()
                .Where(o => !o.IsDelete && o.UserId == userId && o.BehaviorType == type)
                .OrderByDescending(t => t.CreatedDate)
                .Skip(from)
                .Take(size)
                .MapTo<UserBehaviorModel>()
                .ToList();
        }
    }
}
