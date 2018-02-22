using Jurassic.Semantics.EFProvider;
using Jurassic.Semantics.IService;
using Jurassic.Semantics.IService.ViewModel;
using System.Collections.Generic;
using System.Linq;

namespace Jurassic.Semantics.Service
{
    public class KeyWordsManageService : IKeyWordsManageService
    {
        private KeyWordProvider _efProvider;

        public KeyWordsManageService(KeyWordProvider efProvider)
        {
            _efProvider = efProvider;
        }

        public List<BPAndPTTreeModel> GetBPPTTree(string id, bool show)
        {
            return _efProvider.GetBpAndPtTree(id, show).Select(AutoMapper.Mapper.Map<BPAndPTTreeModel>)
                .OrderBy(o => o.OrderIndex).ThenBy(o => o.CreateDate).ToList();
        }

        public void AddKeyWords(string id, string userName, Dictionary<string, int> keyWordsAndOrder)
        {
            _efProvider.InsertKeyWord(id, userName, keyWordsAndOrder);
        }

        public void UpdateKeyWords(string id, string userName, Dictionary<string, int> keyWordsAndOrder)
        {
            _efProvider.UpdateKeyWord(id, userName, keyWordsAndOrder);
        }

        public List<TermKeyWords> GetKeyWords(string id)
        {
            return _efProvider.GetKeyWords(id).Select(AutoMapper.Mapper.Map<TermKeyWords>).OrderBy(o => o.OrderIndex).ToList();
        }

        public void DeleteKeyWords(string id, List<string> words)
        {
            _efProvider.DeleteKeyWord(id,words);
        }
    }
}
