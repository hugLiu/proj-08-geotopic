using System;
using System.Collections.Generic;
using System.Linq;
using Jurassic.Semantics.EntityNew;

namespace Jurassic.Semantics.EFProvider
{
    /// <summary>
    /// 业务对象别名维护
    /// </summary>
    public class BoAliasEfProvider
    {
        private readonly BODbContext _dbContext;

        public BoAliasEfProvider()
        {
            _dbContext = new BODbContext();
        }

        /// <summary>
        /// 根据指定的业务对象id获得其别名信息
        /// </summary>
        /// <param name="id">业务对象id</param>
        /// <returns>返回别名信息列表</returns>
        public List<BO_BOAlias> GetById(Guid id)
        {
            return
                _dbContext.BO_BOAlias.Where(w => w.ID.Equals(id))
                    .OrderBy(o => o.OrderIndex)
                    .ThenBy(t => t.CreatedDate)
                    .ToList();
        }

        /// <summary>
        /// 添加业务对象别名
        /// </summary>
        /// <param name="alias">别名对象</param>
        public void Add(BO_BOAlias alias)
        {
            if (!_dbContext.BO_BOAlias.Any(a => a.Alias.Equals(alias.Alias) && a.ID.Equals(alias.ID)))
            {
                _dbContext.BO_BOAlias.Add(alias);
                _dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// 删除已存在业务对象别名信息
        /// </summary>
        /// <param name="id">待删除对象的ID</param>
        /// <param name="alia">待删除别名</param>
        public void Delete(Guid id, string alia)
        {
            if (_dbContext.BO_BOAlias.Any(a => a.ID.Equals(id) && a.Alias.Equals(alia)))
            {
                var oldAlias = _dbContext.BO_BOAlias.SingleOrDefault(a => a.ID.Equals(id) && a.Alias.Equals(alia));
                _dbContext.BO_BOAlias.Remove(oldAlias);
                _dbContext.SaveChanges();
            }
            else
            {
                throw new Exception("删除的数据不存在！");
            }

        }
        /// <summary>
        /// 删除已存在业务对象别名信息
        /// </summary>
        /// <param name="id">待删除对象的ID</param>
        /// <param name="alia">待删除别名</param>
        public void Delete(Guid id)
        {
            if (_dbContext.BO_BOAlias.Any(a => a.ID.Equals(id)))
            {
                var oldAlias = _dbContext.BO_BOAlias.Where(a => a.ID.Equals(id));
                _dbContext.BO_BOAlias.RemoveRange(oldAlias);
                _dbContext.SaveChanges();
            }
            else
            {
                throw new Exception("删除的数据不存在！");
            }

        }

        /// <summary>
        /// 更新业务对象别名信息
        /// </summary>
        /// <param name="alias">别名</param>
        public void Update(BO_BOAlias alias)
        {
            if (_dbContext.BO_BOAlias.Any(a => a.ID.Equals(alias.ID) && a.Alias.Equals(alias.Alias)))
            {
                var oldAlias = _dbContext.BO_BOAlias.SingleOrDefault(a => a.ID.Equals(alias.ID) && a.Alias.Equals(alias.Alias));
                if (oldAlias != null)
                {
                    oldAlias.OrderIndex = alias.OrderIndex;
                    oldAlias.Remark = alias.Remark;
                    oldAlias.SourceDB = alias.SourceDB;
                    oldAlias.SourceID = alias.SourceID;
                    oldAlias.SourceTable = alias.SourceTable;
                    oldAlias.LastUpdatedBy = alias.LastUpdatedBy;
                    oldAlias.LastUpdatedDate = DateTime.Now;
                    _dbContext.SaveChanges();
                }
            }
            else
            {
                throw new Exception("更新的数据不存在!");
            }
        }
    }
}
