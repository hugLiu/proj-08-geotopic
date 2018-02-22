using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.Sooil.IServiceBase
{
    public class UnitTransform
    {
        /// <summary>
        /// 米转换为度
        /// </summary>
        /// <param name="meter">米</param>
        /// <returns></returns>
        public static double MiToDu(double meter)
        {
            var degree = meter / 1000;
            return (180 * degree) / (3.141593 * 6371);
        }
        /// <summary>
        /// 度转换成米
        /// </summary>
        /// <param name="du"></param>
        /// <returns></returns>
        public static double DuToMi(double du)
        {
            return du*(3.141593/180)*6371*1000;
        }
    }
}
