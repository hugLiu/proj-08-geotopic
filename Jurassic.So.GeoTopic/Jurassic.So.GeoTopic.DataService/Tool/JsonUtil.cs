using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Jurassic.So.GeoTopic.DataService.Tool
{
    public class JsonUtil
    {
        //从一个对象信息生成Json串        
        public static string ObjectToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        public static object JsonToObject(string jsonString, Type type)
        {
            //项目中统一时间格式为d/m/Y
            //IsoDateTimeConverter timeFormat1 = new IsoDateTimeConverter();
            //timeFormat1.DateTimeFormat = "yyyy/MM/dd";
            //return Newtonsoft.Json.JsonConvert.DeserializeObject(jsonString, type, timeFormat1);
            return JsonConvert.DeserializeObject(jsonString, type);
        }
    }
}
