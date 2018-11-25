using System;

namespace FzLib.Geography.Coordinate
{
    public static class Transformation
    {
        public const double meterPerDegree = 40075.04 / 360 * 1000;
        public static LatLng Move(LatLng p, double north, double east)
        {
            LatLng newP = new LatLng()
            {
                Latitude = p.Latitude + north / meterPerDegree,
                Longitude = p.Longitude + east / meterPerDegree * Math.Cos(Math.Abs(p.Latitude) * Math.PI / 180),
            };
            return newP;
        }
    }
}
