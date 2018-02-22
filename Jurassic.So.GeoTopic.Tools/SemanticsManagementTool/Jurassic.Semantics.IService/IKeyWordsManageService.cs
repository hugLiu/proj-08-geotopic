using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jurassic.Semantics.IService.ViewModel;

namespace Jurassic.Semantics.IService
{
    public interface IKeyWordsManageService
    {
        List<BPAndPTTreeModel> GetBPPTTree(string id, bool showNoKeyWordPt);

        void AddKeyWords(string id, string userName, Dictionary<string, int> keyWordsAndOrder);

        void UpdateKeyWords(string id, string userName, Dictionary<string, int> keyWordsAndOrder);

        void DeleteKeyWords(string id, List<string> words);

        List<TermKeyWords> GetKeyWords(string id);
    }
}
