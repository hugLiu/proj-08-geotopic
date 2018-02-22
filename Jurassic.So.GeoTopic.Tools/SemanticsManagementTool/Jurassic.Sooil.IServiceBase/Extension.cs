using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.Sooil.IServiceBase
{
    public static class Extension
    {
        public static T ToEntity<T>(this string jsonString)
        {
            try
            {
                return (T)Newtonsoft.Json.JsonConvert.DeserializeObject(jsonString, typeof(T));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
