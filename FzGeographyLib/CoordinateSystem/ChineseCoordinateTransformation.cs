using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Text;

namespace FzLib.Geography.CoordinateSystem
{
    public static class ChineseCoordinateTransformation
    {
        private const double a = 6378245.0;

        private const double ee = 0.0066934216229659433;

        public static Point BD09ToWgs84(Point point)
        {
            var gcj = BD09ToGCJ02(point);
            var wgs84 = GCJ02ToWGS84(gcj);
            return wgs84;
        }

        public static Point BD09ToGCJ02(Point point)
        {
            var bd_lat = point.Y;
            var bd_lon = point.X;
            double x = bd_lon - 0.0065;
            double y = bd_lat - 0.006;
            double z = Math.Sqrt(x * x + y * y) - 0.00002 * Math.Sin(y * Math.PI);
            double theta = Math.Atan2(y, x) - 0.000003 * Math.Cos(x * Math.PI);
            double gg_lng = z * Math.Cos(theta);
            double gg_lat = z * Math.Sin(theta);
            return new Point(gg_lng, gg_lat);
        }

        public static Point WGS84ToGCJ02(Point wgLoc)
        {
            if (OutOfChina(wgLoc.Y, wgLoc.X))
            {
                return new Point(wgLoc.Y, wgLoc.X);
            }
            double num = TransformLat(wgLoc.X - 105.0, wgLoc.Y - 35.0);
            double num2 = TransformLon(wgLoc.X - 105.0, wgLoc.Y - 35.0);
            double d = wgLoc.Y / 180.0 * 3.1415926535897931;
            double num3 = Math.Sin(d);
            num3 = 1.0 - 0.0066934216229659433 * num3 * num3;
            double num4 = Math.Sqrt(num3);
            num = num * 180.0 / (6335552.7170004258 / (num3 * num4) * 3.1415926535897931);
            num2 = num2 * 180.0 / (6378245.0 / num4 * Math.Cos(d) * 3.1415926535897931);
            return new Point(wgLoc.X + num2, wgLoc.Y + num);
        }

        public static Point WGS84ToBD09(double gg_lat, double gg_lon)
        {
            Point wgLoc = new Point(gg_lat, gg_lon);
            wgLoc = WGS84ToGCJ02(wgLoc);
            double num = 52.359877559829883;
            double X = wgLoc.X;
            double Y = wgLoc.Y;
            double num2 = Math.Sqrt(X * X + Y * Y) + 2E-05 * Math.Sin(Y * num);
            double d = Math.Atan2(Y, X) + 3E-06 * Math.Cos(X * num);
            wgLoc.X = num2 * Math.Cos(d) + 0.0065;
            wgLoc.Y = num2 * Math.Sin(d) + 0.006;
            return wgLoc;
        }

        public static Point GCJ02ToWGS84(Point gcjPoint)
        {
            if (OutOfChina(gcjPoint.Y, gcjPoint.X))
            {
                return gcjPoint;
            }
            Point latLng = Transform(gcjPoint);
            return new Point(gcjPoint.X - latLng.X, gcjPoint.Y - latLng.Y);
        }

        private static double TransformLat(double x, double y)
        {
            double num = -100.0 + 2.0 * x + 3.0 * y + 0.2 * y * y + 0.1 * x * y + 0.2 * Math.Sqrt((x > 0.0) ? x : (0.0 - x));
            num += (20.0 * Math.Sin(6.0 * x * 3.1415926535897931) + 20.0 * Math.Sin(2.0 * x * 3.1415926535897931)) * 2.0 / 3.0;
            num += (20.0 * Math.Sin(y * 3.1415926535897931) + 40.0 * Math.Sin(y / 3.0 * 3.1415926535897931)) * 2.0 / 3.0;
            return num + (160.0 * Math.Sin(y / 12.0 * 3.1415926535897931) + 320.0 * Math.Sin(y * 3.1415926535897931 / 30.0)) * 2.0 / 3.0;
        }

        private static double TransformLon(double x, double y)
        {
            double num = 300.0 + x + 2.0 * y + 0.1 * x * x + 0.1 * x * y + 0.1 * Math.Sqrt((x > 0.0) ? x : (0.0 - x));
            num += (20.0 * Math.Sin(6.0 * x * 3.1415926535897931) + 20.0 * Math.Sin(2.0 * x * 3.1415926535897931)) * 2.0 / 3.0;
            num += (20.0 * Math.Sin(x * 3.1415926535897931) + 40.0 * Math.Sin(x / 3.0 * 3.1415926535897931)) * 2.0 / 3.0;
            return num + (150.0 * Math.Sin(x / 12.0 * 3.1415926535897931) + 300.0 * Math.Sin(x / 30.0 * 3.1415926535897931)) * 2.0 / 3.0;
        }

        public static bool OutOfChina(double lat, double lon)
        {
            if (lon < 72.004 || lon > 137.8347)
            {
                return true;
            }
            if (lat < 0.8293 || lat > 55.8271)
            {
                return true;
            }
            return false;
        }

        private static Point Transform(Point point)
        {
            Point latLng = new Point(0.0, 0.0);
            double num = TransformLat(point.X - 105.0, point.Y - 35.0);
            double num2 = TransformLon(point.X - 105.0, point.Y - 35.0);
            double d = point.Y / 180.0 * 3.1415926535897931;
            double num3 = Math.Sin(d);
            num3 = 1.0 - 0.0066934216229659433 * num3 * num3;
            double num4 = Math.Sqrt(num3);
            num = num * 180.0 / (6335552.7170004258 / (num3 * num4) * 3.1415926535897931);
            num2 = num2 * 180.0 / (6378245.0 / num4 * Math.Cos(d) * 3.1415926535897931);
            latLng.Y = num;
            latLng.X = num2;
            return latLng;
        }
    }
}