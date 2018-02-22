using System;
using Jurassic.So.Infrastructure;
using Jurassic.PKS.Service;

namespace Jurassic.So.Search.Mongo
{
    /// <summary>
    /// 通用表达式运算符和Mongo运算符转化
    /// </summary>
    public static class OpretorConvert
    {
        #region 转化Mongo操作符

        /// <summary>
        /// 根据操作类型转化为Mongo中的查询操作符
        /// </summary>
        /// <param name="opretor">定义的通用操作类型（枚举）</param>
        /// <returns></returns>
        public static string GetMongoName(this BinaryOperator opretor)
        {
            var mongoOp = (MongoOpretor)Enum.Parse(typeof(MongoOpretor), opretor.ToString());
            return mongoOp.GetRemark();
        }
        /// <summary>
        /// 根据给定的mongo操作符转化为通用操作符枚举类
        /// </summary>
        /// <param name="mongoOpName"></param>
        /// <returns></returns>
        public static BinaryOperator ToOp(this string mongoOpName)
        {
            var mongoOp = mongoOpName.ByRemark<MongoOpretor>();
            return (BinaryOperator)Enum.Parse(typeof(BinaryOperator), mongoOp);
        }

        /// <summary>
        /// 根据逻辑操作类型转化为Mongo中的逻辑查询操作符
        /// </summary>
        /// <param name="opretor">逻辑操作符（枚举）</param>
        /// <returns></returns>
        public static string GetMongoName(this LogicOperator opretor)
        {
            var mongoOp = (MongoOpretor)Enum.Parse(typeof(MongoOpretor), opretor.ToString());
            return mongoOp.GetRemark();
        }

        /// <summary>
        /// 根据给定的mongo逻辑操作符转化为通用逻辑操作符枚举类
        /// </summary>
        /// <param name="mongoLogicOpName"></param>
        /// <returns></returns>
        public static LogicOperator ToLogicOp(this string mongoLogicOpName)
        {
            var mongoOp = mongoLogicOpName.ByRemark<MongoOpretor>();
            return (LogicOperator)Enum.Parse(typeof(LogicOperator), mongoOp);
        }
        #endregion
    }
}
