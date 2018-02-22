
using System;
using System.Collections.Generic;
using Jurassic.Semantics.EntityNew;

namespace Jurassic.Semantics.EFProvider
{
    /// <summary>
    /// CD_TypeCode operation
    /// </summary>
    public class BOTEFProvider
    {
        //private readonly BODbContext _dbContext;
        //public BOTEFProvider()
        //{
        //    _dbContext = new BODbContext();
        //}
        ///// <summary>
        ///// get bot by id
        ///// </summary>
        ///// <param name="id">id</param>
        ///// <returns>bots</returns>
        //public List<CD_TypeCode> GetBotbyId(string id)
        //{
        //    //如果id为null返回全部
        //    if (string.IsNullOrWhiteSpace(id))
        //        return _dbContext.CD_TypeCode.ToList();

        //    var _id = Guid.Parse(id);

        //    return _dbContext.CD_TypeCode.Where(w => w.TypeID.Equals(_id))
        //        .OrderBy(o => o.OrderIndex).ThenBy(t => t.CreatedDate).ToList();
        //}

        ///// <summary>
        ///// insert bot int CD_TypeCode
        ///// </summary>
        ///// <param name="bot">bot name</param>
        ///// <param name="userName">Current login user name</param>
        ///// <param name="typeName">type name</param>
        ///// <param name="orderIndex">order index</param>
        //public void InsertBot(string bot, string userName, string typeName, int orderIndex, string description, string remark)
        //{
        //    if (!_dbContext.CD_TypeCode.Any(a => a.BOT.Equals(bot) && a.TypeName.Equals(typeName)))
        //    {
        //        var id = Guid.NewGuid();
        //        var type = new CD_TypeCode()
        //        {
        //            TypeID = id,
        //            BOT = bot,
        //            TypeName = typeName,
        //            OrderIndex = orderIndex,
        //            Description = description,
        //            Remark = remark,
        //            CreatedBy = userName,
        //            CreatedDate = DateTime.Now
        //        };

        //        _dbContext.CD_TypeCode.Add(type);

        //        _dbContext.SaveChanges();
        //    }
        //    else
        //    {
        //        throw new Exception("该类型已存在，请勿重复插入");
        //    }
        //}

        ///// <summary>
        ///// update bot
        ///// </summary>
        ///// <param name="id">edit bot id</param>
        ///// <param name="userName">Current login user name</param>
        ///// <param name="bot">edited bot name</param>
        ///// <param name="typeName">edited type name</param>
        ///// <param name="orderIndex">edited order index</param>
        //public void UpdateBot(string id, string userName, string bot, string typeName, int orderIndex, string description, string remark)
        //{
        //    var type = new CD_TypeCode() { BOT = bot, TypeName = typeName, OrderIndex = orderIndex };
        //    var _id = Guid.Parse(id);
        //    if (_dbContext.CD_TypeCode.Any(w => w.TypeID.Equals(_id))) 
        //    {
        //        //get the origin data
        //        var oldBot = _dbContext.CD_TypeCode.SingleOrDefault(w => w.TypeID.Equals(_id));
        //        //deassign changed properties
        //        if (oldBot != null)
        //        {
        //            oldBot.BOT = bot;
        //            oldBot.TypeName = typeName;
        //            oldBot.OrderIndex = orderIndex;
        //            oldBot.Description = description;
        //            oldBot.Remark = remark;
        //            oldBot.LastUpdatedBy = userName;
        //            oldBot.LastUpdatedDate = DateTime.Now;
        //        }
        //        _dbContext.SaveChanges();
        //    }
        //    else
        //    {
        //       throw new Exception("该业务对象类型不存在！");
        //    }

        //}

        ///// <summary>
        ///// delete bot by id
        ///// </summary>
        ///// <param name="id">selected id</param>
        //public void DeleteBot(string id)
        //{
        //    //如果删除的话，需要先删除使用该type的业务对象
        //    //是删除还是其他操作

        //    if (id == null) throw new ArgumentNullException("id");
        //    var _id = Guid.Parse(id);

        //    if (_dbContext.CD_TypeCode.Include("BO_BaseInfo")
        //        .Any(w => w.TypeID.Equals(_id) && !w.BO_BaseInfo.Any()))
        //    {
        //        var deleteBot = _dbContext.CD_TypeCode.Include("BO_BaseInfo")
        //          .SingleOrDefault(w => w.TypeID.Equals(_id) && !w.BO_BaseInfo.Any());
        //        _dbContext.CD_TypeCode.Remove(deleteBot);
        //        _dbContext.SaveChanges();
        //    }
        //    else
        //    {
        //        throw new Exception("该类型已使用，不能删除");
        //    }
        //}
    }
}
