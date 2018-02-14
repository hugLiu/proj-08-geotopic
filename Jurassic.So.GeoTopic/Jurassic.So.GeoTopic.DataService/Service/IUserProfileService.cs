using Jurassic.So.GeoTopic.DataService.Models;
using System.Collections.Generic;

namespace Jurassic.So.GeoTopic.DataService
{
    public interface IUserProfileService
    {
        /// <summary>
        /// 获取主题索引元数据名称集合
        /// </summary>
        /// <returns></returns>
        List<UserProfileModel> GetUserIndexs();

        /// <summary>
        /// 根据用户权限获取知识主题链信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<KTopicModel> GetTopicsByUserId(string userId);

    }
}
