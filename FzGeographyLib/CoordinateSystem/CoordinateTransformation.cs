using GeoAPI.CoordinateSystems.Transformations;
using System;
using System.Collections.Generic;
using System.Text;
using cs = ProjNet.CoordinateSystems;
using GeoAPI.CoordinateSystems;
using NetTopologySuite.Geometries;

namespace FzLib.Geography.CoordinateSystem
{
    public static class CoordinateTransformation
    {
        public static Point TransformPoint(Point point, int srid, bool thisObject = false)
        {
            var trans = getInstance(point.SRID, srid);
            Point result = thisObject ? point : point.Copy() as Point;
            double[] newPoint;
            if (point.Z != double.NaN)
            {
                newPoint = trans.MathTransform.Transform(new double[] { point.X, point.Y });
                result.X = newPoint[0];
                result.Y = newPoint[1];
            }
            else
            {
                newPoint = trans.MathTransform.Transform(new double[] { point.X, point.Y, point.Z == double.NaN ? point.Z : 0 });
                result.X = newPoint[0];
                result.Y = newPoint[1];
                result.Z = newPoint[2];
            }
            result.SRID = srid;
            return result;
        }
        private static ICoordinateTransformation getInstance(int srid1, int srid2)
        {
            if (sridCache.ContainsKey((srid1, srid2)))
            {
                return sridCache[(srid1, srid2)];
            }

            ICoordinateTransformation trans = new cs.Transformations.CoordinateTransformationFactory()
                .CreateFromCoordinateSystems(CoordinateSystemExtend.Get(srid1), CoordinateSystemExtend.Get(srid2));
            sridCache.Add((srid1, srid2), trans);
            return trans;

        }

        internal static Dictionary<(int, int), ICoordinateTransformation> sridCache = new Dictionary<(int, int), ICoordinateTransformation>();


        public static Point TransformPoint(Point point, ICoordinateSystem to, bool thisObject = false)
        {
            return TransformPoint(point, CoordinateSystemExtend.Get(point.SRID), to, thisObject);
        }
        public static Point TransformPoint(Point point, ICoordinateSystem from, ICoordinateSystem to, bool thisObject = false)
        {
            var trans = getInstance(from, to);
            Point result = thisObject ? point : point.Copy() as Point;
            double[] newPoint;
            if (point.Z != double.NaN)
            {
                newPoint = trans.MathTransform.Transform(new double[] { point.X, point.Y });
                result.X = newPoint[0];
                result.Y = newPoint[1];
            }
            else
            {
                newPoint = trans.MathTransform.Transform(new double[] { point.X, point.Y, point.Z != double.NaN ? point.Z : 0 });
                result.X = newPoint[0];
                result.Y = newPoint[1];
                result.Z = newPoint[2];
            }
            //result.CoordinateSystem = coordinateSystem;
            return result;
        }

        private static ICoordinateTransformation getInstance(ICoordinateSystem coordinateSystem1, ICoordinateSystem coordinateSystem2)
        {
            if (cache.ContainsKey((coordinateSystem1.Name, coordinateSystem2.Name)))
            {
                return cache[(coordinateSystem1.Name, coordinateSystem2.Name)];
            }

            ICoordinateTransformation trans = new cs.Transformations.CoordinateTransformationFactory()
                .CreateFromCoordinateSystems(coordinateSystem1, coordinateSystem2);
            cache.Add((coordinateSystem1.Name, coordinateSystem2.Name), trans);
            return trans;

        }

        internal static Dictionary<(string, string), ICoordinateTransformation> cache = new Dictionary<(string, string), ICoordinateTransformation>();

    }
}
