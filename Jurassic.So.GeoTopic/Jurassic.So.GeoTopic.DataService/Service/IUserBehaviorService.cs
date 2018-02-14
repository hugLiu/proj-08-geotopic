using Jurassic.So.GeoTopic.DataService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.GeoTopic.DataService.Service
{
    public interface IUserBehaviorService
    {
        ///leijing
        /// <summary>
        /// 通过用户Id拿到当前用户的下载历史信息
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        List<UserBehaviorModel> GetDownloadMessByUserId(int UserId, int count);

        /// <summary>
        /// 通过当前用户Id拿到当前用户的收藏信息
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        List<UserBehaviorModel> GetFavoriteMessByUserId(int UserId, int count);

        /// <summary>
        /// 通过当前用户Id拿到当前用户的收藏信息
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        List<UserBehaviorModel> GetFavoriteMessByUserId(int UserId);
        /// <summary>
        /// 根据id查询用户操作信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="count"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        List<UserBehaviorModel> QueryOptionMessByUserId(int userId, int count, string type);

        /// <summary>
        /// 根据操作id删除操作信息
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="userName"></param>
        void DeleteOptionMessById(int Id, string userName);
        /// <summary>
        /// 添加用户操作信息
        /// </summary>
        /// <param name="model"></param>
        void AddOptionMess(UserBehaviorModel model);
        /// <summary>
        /// 获得某类某页行为列表
        /// </summary>
        /// <param name="model"></param>
        List<UserBehaviorModel> GetList(int userId, string type, int from, int size);
    }
}
