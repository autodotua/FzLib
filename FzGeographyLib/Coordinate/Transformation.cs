using System;

namespace FzLib.Geography.Coordinate
{
    public static class Transformation
    {
        public const double meterPerDegree = 40075.04 / 360 * 1000;
        public static GeoPoint Move(GeoPoint p, double north, double east)
        {
            GeoPoint newP = new GeoPoint()
            {
                Latitude = p.Latitude + north / meterPerDegree,
                Longitude = p.Longitude + east / meterPerDegree * Math.Cos(Math.Abs(p.Latitude) * Math.PI / 180),
            };
            return newP;
        }
    }
}
