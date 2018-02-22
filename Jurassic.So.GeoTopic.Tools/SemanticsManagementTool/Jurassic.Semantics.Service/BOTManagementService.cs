using System.Collections.Generic;
using Jurassic.Semantics.EFProvider;
using Jurassic.Semantics.IService;
using Jurassic.Semantics.IService.ViewModel;

namespace Jurassic.Semantics.Service
{
    public class BOTManagementService : IBOTManageService
    {
        private BOTEFProvider _efProvider;

        public BOTManagementService(BOTEFProvider efProvider)
        {
            _efProvider = efProvider;
        }
        public List<BOTModel> GetBotbyId(string id)
        {
            //return _efProvider.GetBotbyId(id).Select(AutoMapper.Mapper.Map<BOTModel>).ToList();
            return null;
        }

        public void InsertBot(string bot, string userName, string typeName, int orderIndex, string description, string remark)
        {
            //_efProvider.InsertBot(bot, userName, typeName, orderIndex,description, remark);
        }

        public void UpdateBot(string id, string userName, string bot, string typeName, int orderIndex,string description,string remark)
        {
            //_efProvider.UpdateBot(id, userName, bot, typeName, orderIndex, description,remark);
        }

        public void DeleteBot(string id)
        {
            //_efProvider.DeleteBot(id);
        }
    }
}
