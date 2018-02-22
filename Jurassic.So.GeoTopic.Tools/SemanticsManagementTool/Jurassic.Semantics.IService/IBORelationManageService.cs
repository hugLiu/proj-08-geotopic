using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jurassic.Semantics.IService.ViewModel;

namespace Jurassic.Semantics.IService
{
    public interface IBoRelationManageService
    {
        /// <summary>
        /// 添加业务对象关系
        /// </summary>
        /// <param name="bor"></param>
        void AddBoRelation(BoRelationModel bor);

        /// <summary>
        /// 更新业务对象关系
        /// </summary>
        /// <param name="boRelation"></param>
        void UpdateBoRelation(BoRelationModel boRelation);

        /// <summary>
        /// 删除业务对象关系
        /// </summary>
        /// <param name="bor"></param>
        void DeleteBoRelation(BoRelationModel bor);

        /// <summary>
        /// 获得业务对象关系类星
        /// </summary>
        /// <returns></returns>
        List<BoRelationTypeModel> GetBoRelationType();

        /// <summary>
        /// 根据对象1，和对象2的对象类型获得 
        /// </summary>
        /// <returns></returns>
        List<BoRelationModel> GetId2ById1AndBot2(string id, string bot2);
    }
}
