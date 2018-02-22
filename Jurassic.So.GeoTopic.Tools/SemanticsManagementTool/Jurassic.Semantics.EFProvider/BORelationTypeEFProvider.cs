using System;
using System.Collections.Generic;
using Jurassic.Semantics.EntityNew;

namespace Jurassic.Semantics.EFProvider
{
    /// <summary>
    /// 对象关系类型维护
    /// </summary>
    //public class RelationTypeEfProvider
    //{
    //    private readonly BODbContext _dbContext;

    //    public RelationTypeEfProvider()
    //    {
    //        _dbContext = new BODbContext();
    //    }

    //    /// <summary>
    //    /// 获得业务对象关系类型的所有信息
    //    /// </summary>
    //    /// <returns></returns>
    //    public List<CD_RelType> GetAllRelType()
    //    {
    //        return _dbContext.CD_RelType
    //              .OrderBy(o => o.RelTypeName)
    //              .ToList();
    //    }

    //    /// <summary>
    //    /// 添加新的业务对象关系类型
    //    /// </summary>
    //    /// <param name="type">新添加的业务对象关系类型</param>
    //    public void Add(CD_RelType type)
    //    {
    //        if (!_dbContext.CD_RelType.Any(a => a.RelTypeCode.Equals(type.RelTypeCode)))
    //        {
    //            _dbContext.CD_RelType.Add(type);
    //            _dbContext.SaveChanges();
    //        }
    //        else
    //        {
    //            throw new Exception("插入的数据已存在!");
    //        }
    //    }

    //    /// <summary>
    //    /// 删除已存在业务对象关系类型
    //    /// </summary>
    //    /// <param name="tyrpCode">待删除类型编码</param>
    //    public void Delete(string tyrpCode)
    //    {
    //        if (_dbContext.CD_RelType.Any(a => a.RelTypeCode.Equals(tyrpCode)))
    //        {
    //            var oldType = _dbContext.CD_RelType.SingleOrDefault(a => a.RelTypeCode.Equals(tyrpCode));
    //            _dbContext.CD_RelType.Remove(oldType);
    //            _dbContext.SaveChanges();
    //        }
    //        else
    //        {
    //            throw new Exception("删除的数据不存在！");
    //        }
    //    }

    //    /// <summary>
    //    /// 更新业务对象关系类型
    //    /// </summary>
    //    /// <param name="type">待更新的业务对象关系类型</param>
    //    public void Update(CD_RelType type)
    //    {
    //        if (_dbContext.CD_RelType.Any(a => a.RelTypeCode.Equals(type.RelTypeCode)))
    //        {
    //            var oldType = _dbContext.CD_RelType.SingleOrDefault(a => a.RelTypeCode.Equals(type.RelTypeCode));
    //            if (oldType != null)
    //            {
    //                oldType.RelTypeName = type.RelTypeName;
    //                oldType.Description = type.Description;
    //                oldType.Remark = type.Remark;

    //                _dbContext.SaveChanges();
    //            }
    //        }
    //        else
    //        {
    //            throw new Exception("更新的数据不存在!");
    //        }
    //    }
    //}
}
