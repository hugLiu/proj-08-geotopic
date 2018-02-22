using System.Collections.Generic;
using System.Linq;
using Jurassic.Semantics.EntityNew;

namespace Jurassic.Semantics.EFProvider
{
    /// <summary>
    /// 业务对象关系维护
    /// </summary>
    public class BoRelationEfProvider
    {
        private readonly BODbContext _dbContext;

        public BoRelationEfProvider()
        {
            _dbContext = new BODbContext();
        }

        /// <summary>
        /// 添加新的业务对象关系
        /// </summary>
        /// <param name="boRelation"></param>
        public void Add(BO_Relation boRelation)
        {
            if (!_dbContext.BO_Relation.Any(a => a.ID1.Equals(boRelation.ID1) && a.ID2.Equals(boRelation.ID2) && a.RelTypeCode.Equals(boRelation.RelTypeCode)))
            {
                _dbContext.BO_Relation.Add(boRelation);
                _dbContext.SaveChanges();
            }
        }
        /// <summary>
        /// 更新业务对象关系
        /// </summary>
        /// <param name="boRelation"></param>
        public void Update(BO_Relation boRelation)
        {
            if (_dbContext.BO_Relation.Any(a => a.ID1.Equals(boRelation.ID1) && a.ID2.Equals(boRelation.ID2) && !a.RelTypeCode.Equals(boRelation.RelTypeCode)))
            {
                var oldBo = _dbContext.BO_Relation.SingleOrDefault(a => a.ID1.Equals(boRelation.ID1) && a.ID2.Equals(boRelation.ID2));
                if (oldBo != null)
                {
                    oldBo.BOT1 = boRelation.BOT1;
                    oldBo.BOT2 = boRelation.BOT2;
                    oldBo.ID1 = boRelation.ID1;
                    oldBo.ID2 = boRelation.ID2;
                    oldBo.RelTypeCode = boRelation.RelTypeCode;
                    _dbContext.SaveChanges();
                }
            }
        }

        /// <summary>
        /// 删除已存在业务对象关系
        /// </summary>
        /// <param name="boRelation"></param>
        public void Delete(BO_Relation boRelation)
        {
            if (_dbContext.BO_Relation.Any(a => a.ID1.Equals(boRelation.ID1) && a.ID2.Equals(boRelation.ID2) && a.RelTypeCode.Equals(boRelation.RelTypeCode)))
            {
                var oldBoRelation = _dbContext.BO_Relation.SingleOrDefault(a => a.ID1.Equals(boRelation.ID1) && a.ID2.Equals(boRelation.ID2) && a.RelTypeCode.Equals(boRelation.RelTypeCode));
                _dbContext.BO_Relation.Remove(oldBoRelation);
                _dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// 根据业务对象关系的前面BOT获得对应关系的BOT
        /// </summary>
        /// <param name="bot1">对象关系的前面bot</param>
        /// <returns>返回指定bot1的对应bot2</returns>
        public List<string> GetBot2(string bot1)
        {
            return
                _dbContext.BO_Relation
                .Where(w => w.BOT1.Equals(bot1))
                .OrderBy(o => o.BOT2)
                .Select(s => s.BOT2)
                .Distinct().ToList();
        }


        // <summary>
        // 获得所有的业务对象关系类型
        // </summary>
        // <returns></returns>
        //public List<CD_RelType> GetBoRelationType()
        //{
        //    return _dbContext.CD_RelType.ToList();
        //}

        // <summary>
         
        // </summary>
        // <returns></returns>
        //public List<BO_Relation> GetId2ById1AndBot2(string id, string bot2)
        //{
        //    var id1 = Guid.Parse(id);
        //    return _dbContext.BO_Relation.Include("BO_BaseInfo").Include("BO_BaseInfo1").Include("CD_RelType").Where(w => w.ID1.Equals(id1) && w.BOT2.Equals(bot2)).ToList();
        //}
    }
}
