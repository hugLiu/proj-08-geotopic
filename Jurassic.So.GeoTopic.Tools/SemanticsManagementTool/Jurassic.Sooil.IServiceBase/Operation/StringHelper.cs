using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Jurassic.Sooil.IServiceBase
{
    public class StringHelper
    {
        /// <summary>
        /// 转义字符串中所有正则特殊字符
        /// </summary>
        /// <param name="input">传入字符串</param>
        /// <returns></returns>
        public static string FilterString(string input)
        {
            
            input = input.Replace("\\", "\\\\");//先替换“\”，不然后面会因为替换出现其他的“\”
            input = input.Replace("\"", "'");
            var r = new Regex("[\\:\\!\\-\\~\\*\\.\\?\\+\\$\\^\\[\\]\\(\\)\\{\\}\\|\\/]");
            var ms = r.Matches(input);
            var list = new List<string>();
            foreach (Match item in ms)
            {
                if (list.Contains(item.Value))
                    continue;
                input = input.Replace(item.Value, "\\" + item.Value);
                list.Add(item.Value);
            }
            return input;
        }
    }
}
