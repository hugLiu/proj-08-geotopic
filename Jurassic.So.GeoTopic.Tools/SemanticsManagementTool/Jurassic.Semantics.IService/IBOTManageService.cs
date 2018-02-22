using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jurassic.Semantics.IService.ViewModel;

namespace Jurassic.Semantics.IService
{
    public interface IBOTManageService
    {
        List<BOTModel> GetBotbyId(string id);

        void InsertBot(string bot, string userName, string typeName, int orderIndex, string description, string remark);

        void UpdateBot(string id, string userName, string bot, string typeName, int orderIndex,string description,string remark);

        void DeleteBot(string id);
    }
}
