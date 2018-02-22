using System;
using System.Collections.Generic;
using System.Linq;
using Jurassic.So.Infrastructure;
using Jurassic.PKS.Service;

namespace Jurassic.So.Search.Mongo
{
    /// <summary>
    /// 通配符扩展
    /// </summary>
    public static class WildCardExtension
    {
        /// <summary>
        /// Mongodb接口服务
        /// </summary>
        public static MongoSearch Mongo = new MongoSearch();
        /// <summary>
        /// 自定义map
        /// </summary>
        public static Dictionary<Type, object> Mappers { get; private set; }

        static WildCardExtension()
        {
            Mappers = new Dictionary<Type, object>();
            Mappers.Add(typeof(BinaryConditionExpression), new FilterSupport());
            Mappers.Add(typeof(ConditionCollectionExpression), new FilterSupport());
            AddSupport(new GroupRuleSupport());
            AddSupport(new FilterSupport());
            AddSupport(new ProjectsSupport());
            AddSupport(new SortRuleSupport());
            AddSupport(new MatchConditionSupport());
            AddSupport(new ListSupport());
            AddSupport(new DicSupport());
        }

        /// <summary>
        /// 添加一个新的通配符支持器
        /// </summary>
        /// <typeparam name="T">包含field的对象</typeparam>
        /// <param name="support">指定对象的通配支持器</param>
        private static void AddSupport<T>(IFieldSupportWildCard<T> support)
        {
            var baseType = support.GetType().GetInterface(typeof(IFieldSupportWildCard<>).Name);
            var objType = baseType?.GenericTypeArguments.First();
            if (objType != null) Mappers[objType] = support;
        }
        /// <summary>
        /// 控制字段支持通配*，
        /// 从metadatadefinition中查询获得匹配字段名称
        /// </summary>
        /// <param name="fieldName">带通配符的字段名称</param>
        /// <returns></returns>
        internal static List<string> WildCardFields(this string fieldName)
        {
            var list = new List<string>();
            if (!fieldName.IsContainRegex()) return new List<string> { fieldName };
            var result = Mongo.GetMetadataDefinition(fieldName);
            var sets = result.Select(s => s.Mapping.Set.Select(ss => ss.Key));
            foreach (var set in sets)
            {
                foreach (var key in set)
                {
                    list.Add(key.RemoveSubScript());
                }
            }
            list = list.Distinct().ToList();
            return list;
        }

        /// <summary>
        /// 转化对象中的field，使之支持通配
        /// </summary>
        /// <typeparam name="T">包含filed的对象</typeparam>
        /// <param name="obj">对象实例</param>
        /// <returns></returns>
        public static T SupportWildCard<T>(this T obj)
        {
            dynamic support = Mappers[obj.GetType()];
            dynamic obj2 = obj;
            return support.SupportWildCard(obj2);
        }
    }
}
