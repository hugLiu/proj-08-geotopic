using System.Collections;
using Jurassic.Semantics.EntityNew;
using Jurassic.Semantics.IService.ViewModel;
using System.Collections.Generic;

namespace Jurassic.Semantics.IService
{
    public interface IPtContextManageService
    {  /// <summary>
        /// 获得与pt存在关系的其他概念类和语义关系类型
        /// </summary>
        /// <returns></returns>
        List<PtRelations> GetPtRelations();
        /// <summary>
        /// 获得某一概念类的分组统计（在pt上下文中）
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        IEnumerable GetTermGroup(string pt, string field);
        /// <summary>
        /// 获得pt上下文
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="filterItems"></param>
        /// <param name="field"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        List<SMT_PTContextView> FilterPtContext(string pt, string filterItems, string field, int pageIndex, int pageSize);
        /// <summary>
        /// 获得pt上下文，可带过滤条件 的个数
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="filterItems"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        int GetFilterPtContextCount(string pt, string filterItems, string field);
        /// <summary>
        /// 保存修改的pt上下文关系
        /// </summary>
        /// <param name="ptId"></param>
        /// <param name="pt"></param>
        /// <param name="sr"></param>
        /// <param name="ccId"></param>
        /// <param name="ccTerm"></param>
        /// <param name="userName"></param>
        void SavePtContext(string ptId, string pt, string sr, string ccId, string ccTerm, string userName);

        /// <summary>
        /// 获得指定概念类对应的树
        /// </summary>
        /// <param name="cc"></param>
        /// <returns></returns>
        List<TreeModel> GetTermTree(string cc);

        /// <summary>
        /// 删除pt指定的语义关系
        /// </summary>
        /// <param name="ptid"></param>
        /// <param name="sr"></param>
        /// <param name="term"></param>
        void DeletePtSemantics(string ptid,string sr, string term);

        object GetPtRelationsOfCc(string ptid, string sr);
    }
}
