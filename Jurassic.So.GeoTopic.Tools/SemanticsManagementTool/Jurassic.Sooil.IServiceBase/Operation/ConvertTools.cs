using System;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Jurassic.Sooil.IServiceBase
{
    public static class ConvertTools
    {
        /// <summary>
        /// Base64转DataSet
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DataSet Base64ToDataSet(string str)
        {
            byte[] bytes = Convert.FromBase64String(str);
            MemoryStream m = new MemoryStream(bytes);
            BinaryFormatter formatter = new BinaryFormatter();
            m.Position = 0;
            return (DataSet)formatter.Deserialize(m);
        }
        /// <summary>
        /// Base64转Bytes
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] Base64ToBytes(string str)
        {
            byte[] bytes = Convert.FromBase64String(str);
            return bytes;
        }
        /// <summary>
        /// 将字符串转化成GUID
        /// </summary>
        /// <param name="id">字符串</param>
        /// <returns>返回GUID</returns>
        public static Guid StringToGuid(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentNullException();
            Guid guid;
            var isValid = Guid.TryParse(id, out guid);
            if (!isValid) throw new FormatException();
            return guid;
        }
    }
}
