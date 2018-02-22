using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Jurassic.Sooil.IServiceBase
{
    public class MetaDataFactory
    {
        private static List<Type> metaDataTypes = new List<Type>();
        private static IDictionary<string, string> mappedClassName = new Dictionary<string, string>();
        static MetaDataFactory()
        {
            mappedClassName.Add("Title_", "Title");
            mappedClassName.Add("Title_Formal", "DisplayTitle");
            mappedClassName.Add("Title_Alternative", "Title");

            mappedClassName.Add("DC_", "DC");
            mappedClassName.Add("DC_Sponsor", "DC");
            mappedClassName.Add("DC_Creator", "Creator");
            mappedClassName.Add("DC_Auditor", "DC");
            mappedClassName.Add("DC_Publisher", "DC");
            mappedClassName.Add("DC_Organization", "DC");

            mappedClassName.Add("Date_", "SMDDate");
            mappedClassName.Add("Date_Created", "CreatedDate");
            mappedClassName.Add("Date_Valid", "SMDDate");
            mappedClassName.Add("Date_Available", "SMDDate");
            mappedClassName.Add("Date_Issued", "SMDDate");
            mappedClassName.Add("Date_Modified", "SMDDate");
            mappedClassName.Add("Date_Accepted", "SMDDate");
            mappedClassName.Add("Date_Copyrighted", "SMDDate");
            mappedClassName.Add("Date_Submitted", "SMDDate");
            //这里是遍历指定程序集的所有类类型，并保留符合要求的类型
            foreach (Type type in Assembly.Load("Jurassic.Sooil.SemanticsService.IO").GetTypes()
                                  .Where(type => type.IsSubclassOf(typeof(MetaData))))
            {
                metaDataTypes.Add(type);
            }
        }
        public static MetaData CreateMetaData(string tag,string type,string value)
        {
            Type metaDataType = metaDataTypes.FirstOrDefault(md => MappingType(tag,type, md.Name) == true);
            if (metaDataType == null)
            {
                return null;
            }
            MetaData metaData = (MetaData)Activator.CreateInstance(metaDataType);
            metaData.Type = type;
            metaData.Value = value;
            return metaData;
        }
        public static MetaData CreateMetaData(string tag, string type, string value,double score)
        {
            Type metaDataType = metaDataTypes.FirstOrDefault(md => MappingType(tag, type, md.Name) == true);
            if (metaDataType == null)
            {
                return null;
            }
            MetaData metaData = (MetaData)Activator.CreateInstance(metaDataType);
            metaData.Type = type;
            metaData.Value = value;
            metaData.Score = score;
            return metaData;
        }
        private static bool MappingType(string tag,string type,string typeName)
        {
            string key = string.Format("{0}_{1}", tag, type);
            string className;
            if(!mappedClassName.TryGetValue(key,out className))
            {
                className = tag;
            }
            return string.Compare(className, typeName, true) == 0;
        }
        public static T CreateMetaData<T>(string tag, string type, string value) where T : class, new()
        {
            return CreateMetaData(tag,type,value) as T;
        }
    }
}
