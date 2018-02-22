using Jurassic.Semantics.EntityNew;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jurassic.Semantics.EFProvider
{
    /// <summary>
    /// 业务对象基础信息维护
    /// </summary>
    public class BoBaseInfoEfProvider
    {
        private readonly BODbContext _dbContext;

        public BoBaseInfoEfProvider()
        {
            _dbContext = new BODbContext();
        }

        /// <summary>
        /// 根据指定的BOT获得对应的业务对象的基础信息
        /// </summary>
        /// <param name="botName">具体的业务对象类型</param>
        /// <param name="pid">父id</param>
        /// <param name="name"></param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">一页最多纪录</param>
        /// <returns>返回BO基础信息列表</returns>
        public List<BO_BaseInfo> GetByBotAndPid(string botName, string pid, string name, int pageIndex, int pageSize)
        {
            var skip = pageIndex * pageSize;
            var take = pageSize;
            var list = new List<BO_BaseInfo>();
            if (string.IsNullOrEmpty(pid) || pid == "null")
            {
                list =
                    _dbContext.BO_BaseInfo.Include("BO_BOAlias").Where(w => w.BOT.Equals(botName) && !w.PID.HasValue && (w.Name.Contains(name) || name == ""))
                        .OrderBy(o => o.OrderIndex)
                        .ThenBy(t => t.Name)
                        .Skip(skip)
                        .Take(take)
                        .ToList();
            }
            else
            {
                var pId = Guid.Parse(pid);
                //先获得所有存在pid的数据，因为当guid？值为空的时候，获得其value值会抛出异常
                var query = _dbContext.BO_BaseInfo.Include("BO_BOAlias").Where(w => w.BOT.Equals(botName) && w.PID.HasValue);
                list = query.Where(w => w.PID.Value.Equals(pId) && (w.Name.Contains(name) || name == ""))
                        .OrderBy(o => o.OrderIndex)
                        .ThenBy(t => t.CreatedDate)
                        .Skip(skip)
                        .Take(take)
                        .ToList();
            }
            return list;
        }

        /// <summary>
        /// 根据bot获得全部的业务对象
        /// </summary>
        /// <param name="botName">业务对象类型</param>
        /// <param name="name">对业务对象的名字进行过滤</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">一页总记录条数</param>
        /// <returns></returns>
        public List<BO_BaseInfo> GetByBot(string botName, string name, int pageIndex, int pageSize)
        {
            if (name == null) name = "";
            var skip = pageIndex * pageSize;
            var take = pageSize;
            var list =
                _dbContext.BO_BaseInfo.Include("BO_BOAlias").Where(w => w.BOT.Equals(botName) && (w.Name.Contains(name) || name == ""))
                    .OrderBy(o => o.OrderIndex)
                    .ThenBy(t => t.Name)
                    .Skip(skip)
                    .Take(take)
                    .ToList();
            return list;

        }

        public int GetCountByBot(string botName, string name, string pid)
        {
            var listCount = 0;
            if (string.IsNullOrEmpty(pid) || pid == "null")
            {
                listCount =
                    _dbContext.BO_BaseInfo.Include("BO_BOAlias").Where(w => w.BOT.Equals(botName) && !w.PID.HasValue && (w.Name.Contains(name) || name == ""))
                        .OrderBy(o => o.OrderIndex)
                        .ThenBy(t => t.CreatedDate)
                        .Count();
            }
            else
            {
                var pId = Guid.Parse(pid);
                //先获得所有存在pid的数据，因为当guid？值为空的时候，获得其value值会抛出异常
                var query = _dbContext.BO_BaseInfo.Include("BO_BOAlias").Where(w => w.BOT.Equals(botName) && w.PID.HasValue);
                listCount = query.Where(w => w.PID.Value.Equals(pId) && (w.Name.Contains(name) || name == ""))
                        .OrderBy(o => o.OrderIndex)
                        .ThenBy(t => t.CreatedDate)
                        .Count();
            }
            return listCount;
        }
        public int GetCountByBot(string botName, string name)
        {
            return _dbContext.BO_BaseInfo.Include("BO_BOAlias").Count(w => w.BOT.Equals(botName) && (w.Name.Contains(name) || name == ""));
        }
        /// <summary>
        ///获得指定BOT和pid的业务对象
        /// </summary>
        /// <param name="botName"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        public List<BO_BaseInfo> GetByBot(string botName, string pid)
        {
            var pId = Guid.Parse(pid);
            var list = new List<BO_BaseInfo>();

            //先获得所有存在pid的数据，因为当guid？值为空的时候，获得其value值会抛出异常
            var query = _dbContext.BO_BaseInfo.Include("BO_BOAlias").Where(w => w.BOT.Equals(botName) && w.PID.HasValue);
            list = query.Where(w => w.PID.Value.Equals(pId))
                    .OrderBy(o => o.OrderIndex)
                    .ThenBy(t => t.CreatedDate)
                    .ToList();

            return list;
        }

        /// <summary>
        /// 根据id获得业务对象的信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BO_BaseInfo GetBoById(string id)
        {
            if (id == null) throw new ArgumentNullException("id");

            var _id = Guid.Parse(id);

            return _dbContext.BO_BaseInfo.Include("BO_BOAlias").FirstOrDefault(w => w.ID.Equals(_id));
        }

        /// <summary>
        /// 添加新的业务对象
        /// </summary>
        /// <param name="bo">新添加的业务对象信息</param>
        public void Add(BO_BaseInfo bo)
        {
            if (!_dbContext.BO_BaseInfo.Any(a => a.BOT.Equals(bo.BOT) && a.Name.Equals(bo.Name)))
            {
                _dbContext.BO_BaseInfo.Add(bo);
                _dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// 删除已存在业务对象基础信息
        /// </summary>
        /// <param name="id">待删除对象的ID</param>
        public void Delete(Guid id)
        {
            if (_dbContext.BO_BaseInfo.Any(a => a.ID.Equals(id)))
            {
                var oldBo = _dbContext.BO_BaseInfo.SingleOrDefault(a => a.ID.Equals(id));
                if (oldBo != null && (oldBo.BO_Relation.Count > 0 || oldBo.BO_Relation1.Count > 0))
                {
                    throw new Exception("删除的数据存在关联关系，不可删除！");
                }
                _dbContext.BO_BaseInfo.Remove(oldBo);
                _dbContext.SaveChanges();
            }
            else
            {
                throw new Exception("删除的数据不存在！");
            }

        }

        /// <summary>
        /// 更新业务对象信息
        /// </summary>
        /// <param name="bo">待更新的业务对象</param>
        public void Save(BO_BaseInfo bo)
        {
            if (_dbContext.BO_BaseInfo.Any(a => a.BOT.Equals(bo.BOT) && a.Name.Equals(bo.Name)))
            {
                var oldBo = _dbContext.BO_BaseInfo.SingleOrDefault(a => a.ID.Equals(bo.ID));
                if (oldBo != null)
                {
                    oldBo.Name = bo.Name;
                    oldBo.SID = bo.SID;
                    oldBo.OrderIndex = bo.OrderIndex;
                    oldBo.SpaceLocation = bo.SpaceLocation;
                    oldBo.Remark = bo.Remark;
                    oldBo.SourceDB = bo.SourceDB;
                    oldBo.SourceID = bo.SourceID;
                    oldBo.SourceTable = bo.SourceTable;
                    oldBo.LastUpdatedBy = bo.LastUpdatedBy;
                    oldBo.LastUpdatedDate = DateTime.Now;
                    _dbContext.SaveChanges();
                }

            }
            else
            {
                Add(bo);
            }
        }
    }
}
