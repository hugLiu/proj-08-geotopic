using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.Sooil.IServiceBase
{
    public class CoordConvertor
    {
        private const double PI = 3.1415926535898;

        private static double _a = 6378245.0;
        private static double _alfa = 298.3;
        private static int _zoneWide = 3;//带宽

        public CoordConvertor() { }
        /// <summary>
        /// 椭球体初始化
        /// </summary>
        /// <param name="a">长半径</param>
        /// <param name="b">短半径</param>
        public CoordConvertor(double a, double b)
        {
            _a = a;
            _alfa = (a - b) / a;
        }

        /// <summary>
        /// 设置椭球体
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static void SetEllip(double a, double b)
        {
            _a = a;
            _alfa = (a - b) / a;
        }

        /// <summary>
        /// 设置带宽
        /// </summary>
        /// <param name="zonewide"></param>
        public static void SetZoneWide(int zonewide)
        {
            _zoneWide = zonewide;
        }

        /// <summary>
        /// 度分秒转化为度
        /// 41°10'6.67″→41.100667
        /// </summary>
        /// <param name="Dms"></param>
        /// <returns></returns>
        public static double Dms2D(double Dms)
        {
            double Degree, Miniute;
            double Second;
            int Sign;
            double D;
            if (Dms >= 0)
                Sign = 1;
            else
                Sign = -1;
            Dms = Math.Abs(Dms);
            Degree = Math.Floor(Dms);
            Miniute = Math.Floor(Dms * 100.0 % 100.0);
            Second = Dms * 10000.0 % 100.0;
            D = Sign * (Degree + Miniute / 60.0 + Second / 3600.0);
            return D;
        }

        /// <summary>
        /// 度分秒转化为度
        /// </summary>
        /// <param name="d">度</param>
        /// <param name="m">分</param>
        /// <param name="s">秒</param>
        /// <returns></returns>
        public static double Dms2D(double d, double m, double s)
        {
            int Sign;
            if (d >= 0)
                Sign = 1;
            else
                Sign = -1;
            double dd = Sign * (d + m / 60.0 + s / 3600.0);
            return dd;
        }

        /// <summary>
        /// 经纬度转化为坐标点
        /// </summary>
        /// <param name="longitude">经度</param>
        /// <param name="latitude">纬度</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void LLtoXY(double longitude, double latitude, out double x, out double y)
        {
            int ProjNo = 0;
            double longitude1, latitude1, longitude0, X0, Y0, xval, yval;
            double a, f, e2, ee, NN, T, C, A, M, iPI;
            iPI = PI / 180.0; ////3.1415926535898/180.0;
            a = _a; f = 1.0 / _alfa; //54年北京坐标系参数
            ////a=6378140.0; f=1/298.257; //80年西安坐标系参数
            ProjNo = (int)(longitude / _zoneWide);
            longitude0 = ProjNo * _zoneWide + _zoneWide / 2;
            longitude0 = longitude0 * iPI;
            longitude1 = longitude * iPI; //经度转换为弧度
            latitude1 = latitude * iPI; //纬度转换为弧度
            e2 = 2 * f - f * f;
            ee = e2 * (1.0 - e2);
            NN = a / Math.Sqrt(1.0 - e2 * Math.Sin(latitude1) * Math.Sin(latitude1));
            T = Math.Tan(latitude1) * Math.Tan(latitude1);
            C = ee * Math.Cos(latitude1) * Math.Cos(latitude1);
            A = (longitude1 - longitude0) * Math.Cos(latitude1);
            M = a * ((1 - e2 / 4 - 3 * e2 * e2 / 64 - 5 * e2 * e2 * e2 / 256) * latitude1 - (3 * e2 / 8 + 3 * e2 * e2 / 32 + 45 * e2 * e2
            * e2 / 1024) * Math.Sin(2 * latitude1)
            + (15 * e2 * e2 / 256 + 45 * e2 * e2 * e2 / 1024) * Math.Sin(4 * latitude1) - (35 * e2 * e2 * e2 / 3072) * Math.Sin(6 * latitude1));
            xval = M + NN * Math.Tan(latitude1) * (A * A / 2 + (5 - T + 9 * C + 4 * C * C) * A * A * A * A / 24
            + (61 - 58 * T + T * T + 600 * C - 330 * ee) * A * A * A * A * A * A / 720);
            yval = NN * (A + (1 - T + C) * A * A * A / 6 + (5 - 18 * T + T * T + 72 * C - 58 * ee) * A * A * A * A * A / 120);
            X0 = 0;
            Y0 = 1000000L * (ProjNo + 1) + 500000L;
            xval = xval + X0; yval = yval + Y0;
            x = xval;
            y = yval;
        }

        /// <summary>
        /// 坐标点转化为经纬度
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        public static void XYtoLL(double x, double y, out double longitude, out double latitude)
        {
            int ProjNo;
            double longitude1, latitude1, longitude0, X0, Y0, xval, yval;
            double e1, e2, f, a, ee, NN, T, C, M, D, R, u, fai, iPI;
            iPI = PI / 180.0; ////3.1415926535898/180.0;
            a = _a; f = 1.0 / _alfa; //54年北京坐标系参数
            ////a=6378140.0; f=1/298.257; //80年西安坐标系参数
            ProjNo = (int)(y / 1000000L); //查找带号
            longitude0 = (ProjNo - 1) * _zoneWide + _zoneWide / 2;
            longitude0 = longitude0 * iPI; //中央经线
            Y0 = ProjNo * 1000000L + 500000L;
            X0 = 0;
            xval = y - Y0; yval = x - X0; //带内大地坐标
            e2 = 2 * f - f * f;
            e1 = (1.0 - Math.Sqrt(1 - e2)) / (1.0 + Math.Sqrt(1 - e2));
            ee = e2 / (1 - e2);
            M = yval;
            u = M / (a * (1 - e2 / 4 - 3 * e2 * e2 / 64 - 5 * e2 * e2 * e2 / 256));
            fai = u + (3 * e1 / 2 - 27 * e1 * e1 * e1 / 32) * Math.Sin(2 * u) + (21 * e1 * e1 / 16 - 55 * e1 * e1 * e1 * e1 / 32) * Math.Sin(
            4 * u)
            + (151 * e1 * e1 * e1 / 96) * Math.Sin(6 * u) + (1097 * e1 * e1 * e1 * e1 / 512) * Math.Sin(8 * u);
            C = ee * Math.Cos(fai) * Math.Cos(fai);
            T = Math.Tan(fai) * Math.Tan(fai);
            NN = a / Math.Sqrt(1.0 - e2 * Math.Sin(fai) * Math.Sin(fai));
            R = a * (1 - e2) / Math.Sqrt((1 - e2 * Math.Sin(fai) * Math.Sin(fai)) * (1 - e2 * Math.Sin(fai) * Math.Sin(fai)) * (1 - e2 * Math.Sin
            (fai) * Math.Sin(fai)));
            D = xval / NN;
            //计算经度(Longitude) 纬度(Latitude)
            longitude1 = longitude0 + (D - (1 + 2 * T + C) * D * D * D / 6 + (5 - 2 * C + 28 * T - 3 * C * C + 8 * ee + 24 * T * T) * D
            * D * D * D * D / 120) / Math.Cos(fai);
            latitude1 = fai - (NN * Math.Tan(fai) / R) * (D * D / 2 - (5 + 3 * T + 10 * C - 4 * C * C - 9 * ee) * D * D * D * D / 24
            + (61 + 90 * T + 298 * C + 45 * T * T - 256 * ee - 3 * C * C) * D * D * D * D * D * D / 720);
            //转换为度 DD
            longitude = longitude1 / iPI;
            latitude = latitude1 / iPI;
        }

    }
}
