using System;
using System.Collections.Generic;
using System.Linq;
using Jurassic.So.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Jurassic.PKS.Service;

namespace Jurassic.So.Search.Mongo
{
    /// <summary>
    /// Mongo查询条件构建扩展
    /// </summary>
    public static class MongoExpBuilderExtension
    {
        public static Dictionary<Type, object> Mappers { get; private set; }
        static MongoExpBuilderExtension()
        {
            Mappers = new Dictionary<Type, object>();
            AddBuilder(new MongoCombineExpBuilder());
            AddBuilder(new MongoBinaryExpBuilder());
        }

        private static void AddBuilder<TExp>(IMongoExpBuilder<TExp> builder)
            where TExp : IConditionalExpression
        {
            var baseType = builder.GetType().GetInterface(typeof(IMongoExpBuilder<>).Name);
            var expType = baseType?.GenericTypeArguments.First();
            if (expType != null) Mappers[expType] = builder;
        }

        public static TBuilder Get<TBuilder>(this object exp)
            where TBuilder : class
        {
            return Mappers[exp.GetType()].As<TBuilder>();
        }
        /// <summary>
        /// 根据指定的JSON获得对应条件表达式
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static IConditionalExpression FromMongoJson(this object json)
        {
            if (json == null) return null;
            var root = JsonConvert.DeserializeObject<JObject>(json.ToString());
            return ((IConditionalExpression)null).AnalyzeJObject(root);
        }

        /// <summary>
        /// 根据给定的条件表达式获得对应字符串
        /// </summary>
        /// <param name="exp">条件表达式</param>
        /// <returns></returns>
        public static string ToMongoJson(this IConditionalExpression exp)
        {
            if (exp != null)
            {
                dynamic builder = Mappers[exp.GetType()];
                dynamic exp2 = exp;
                return builder.GetJson(exp2);
            }
            return string.Empty;
        }
    }
}
