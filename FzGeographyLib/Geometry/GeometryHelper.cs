using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Text;
using Envelope = NetTopologySuite.Geometries.Envelope;

namespace FzLib.Geography.Geometry
{
    public static class GeometryHelper
    {
        public static Envelope GetEnvelope(IEnumerable<Point> points)
        {
            InitializeValues(out double minX, out double minY, out double minZ, out double maxX, out double maxY, out double maxZ);
            foreach (var point in points)
            {
                minX = Math.Min(point.X, minX);
                maxX = Math.Max(point.X, maxX);
                minY = Math.Min(point.Y, minY);
                maxY = Math.Max(point.Y, maxY);
                //if (!double.IsNaN(point.Z))
                //{
                //    minZ = Math.Min(point.Z, minZ);
                //    maxZ = Math.Max(point.Z, maxZ);
                //}
            }

            return new Envelope(minX,maxX,minY,maxY);

        }

        private static void InitializeValues(out double minX, out double minY, out double minZ, out double maxX, out double maxY, out double maxZ)
        {
            minX = minY = minZ = double.MaxValue;
            maxX = maxY = maxZ = double.MinValue;
        }
    }
}
