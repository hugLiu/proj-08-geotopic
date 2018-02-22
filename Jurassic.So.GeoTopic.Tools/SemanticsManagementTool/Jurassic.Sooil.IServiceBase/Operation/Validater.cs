using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Jurassic.Sooil.IServiceBase
{
    public static class Validater
    {
        public static bool IsMobile(string value)
        {
            return Regex.IsMatch(value, @"^1[3-9]\d{9}");
        }

        public static bool IsEmail(string value)
        {
            return Regex.IsMatch(value, @"(?:[a-z0-9]+[_\-+.]?)*[a-z0-9]+@(?:([a-z0-9]+-?)*[a-z0-9]+\.)+([a-z]{2,})+");
        }
    }
}
