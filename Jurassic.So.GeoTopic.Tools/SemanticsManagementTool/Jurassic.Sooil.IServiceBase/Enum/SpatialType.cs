using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.Sooil.IServiceBase
{
    public enum SpatialType
    {
        //点
        Point = 1,
        //点集合
        MultiPoint = 2,
        //线
        LineString = 3,
        //线集合
        MultiLineString = 4,
        //多边形
        Polygon = 5,
        //多边形集合
        MultiPolygon = 6,
        //空间对象集合
        GeometryCollection = 7 
    }
}
