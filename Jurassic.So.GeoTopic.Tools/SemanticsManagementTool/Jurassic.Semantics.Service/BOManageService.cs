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
    public class BoManageService : IBoManageService
    {
        private readonly BoBaseInfoEfProvider _efProvider;
        private readonly BoAliasEfProvider _aliasProvider;

        public BoManageService(BoBaseInfoEfProvider efProvider, BoAliasEfProvider aliasProvider)
        {
            _efProvider = efProvider;
            _aliasProvider = aliasProvider;
        }

        public void AddBo(BoBaseInfoModel bo)
        {
            if (bo == null) throw new ArgumentNullException("bo");
            _efProvider.Add(AutoMapper.Mapper.Map<BO_BaseInfo>(bo));
            InsertAlias(bo);
        }

        public void UpdateBo(BoBaseInfoModel bo)
        {
            if (bo == null) throw new ArgumentNullException("bo");
            InsertAlias(bo);
            _efProvider.Save(AutoMapper.Mapper.Map<BO_BaseInfo>(bo));
        }

        public void DeleteBo(string id, string alias)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentNullException("id");
            if (!string.IsNullOrEmpty(alias))
            {
                //先删除别名表中的数据
                _aliasProvider.Delete(Guid.Parse(id));
            }
            //再删除对象基础信息表中的数据
            _efProvider.Delete(Guid.Parse(id));
        }

        public List<BoBaseInfoModel> GetBoByBot(string bot, string pid, string key, int pageIndex, int pageSize)
        {
            return _efProvider.GetByBotAndPid(bot, pid, key, pageIndex, pageSize).Select(AutoMapper.Mapper.Map<BoBaseInfoModel>).ToList();
        }

        public List<BoBaseInfoModel> GetByBot(string botName, string name, int pageIndex, int pageSize)
        {
            return _efProvider.GetByBot(botName, name, pageIndex, pageSize).Select(AutoMapper.Mapper.Map<BoBaseInfoModel>).ToList();
        }

        public List<BoBaseInfoModel> GetBoByBot(string bot, string pid)
        {
            return _efProvider.GetByBot(bot, pid).Select(AutoMapper.Mapper.Map<BoBaseInfoModel>).ToList();
        }

        public BoBaseInfoModel GetBoById(string id)
        {
            return AutoMapper.Mapper.Map<BoBaseInfoModel>(_efProvider.GetBoById(id));
        }

        public int GetCountByBotAndId(string bot, string key, string pid)
        {
            return _efProvider.GetCountByBot(bot,key, pid);
        }

        public int GetCountByBot(string botName, string key)
        {
            return _efProvider.GetCountByBot(botName, key);
        }

        private void InsertAlias(BoBaseInfoModel bo)
        {
            if (!string.IsNullOrEmpty(bo.Alias))
            {
                int i = 1;
                foreach (var alia in bo.Alias.Split(new char[] { ',', ',' }))
                {
                    var alias = new BoAliasModel();
                    alias.Alias = alia;
                    alias.ID = bo.ID;
                    alias.CreatedBy = bo.CreatedBy;
                    alias.CreatedDate = bo.CreatedDate;
                    alias.LastUpdatedBy = bo.LastUpdatedBy;
                    alias.LastUpdatedDate = bo.LastUpdatedDate;
                    alias.Remark = bo.Remark;
                    alias.SourceDB = bo.SourceDB;
                    alias.SourceID = bo.SourceID;
                    alias.SourceTable = bo.SourceTable;
                    alias.OrderIndex = i++;

                    _aliasProvider.Add(AutoMapper.Mapper.Map<BO_BOAlias>(alias));
                }
            }
        }
    }
}
