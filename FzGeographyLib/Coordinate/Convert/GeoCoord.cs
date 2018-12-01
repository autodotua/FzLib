using System;

namespace FzLib.Geography.Coordinate.Convert
{
    public static class GeoCoordConverter
    {
        private const double a = 6378245.0;

        private const double ee = 0.0066934216229659433;


        public static GeoPoint WGS84ToGCJ02(GeoPoint wgLoc)
        {
            if (OutOfChina(wgLoc.Latitude, wgLoc.Longitude))
            {
                return new GeoPoint(wgLoc.Latitude, wgLoc.Longitude);
            }
            double num = TransformLat(wgLoc.Longitude - 105.0, wgLoc.Latitude - 35.0);
            double num2 = TransformLon(wgLoc.Longitude - 105.0, wgLoc.Latitude - 35.0);
            double d = wgLoc.Latitude / 180.0 * 3.1415926535897931;
            double num3 = Math.Sin(d);
            num3 = 1.0 - 0.0066934216229659433 * num3 * num3;
            double num4 = Math.Sqrt(num3);
            num = num * 180.0 / (6335552.7170004258 / (num3 * num4) * 3.1415926535897931);
            num2 = num2 * 180.0 / (6378245.0 / num4 * Math.Cos(d) * 3.1415926535897931);
            return new GeoPoint(wgLoc.Latitude + num, wgLoc.Longitude + num2);
        }

        public static GeoPoint WGS84ToBD09(double gg_lat, double gg_lon)
        {
            GeoPoint wgLoc = new GeoPoint(gg_lat, gg_lon);
            wgLoc = WGS84ToGCJ02(wgLoc);
            double num = 52.359877559829883;
            double longitude = wgLoc.Longitude;
            double latitude = wgLoc.Latitude;
            double num2 = Math.Sqrt(longitude * longitude + latitude * latitude) + 2E-05 * Math.Sin(latitude * num);
            double d = Math.Atan2(latitude, longitude) + 3E-06 * Math.Cos(longitude * num);
            wgLoc.Longitude = num2 * Math.Cos(d) + 0.0065;
            wgLoc.Latitude = num2 * Math.Sin(d) + 0.006;
            return wgLoc;
        }

        public static GeoPoint GCJ02ToWGS84(GeoPoint gcjPoint)
        {
            if (OutOfChina(gcjPoint.Latitude, gcjPoint.Longitude))
            {
                return gcjPoint;
            }
            GeoPoint latLng = Transform(gcjPoint);
            return new GeoPoint(gcjPoint.Latitude - latLng.Latitude, gcjPoint.Longitude - latLng.Longitude);
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

        private static GeoPoint Transform(GeoPoint point)
        {
            GeoPoint latLng = new GeoPoint(0.0, 0.0);
            double num = TransformLat(point.Longitude - 105.0, point.Latitude - 35.0);
            double num2 = TransformLon(point.Longitude - 105.0, point.Latitude - 35.0);
            double d = point.Latitude / 180.0 * 3.1415926535897931;
            double num3 = Math.Sin(d);
            num3 = 1.0 - 0.0066934216229659433 * num3 * num3;
            double num4 = Math.Sqrt(num3);
            num = num * 180.0 / (6335552.7170004258 / (num3 * num4) * 3.1415926535897931);
            num2 = num2 * 180.0 / (6378245.0 / num4 * Math.Cos(d) * 3.1415926535897931);
            latLng.Latitude = num;
            latLng.Longitude = num2;
            return latLng;
        }

       
    }
}
