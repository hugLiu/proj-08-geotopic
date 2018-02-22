using System;
using System.Collections.Generic;
using Jurassic.Semantics.IService.ViewModel;

namespace Jurassic.Semantics.IService
{
    public interface IGlossaryManageService
    {
        List<TermTreeModel> GetTermTree(string strccTerm);
        List<TermSourceModel> GetSourceList(string strccTerm);
        List<string> GetSourceListService(string termId);
        List<CcTermModel> GetTermList(string cc);
        void AddChildrenNode(Guid termGuid, CcTermModel newmodel, string textterm);

        void DeleteCcTermByIdService(Guid termclassid);

        void EditTreeById(Guid termGuid, string term, string path);

        void SaveMs(string strMs, Guid termGuid);

        void AddSource(TermSourceModel termSourceModel, string textvalue);

        void SaveSource(string source, Guid termGuid);

        void DeleteTermSourcrRowByName(string source);

        void SaveOtherName(TermTranslationModel tsTranslationModel);

        void DeleteRow(Guid tuid, string tran);
        void AddAlias(SemanticsModel model);
        void DeleteAlias(SemanticsModel model);

        void DragNodeService(SemanticsModel semantics, CcTermModel dragmodel, string textValue);
        void UpdatePathTerm(Guid termClassId);
        void SequenceService(Guid nodeId, string parentName, int newOrderIndex);

        List<string> GetidlistService(string termclassid);

        /// <summary>
        /// 获得概念类数据，形成下拉选项
        /// </summary>
        /// <returns></returns>
        Dictionary<string, string> GetAllConceptClass();

        /// <summary>
        /// 获得某个叙词的别名列表
        /// </summary>
        /// <param name="id">叙词ID</param>
        /// <returns></returns>
        List<SemanticsModel> GetTermAlias(Guid id);

        /// <summary>
        /// 获得某个叙词的译文列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<TermTranslationModel> GetTermTraslations(Guid id);
    }
}
