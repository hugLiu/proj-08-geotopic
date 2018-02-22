using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.Sooil.IServiceBase
{
    public class PList<T>
    {
        public PList(IList<T> items,int totalItemCount)
        {
            TotalItemCount = totalItemCount;
            Data = items;
        }

        public IList<T> Data { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalItemCount { get; set; }
    }
}
