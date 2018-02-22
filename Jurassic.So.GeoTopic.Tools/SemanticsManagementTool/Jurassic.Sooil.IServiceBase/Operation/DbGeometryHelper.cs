using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.Sooil.IServiceBase
{
    public class DbGeometryHelper
    {
        /// <summary>
        /// 将一个范围转换成一个DbGeometry的面，要是只有一个点就转换成点。
        /// </summary>
        /// <param name="xyList">，号分割的xy坐标: 3.123312,6.7678</param>
        /// <returns></returns>
        public DbGeometry PointToDbGeometry(List<string> xyList)
        {
            DbGeometry geometry;

            if (xyList.Count == 1)
            {
                geometry = DbGeometry.FromText("POINT (" + xyList.FirstOrDefault().Split(',')[0] + " " + xyList.FirstOrDefault().Split(',')[1] + ")", 4326);
            }
            else
            {
                StringBuilder geostr = new StringBuilder();
                foreach (var st in xyList)
                {
                    geostr.Append(st.Split(',')[0] + " " + st.Split(',')[1] + ",");
                }
                geometry = DbGeometry.FromText("POLYGON ((" + geostr.Remove(geostr.Length - 1, 1) + "))", 4326);
            }
            return geometry;
        }
    }
}
