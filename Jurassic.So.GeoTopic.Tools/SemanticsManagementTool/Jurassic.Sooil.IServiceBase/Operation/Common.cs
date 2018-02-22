using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.Sooil.IServiceBase
{
    public static class Common
    {
        public static string MinuteToString(int minute)
        {
            if (minute <= 60)
                return minute + "分钟";
            if (minute > 60 && minute <= 1440)
                return Convert.ToInt32(minute/60) + "小时";
            if (minute > 1440)
                return Convert.ToInt32(minute/1440) + "天";
            return null;
        }
    }
}
