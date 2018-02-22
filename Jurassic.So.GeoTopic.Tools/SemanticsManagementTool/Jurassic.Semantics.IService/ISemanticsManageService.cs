using System.Collections.Generic;
using Jurassic.Semantics.IService.ViewModel;

namespace Jurassic.Semantics.IService
{
    public interface ISemanticsManageService
    {

        /// <summary>
        /// 获得语义关系 正向
        /// </summary>
        /// <param name="term">叙词</param>
        /// <param name="sr">语义关系</param>
        /// <returns></returns>
        List<CcTermModel> GetSemantics(string term, string sr);

        /// <summary>
        /// 获得语义关系 反向
        /// </summary>
        /// <param name="term">叙词</param>
        /// <param name="sr">语义关系</param>
        /// <returns></returns>
        List<CcTermModel> GetReverseSemantics(string term, string sr);


        /// <summary>
        /// 根据概念类名称 加载对应的树或是列表（pid全部为空）
        /// </summary>
        /// <param name="cc">概念类的名称</param>
        /// <returns>返回指定概念类的全部叙词并以树结构展示</returns>
        List<TermTreeModel> GetTermTrees(string cc);


        /// <summary>
        /// 获得需要维护的概念类
        /// </summary>
        /// <returns>返回字典key:概念类code,概念类中文名称</returns>
        Dictionary<string, string> GetNeedManageCC();

        /// <summary>
        /// 获得指定概念类需要维护的关系类型列表
        /// </summary>
        /// <param name="cc">概念类</param>
        /// <returns>返回：key:SR,value:对应的概念类（通过概念类来加载对应的树结构）</returns>
        List<SemanticsTypemodel> GetRelationOfCC(string cc);

        /// <summary>
        /// 添加数据到语义关系表
        /// </summary>
        /// <param name="semantics">语义关系列表</param>
        void SaveSemantics(List<SemanticsModel> semantics);

        /// <summary>
        /// 批量删除语义关系
        /// </summary>
        /// <param name="semantics"></param>

        void DeleteSemantics(List<SemanticsModel> semantics);

    }
}
