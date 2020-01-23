using FzLib.Geography;
using FzLib.Geography.CoordinateSystem;
using GeoAPI.CoordinateSystems;
using NetTopologySuite.Geometries;
using ProjNet.CoordinateSystems;
using System;
using System.Collections.Generic;
using System.Text;

namespace FzLib.Geography.Analysis
{
    public static class Calculate
    {

        public static double Distance(Point point1, Point point2, Ellipsoid ellipsoid)
        {
            return CalculateGeodeticCurve(ellipsoid, point1, point2).Length;
        }
        public static double Distance(Point point1, Point point2)
        {
            //一个有坐标系一个没有坐标系
            //if (point1.CoordinateSystem != null && point2.CoordinateSystem == null)
            //{
            //    point2 = point2.Clone() as Point;
            //    point2.SetCoordinateSystem(point1.CoordinateSystem, false);
            //}
            //else if (point1.CoordinateSystem == null && point2.CoordinateSystem != null)
            //{
            //    point1 = point1.Clone() as Point;
            //    point1.SetCoordinateSystem(point2.CoordinateSystem, false);
            //}
            ////两个坐标系不同
            //else if (point1.CoordinateSystem != point2.CoordinateSystem)
            //{
            //    if(point1.CoordinateSystem is ProjectedCoordinateSystem)
            //    point2 = CoordinateSystems.Transformations.CoordinateTransformation.TransformPoint(point2, point1.CoordinateSystem, false);
            //}
            //地理坐标系
            //if (point1.CoordinateSystem is GeographicCoordinateSystem)
            //{

            //如果两个都没有坐标系，直接求欧氏距离
            if (point1.SRID == 0 && point2.SRID == 0)
            {
                double x = point1.X - point2.X;
                double y = point1.Y - point2.Y;
                return Math.Sqrt(x * x + y * y);
            }
            ////一个有坐标系一个没有坐标系
            //if (point1.CoordinateSystem != null && point2.CoordinateSystem == null)
            //{
            //    point2 = point2.Clone() as Point;
            //    point2.SetCoordinateSystem(point1.CoordinateSystem, false);
            //}
            //else if (point1.CoordinateSystem == null && point2.CoordinateSystem != null)
            //{
            //    point1 = point1.Clone() as Point;
            //    point1.SetCoordinateSystem(point2.CoordinateSystem, false);
            //}
            //else
            //{
            //    point1 = GetWgs84Point(point1);
            //    point2 = GetWgs84Point(point2);
            //}
            if (!point1.SRID.Equals(point2.SRID))
            {
                throw new Exception("坐标系不一致");
            }
            var cs = CoordinateSystemExtend.Get(point1.SRID);
            if (cs is IGeographicCoordinateSystem)
            {
                return CalculateGeodeticCurve(
                 (  cs as IGeographicCoordinateSystem).HorizontalDatum.Ellipsoid as Ellipsoid,
                    point1, point2).Length;
            }
            else
            {
                point1 = CoordinateTransformation.TransformPoint(point1, 4326);
                point2 = CoordinateTransformation.TransformPoint(point2, 4326);
                return CalculateGeodeticCurve(Ellipsoid.WGS84, point1, point2).Length;
            }
            //double lat1 = point1.Latitude;
            //double lat2 = point2.Latitude;
            //double lng1 = point1.Longitude;
            //double lng2 = point2.Longitude;

            //var radLat1 = lat1 * Math.PI / 180.0;
            //var radLat2 = lat2 * Math.PI / 180.0;
            //var a = radLat1 - radLat2;
            //var b = lng1 * Math.PI / 180.0 - lng2 * Math.PI / 180.0;
            //var s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) + Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
            //s *= CoordinateSystem.WGS84.HorizontalDatum.Ellipsoid.SemiMajorAxis; //6378137;
            ////s = Math.Round(s * 10000) / 10000;

            //return s;
            //}



        }

        private static Point GetWgs84Point(Point point)
        {
            if(point.SRID==4326)
            {
                return point;
            }
            var cs = CoordinateSystemExtend.Get(point.SRID);
            if (cs is GeographicCoordinateSystem)
            {
                return CoordinateTransformation.TransformPoint(point, GeographicCoordinateSystem.WGS84, false);
            }
            else
            {
                return CoordinateTransformation.TransformPoint(point, GeographicCoordinateSystem.WGS84, false);
            }
        }

        public static GeodeticCurveInfo CalculateGeodeticCurve(IEllipsoid ellipsoid, Point start, Point end)
        {

            double a = ellipsoid.SemiMajorAxis;
            double b = ellipsoid.SemiMinorAxis;
            double f = 1 / ellipsoid.InverseFlattening;

            // get parameters as radians
            double phi1 =Angle.FromLatitude(start).Radians;
            double lambda1 = Angle.FromLongitude(start).Radians;
            double phi2 = Angle.FromLatitude(end).Radians;
            double lambda2 = Angle.FromLongitude(end).Radians;

            // calculations
            double a2 = a * a;
            double b2 = b * b;
            double a2b2b2 = (a2 - b2) / b2;

            double omega = lambda2 - lambda1;

            double tanphi1 = Math.Tan(phi1);
            double tanU1 = (1.0 - f) * tanphi1;
            double U1 = Math.Atan(tanU1);
            double sinU1 = Math.Sin(U1);
            double cosU1 = Math.Cos(U1);

            double tanphi2 = Math.Tan(phi2);
            double tanU2 = (1.0 - f) * tanphi2;
            double U2 = Math.Atan(tanU2);
            double sinU2 = Math.Sin(U2);
            double cosU2 = Math.Cos(U2);

            double sinU1sinU2 = sinU1 * sinU2;
            double cosU1sinU2 = cosU1 * sinU2;
            double sinU1cosU2 = sinU1 * cosU2;
            double cosU1cosU2 = cosU1 * cosU2;

            // eq. 13
            double lambda = omega;

            // intermediates we'll need to compute 's'
            double A = 0.0;
            double B = 0.0;
            double sigma = 0.0;
            double deltasigma = 0.0;
            double lambda0;
            bool converged = false;

            for (int i = 0; i < 20; i++)
            {
                lambda0 = lambda;

                double sinlambda = Math.Sin(lambda);
                double coslambda = Math.Cos(lambda);

                // eq. 14
                double sin2sigma = (cosU2 * sinlambda * cosU2 * sinlambda) + Math.Pow(cosU1sinU2 - sinU1cosU2 * coslambda, 2.0);
                double sinsigma = Math.Sqrt(sin2sigma);

                // eq. 15
                double cossigma = sinU1sinU2 + (cosU1cosU2 * coslambda);

                // eq. 16
                sigma = Math.Atan2(sinsigma, cossigma);

                // eq. 17    Careful!  sin2sigma might be almost 0!
                double sinalpha = (sin2sigma == 0) ? 0.0 : cosU1cosU2 * sinlambda / sinsigma;
                double alpha = Math.Asin(sinalpha);
                double cosalpha = Math.Cos(alpha);
                double cos2alpha = cosalpha * cosalpha;

                // eq. 18    Careful!  cos2alpha might be almost 0!
                double cos2sigmam = cos2alpha == 0.0 ? 0.0 : cossigma - 2 * sinU1sinU2 / cos2alpha;
                double u2 = cos2alpha * a2b2b2;

                double cos2sigmam2 = cos2sigmam * cos2sigmam;

                // eq. 3
                A = 1.0 + u2 / 16384 * (4096 + u2 * (-768 + u2 * (320 - 175 * u2)));

                // eq. 4
                B = u2 / 1024 * (256 + u2 * (-128 + u2 * (74 - 47 * u2)));

                // eq. 6
                deltasigma = B * sinsigma * (cos2sigmam + B / 4 * (cossigma * (-1 + 2 * cos2sigmam2) - B / 6 * cos2sigmam * (-3 + 4 * sin2sigma) * (-3 + 4 * cos2sigmam2)));

                // eq. 10
                double C = f / 16 * cos2alpha * (4 + f * (4 - 3 * cos2alpha));

                // eq. 11 (modified)
                lambda = omega + (1 - C) * f * sinalpha * (sigma + C * sinsigma * (cos2sigmam + C * cossigma * (-1 + 2 * cos2sigmam2)));

                // see how much improvement we got
                double change = Math.Abs((lambda - lambda0) / lambda);

                if ((i > 1) && (change < 0.0000000000001))
                {
                    converged = true;
                    break;
                }
            }

            // eq. 19
            double s = b * A * (sigma - deltasigma);
            Angle alpha1;
            Angle alpha2;

            // didn't converge?  must be N/S
            if (!converged)
            {
                if (phi1 > phi2)
                {
                    alpha1 = Angle.Half;
                    alpha2 = Angle.Zero;
                }
                else if (phi1 < phi2)
                {
                    alpha1 = Angle.Zero;
                    alpha2 = Angle.Half;
                }
                else
                {
                    alpha1 = Angle.Empty;
                    alpha2 = Angle.Empty;
                }
            }

            // else, it converged, so do the math
            else
            {
                double radians;
                alpha1 = 0;
                alpha2 = 0;

                // eq. 20
                radians = Math.Atan2(cosU2 * Math.Sin(lambda), (cosU1sinU2 - sinU1cosU2 * Math.Cos(lambda)));
                if (radians < 0.0) radians += Math.PI * 2;
                alpha1 = Angle.FromRadians(radians);

                // eq. 21
                radians = Math.Atan2(cosU1 * Math.Sin(lambda), (-sinU1cosU2 + cosU1sinU2 * Math.Cos(lambda))) + Math.PI;
                if (radians < 0.0) radians += Math.PI * 2;
                alpha2 = Angle.FromRadians(radians);
            }

            if (alpha1 >= 360.0) alpha1 -= 360.0;
            if (alpha2 >= 360.0) alpha2 -= 360.0;

            return new GeodeticCurveInfo(s, alpha1, alpha2);
        }

        public static Point CalculateEndingGlobalCoordinates(Ellipsoid ellipsoid, Point start, Angle startBearing, double distance)
        {
            double a = ellipsoid.SemiMajorAxis;
            double b = ellipsoid.SemiMinorAxis;
            double aSquared = a * a;
            double bSquared = b * b;
            double f = 1 / ellipsoid.InverseFlattening;
            double phi1 = Angle.FromLatitude(start).Radians;
            double alpha1 = startBearing.Radians;
            double cosAlpha1 = Math.Cos(alpha1);
            double sinAlpha1 = Math.Sin(alpha1);
            double s = distance;
            double tanU1 = (1.0 - f) * Math.Tan(phi1);
            double cosU1 = 1.0 / Math.Sqrt(1.0 + tanU1 * tanU1);
            double sinU1 = tanU1 * cosU1;

            // eq. 1
            double sigma1 = Math.Atan2(tanU1, cosAlpha1);

            // eq. 2
            double sinAlpha = cosU1 * sinAlpha1;

            double sin2Alpha = sinAlpha * sinAlpha;
            double cos2Alpha = 1 - sin2Alpha;
            double uSquared = cos2Alpha * (aSquared - bSquared) / bSquared;

            // eq. 3
            double A = 1 + (uSquared / 16384) * (4096 + uSquared * (-768 + uSquared * (320 - 175 * uSquared)));

            // eq. 4
            double B = (uSquared / 1024) * (256 + uSquared * (-128 + uSquared * (74 - 47 * uSquared)));

            // iterate until there is a negligible change in sigma
            double deltaSigma;
            double sOverbA = s / (b * A);
            double sigma = sOverbA;
            double sinSigma;
            double prevSigma = sOverbA;
            double sigmaM2;
            double cosSigmaM2;
            double cos2SigmaM2;

            for (; ; )
            {
                // eq. 5
                sigmaM2 = 2.0 * sigma1 + sigma;
                cosSigmaM2 = Math.Cos(sigmaM2);
                cos2SigmaM2 = cosSigmaM2 * cosSigmaM2;
                sinSigma = Math.Sin(sigma);
                double cosSignma = Math.Cos(sigma);

                // eq. 6
                deltaSigma = B * sinSigma * (cosSigmaM2 + (B / 4.0) * (cosSignma * (-1 + 2 * cos2SigmaM2)
                    - (B / 6.0) * cosSigmaM2 * (-3 + 4 * sinSigma * sinSigma) * (-3 + 4 * cos2SigmaM2)));

                // eq. 7
                sigma = sOverbA + deltaSigma;

                // break after converging to tolerance
                if (Math.Abs(sigma - prevSigma) < 0.0000000000001) break;

                prevSigma = sigma;
            }

            sigmaM2 = 2.0 * sigma1 + sigma;
            cosSigmaM2 = Math.Cos(sigmaM2);
            cos2SigmaM2 = cosSigmaM2 * cosSigmaM2;

            double cosSigma = Math.Cos(sigma);
            sinSigma = Math.Sin(sigma);

            // eq. 8
            double phi2 = Math.Atan2(sinU1 * cosSigma + cosU1 * sinSigma * cosAlpha1,
                                     (1.0 - f) * Math.Sqrt(sin2Alpha + Math.Pow(sinU1 * sinSigma - cosU1 * cosSigma * cosAlpha1, 2.0)));

            // eq. 9
            // This fixes the pole crossing defect spotted by Matt Feemster.  When a path
            // passes a pole and essentially crosses a line of latitude twice - once in
            // each direction - the longitude calculation got messed up.  Using Atan2
            // instead of Atan fixes the defect.  The change is in the next 3 lines.
            //double tanLambda = sinSigma * sinAlpha1 / (cosU1 * cosSigma - sinU1*sinSigma*cosAlpha1);
            //double lambda = Math.Atan(tanLambda);
            double lambda = Math.Atan2(sinSigma * sinAlpha1, cosU1 * cosSigma - sinU1 * sinSigma * cosAlpha1);

            // eq. 10
            double C = (f / 16) * cos2Alpha * (4 + f * (4 - 3 * cos2Alpha));

            // eq. 11
            double L = lambda - (1 - C) * f * sinAlpha * (sigma + C * sinSigma * (cosSigmaM2 + C * cosSigma * (-1 + 2 * cos2SigmaM2)));

            // eq. 12
            double alpha2 = Math.Atan2(sinAlpha, -sinU1 * sinSigma + cosU1 * cosSigma * cosAlpha1);

            // build result
            Angle latitude = new Angle();
            Angle longitude = new Angle();

            latitude.Radians = phi2;
            longitude.Radians =Angle.FromLongitude( start).Radians + L;

            //endBearing = new Angle();
            //endBearing.Radians = alpha2;
            return new Point(longitude.Degrees, latitude.Degrees) { SRID = 4326 };
        }


    }
}
