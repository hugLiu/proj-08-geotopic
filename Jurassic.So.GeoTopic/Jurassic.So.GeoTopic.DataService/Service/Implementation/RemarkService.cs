using Jurassic.So.GeoTopic.Database.Models;
using Jurassic.So.GeoTopic.Database.Service;
using Jurassic.So.GeoTopic.DataService.Models;
using Jurassic.So.GeoTopic.DataService.Tool;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.So.GeoTopic.DataService.Service.Implementation
{
    public class RemarkService:IRemarkService
    {
        public IGT_RemarkEFRepository RemarkContent;
        public IUserProfileEFRepository UserProfileContent;
        public RemarkService(IGT_RemarkEFRepository remarkContent, IUserProfileEFRepository userProfile)
        {
            RemarkContent = remarkContent;
            UserProfileContent = userProfile;
        }
        //查询评论列表
        public Pagination QueryRemark(string scope, string key, int index, int size,string filter,int userId)
        {
            var remarkList =from e in RemarkContent.GetAll()
                            where e.Scoap==scope&&e.NatureKey==key
                            select e;
            var count = remarkList.ToList().Count();
            Pagination pageData = new Pagination();
            pageData.total = count;
            IEnumerable<GT_Remark> list;
            if (filter== "Newest")
            {
               list = from a in remarkList
                             where (DateTime.Now.Subtract(a.CreatedDate)).Days <1
                             select a;
                           
            }else if (filter=="My")
            {
                list = from b in remarkList
                           where b.UserId == userId
                           select b;            
            }
            else
            {
                list = remarkList;
            }
            var modelList = list.Select(AutoMapper.Mapper.Map<GT_Remark, RemarkModel>).ToList();
            pageData.data = modelList.OrderByDescending(o => o.CreatedDate).Skip(index).Take(size);
            return pageData;
        }
        /// <summary>
        /// 发表评论
        /// </summary>
        /// <param name="model"></param>
        public void AddRemark(RemarkModel model)
        {
            if (model != null)
            {
                GT_Remark addModel = new GT_Remark();
                addModel.Comment = model.Comment;
                addModel.CreatedBy = UserProfileContent.Find(o => o.UserId == model.UserId).UserName;
                addModel.UserId = model.UserId;
                addModel.Scoap = model.Scoap;
                addModel.NatureKey = model.NatureKey;
                addModel.CreatedDate = DateTime.Now;
                addModel.UpdatedDate =DateTime.Now;
                addModel.IsDelete = false;
                RemarkContent.Add(addModel);
            }
        }
        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="model"></param>
        public void DeleteRemark(int id)
        {           
            var model = RemarkContent.GetQuery().Include("UserProfile").FirstOrDefault(o=>o.Id==id);
            var users = model.UserProfile1;
            if (users.Count > 0) model.UserProfile1.Clear();
            RemarkContent.Delete(model);
        }
        /// <summary>
        /// 赞
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <param name="praise"></param>
        public void PraiseRemark(int id, int userId, bool praise)
        {

            var remark = RemarkContent.GetQuery().Include("UserProfile").FirstOrDefault(o=>o.Id==id);
            var userpro = remark.UserProfile1;
            var user = UserProfileContent.Find(o => o.UserId == userId);
            if (praise)
            {
                userpro.Add(user);
            }
            else
            {
                userpro.Remove(user);
            }
            RemarkContent.Update(remark);
        }
    }
}
