using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jurassic.Semantics.IService.ViewModel;

namespace Jurassic.Semantics.IService
{
    public interface IBusinessFeatureManegementService
    {
        List<TermTreeModel> GetBpModelList();

        List<SMT_BOTTreeViewModel> GetBotTreeByNull();

        List<TermSourceModel> GetTermList(string termGuid);

        List<TermTranslationModel> GeTranslationList(Guid termclassid, string languageType);

        IEnumerable<string> GetBotTreeList(Guid termGuid);

        void AddSameLevel(CcTermModel newmodel);

        void AddChildrenNode(Guid termGuid, CcTermModel newmodel);
        void DeleteById(TermTreeModel getBpTreemodel);

        void EditTreeById(Guid termGuid, string term, string path);

        void SaveMs(string strMs, Guid termGuid);

        void AddTs(TermSourceModel termSourceModel);

        void SaveSource(string source, Guid termGuid);

        void DeleteTermSourcrRowByName(string source);

        void SaveOtherName(TermTranslationModel tsTranslationModel);

        void DeleteRow(Guid tuid, string tran);

        void Add(SemanticsModel sdSemanticsModel);

        void DeleteBotByGuid(Guid Lguid, Guid Fguid);

        void DragNodeService(Guid dragNodeGuid, SemanticsModel dragNodeSemantics,
            SemanticsModel dropSemantics, string parentTerm);
    }
}
