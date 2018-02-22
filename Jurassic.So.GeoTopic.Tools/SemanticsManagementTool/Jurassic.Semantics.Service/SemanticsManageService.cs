using Jurassic.Semantics.EFProvider;
using Jurassic.Semantics.EntityNew;
using Jurassic.Semantics.IService;
using Jurassic.Semantics.IService.ViewModel;
using System.Collections.Generic;
using System.Linq;

namespace Jurassic.Semantics.Service
{
    public class SemanticsManageService : ISemanticsManageService
    {
        private SemanticsRelationEfProvider _efProvider;

        public SemanticsManageService(SemanticsRelationEfProvider efProvider)
        {
            _efProvider = efProvider;
        }

        public List<CcTermModel> GetSemantics(string term, string sr)
        {
            return _efProvider.GetSemantics(term, sr).Select(AutoMapper.Mapper.Map<CcTermModel>).ToList();
        }

        public List<CcTermModel> GetReverseSemantics(string term, string sr)
        {
            return _efProvider.GetReverseSemantics(term, sr).Select(AutoMapper.Mapper.Map<CcTermModel>).ToList();
        }

        public List<TermTreeModel> GetTermTrees(string cc)
        {
            return _efProvider.GetTermTrees(cc).Select(AutoMapper.Mapper.Map<TermTreeModel>).ToList();
        }

        public Dictionary<string, string> GetNeedManageCC()
        {
            return _efProvider.GetNeedManageCC();
        }

        public List<SemanticsTypemodel> GetRelationOfCC(string cc)
        {
            return _efProvider.GetRelationOfCC(cc).Select(AutoMapper.Mapper.Map<SemanticsTypemodel>).ToList();
        }

        public void SaveSemantics(List<SemanticsModel> semantics)
        {
            _efProvider.SaveSemantics(semantics.Select(AutoMapper.Mapper.Map<SD_Semantics>).ToList());
        }
        public void DeleteSemantics(List<SemanticsModel> semantics)
        {
            _efProvider.DeleteSemantics(semantics.Select(AutoMapper.Mapper.Map<SD_Semantics>).ToList());
        }
    }
}
