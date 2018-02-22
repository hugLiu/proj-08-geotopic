using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.Sooil.IServiceBase
{
    public static class CodeHelper
    {
        public static string GenerateCode(int num = 6, CodeType codeType = CodeType.All)
        {
            string[] content = null;
            switch(codeType){
                case CodeType.All:
                content = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "J", "K", "L", "M", "N", "P", "Q", "R", "S", "D", "U", "V", "W", "X", "Y", "Z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
                    break;
                case CodeType.Letter:
                content = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "J", "K", "L", "M", "N", "P", "Q", "R", "S", "D", "U", "V", "W", "X", "Y", "Z" };
                    break;
                case CodeType.Number:
                content = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
                    break;
            }

            StringBuilder sb = new StringBuilder();
            var random = new Random();
            for (int i = 0; i < num; i++)
            {
                int r = random.Next(0, content.Length - 1);
                sb.Append(content[r]);
            }
            return sb.ToString();
        }
        public static string GenerateCode(bool type = true, int forMaxId = 6)
        {
            string[] content = null;
            if (type)
            {
                content = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            }
            else
            {
                content = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "D", "U", "V", "W", "X", "Y", "Z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            }

            StringBuilder sb = new StringBuilder();
            int num = 0;
            var random = new Random();
            for (int i = 0; i < forMaxId; i++)
            {
                num = random.Next(0, content.Length - 1);
                sb.Append(content[num]);
            }
            return sb.ToString();
        }
    }

    public enum CodeType
    {
        Number,
        Letter,
        All
    }
}
