using Jurassic.So.GeoTopic.DataService.Models;
using Jurassic.So.GeoTopic.DataService.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.GeoTopic.DataService.Service
{
    public interface IRemarkService
    {
        /// <summary>
        /// 根据条件查询评论列表
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="key"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="filter"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Pagination QueryRemark(string scope,string key,int index,int size,string filter,int userId);
        /// <summary>
        ///发表评论
        /// </summary>
        /// <param name="model"></param>
        void AddRemark(RemarkModel model);
        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="model"></param>
        void DeleteRemark(int id);
        /// <summary>
        /// 点赞或取消赞
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <param name="praise"></param>
        void PraiseRemark(int id, int userId,bool praise);
    }
}
