using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jurassic.Semantics.EFProvider;
using Jurassic.Semantics.Entity.EntityModel;
using Jurassic.Semantics.EntityNew;
using Jurassic.Semantics.IService;
using Jurassic.Semantics.IService.ViewModel;

namespace Jurassic.Semantics.Service
{
    public class BoRelationManageService : IBoRelationManageService
    {
        private readonly BoRelationEfProvider _efProvider;

        public BoRelationManageService(BoRelationEfProvider efProvider)
        {
            _efProvider = efProvider;
        }

        public void AddBoRelation(BoRelationModel bor)
        {
            _efProvider.Add(AutoMapper.Mapper.Map<BO_Relation>(bor));
        }

        public void UpdateBoRelation(BoRelationModel bor)
        {
            _efProvider.Update(AutoMapper.Mapper.Map<BO_Relation>(bor));
        }

        public void DeleteBoRelation(BoRelationModel bor)
        {
            _efProvider.Delete(AutoMapper.Mapper.Map<BO_Relation>(bor));
        }

        public List<BoRelationTypeModel> GetBoRelationType()
        {
            throw new NotImplementedException();
        }

        public List<BoRelationModel> GetId2ById1AndBot2(string id, string bot2)
        {
            throw new NotImplementedException();
        }

        //public List<BoRelationTypeModel> GetBoRelationType()
        //{
        //    return _efProvider.GetBoRelationType().Select(AutoMapper.Mapper.Map<BoRelationTypeModel>).ToList();
        //}

        //public List<BoRelationModel> GetId2ById1AndBot2(string id, string bot2)
        //{
        //    return _efProvider.GetId2ById1AndBot2(id, bot2).Select(AutoMapper.Mapper.Map<BoRelationModel>).ToList();
        //}
    }
}
