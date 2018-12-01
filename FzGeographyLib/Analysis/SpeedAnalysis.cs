using FzLib.DataAnalysis;
using FzLib.Extension;
using FzLib.Geography.Coordinate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FzLib.Geography.Analysis
{
    public class SpeedAnalysis
    {
        public static double GetSpeed(IEnumerable<GeoPoint> points)
        {
            if (points.Any(p => p.Time == null))
            {
                throw new Exception("其中一个点的时间为空");
            }
            var sortedPoints = points.OrderBy(p => p.Time);
            TimeSpan totalTime = sortedPoints.Last().Time.Value - sortedPoints.First().Time.Value;
            double totalDistance = 0;
            GeoPoint last = null;
            foreach (var point in sortedPoints)
            {
                if (last != null)
                {
                    totalDistance += DistanceAnalysis.GetDistance(last, point);
                }
                last = point;
            }
            return totalDistance / totalTime.TotalSeconds;
        }
        public static double GetSpeed(GeoPoint point1, GeoPoint point2)
        {
            if (point1.Time == null || point2.Time == null)
            {
                throw new Exception("其中一个点的时间为空");
            }
            return GetSpeed(point1.Latitude, point1.Longitude, point2.Latitude, point2.Longitude, TimeSpan.FromMilliseconds(Math.Abs((point1.Time.Value - point2.Time.Value).TotalMilliseconds)));
        }
        public static double GetSpeed(GeoPoint point1, GeoPoint point2, TimeSpan time)
        {
            return GetSpeed(point1.Latitude, point1.Longitude, point2.Latitude, point2.Longitude, time);
        }
        public static double GetSpeed(double lat1, double lng1, double lat2, double lng2, TimeSpan time)
        {
            double distance = DistanceAnalysis.GetDistance(lat1, lng1, lat2, lng2);
            return distance / time.TotalSeconds;
        }
        public static IEnumerable<SpeedInfo> GetUsableSpeeds(IEnumerable<GeoPoint> points)
        {
            GeoPoint last = null;
            foreach (var point in points)
            {
                if (last != null)
                {
                    SpeedInfo info = new SpeedInfo(last, point);
                    if (!(double.IsNaN(info.Speed) || double.IsInfinity(info.Speed)))
                    {
                        yield return info;
                    }
                }
                last = point;
            }
        }
        public static IEnumerable<SpeedInfo> GetSpeeds(GeoPointCollection points)
        {
            List<SpeedInfo> speeds = new List<SpeedInfo>();
            GeoPoint last = null;
            foreach (var point in points.TimeOrderedPoints)
            {
                if (last != null)
                {
                    yield return new SpeedInfo(last, point);
                }
                last = point;
            }
        }

        /// <summary>
        /// 获取一组点经过滤波后的速度
        /// </summary>
        /// <param name="points">点的集合</param>
        /// <param name="sampleCount">每一组采样点的个数</param>
        /// <param name="jump">每一次循环跳跃的个数。比如设置5，采样10，那么第一轮1-10，第二轮6-15</param>
        /// <returns></returns>
        public static IEnumerable<SpeedInfo> GetFilteredSpeeds(GeoPointCollection points, int sampleCount, int jump ,double min=double.MinValue,double max=double.MaxValue)
        {
            var sortedPoints = points.TimeOrderedPoints;
            if (sampleCount > sortedPoints.Count)
            {
                return new SpeedInfo[] { new SpeedInfo(sortedPoints) };
            }
            GeoPoint last = null;
            List<double> distances = new List<double>();
            foreach (var point in points)
            {
                if (last != null)
                {
                    distances.Add(DistanceAnalysis.GetDistance(last, point));
                }
                last = point;
            }
            List<SpeedInfo> speedList = new List<SpeedInfo>();
            for (int i = sampleCount - 1; i < sortedPoints.Count; i+=jump)
            {
                DateTime minTime = sortedPoints[i - sampleCount + 1].Time.Value;
                DateTime maxTime = sortedPoints[i].Time.Value;
                double totalDistance = 0;
                for (int j = i - sampleCount + 1; j < i; j++)
                {
                    totalDistance += distances[j];
                }
                double speed = totalDistance / (maxTime - minTime).TotalSeconds;
                if(speed<min)
                {
                    continue;
                }
                if(speed>max)
                {
                    continue;
                }
                speedList.Add(new SpeedInfo(minTime, maxTime, speed));

            }
            return speedList;
        }

        public class SpeedInfo
        {
            public SpeedInfo(DateTime centerTime,double speed)
            {
                CenterTime = centerTime;
                Speed = speed;
            }
            public SpeedInfo(DateTime minTime, DateTime maxTime, double speed)
            {
                TimeSpan = maxTime - minTime;
                CenterTime = minTime + TimeSpan.FromMilliseconds(TimeSpan.TotalMilliseconds / 2);
                Speed = speed;
            }
            public SpeedInfo(GeoPointCollection points) : this(points.ToArray())
            {

            }
            public SpeedInfo(params GeoPoint[] points)
            {
                List<GeoPoint> relatedPointList = new List<GeoPoint>();
                DateTime minTime = DateTime.MaxValue;
                DateTime maxTime = DateTime.MinValue;
                foreach (var point in points)
                {
                    if (!point.Time.HasValue)
                    {
                        throw new Exception("其中一个点的时间为空");
                    }
                    if (point.Time.Value < minTime)
                    {
                        minTime = point.Time.Value;
                    }
                    if (point.Time.Value > maxTime)
                    {
                        maxTime = point.Time.Value;
                    }
                    relatedPointList.Add(point);
                }
                if (relatedPointList.Count < 2)
                {
                    throw new Exception("点数量过少");
                }
                RelatedPoints = relatedPointList.ToArray();
                TimeSpan = maxTime - minTime;
                CenterTime = minTime + TimeSpan.FromMilliseconds(TimeSpan.TotalMilliseconds / 2);
                if (relatedPointList.Count == 2)
                {
                    Speed = GetSpeed(RelatedPoints[0], RelatedPoints[1]);
                }
                else
                {
                    Speed = GetSpeed(RelatedPoints);
                }

            }

            public GeoPoint[] RelatedPoints { get; private set; }
            public TimeSpan TimeSpan { get; private set; }
            public DateTime CenterTime { get; private set; }
            public double Speed { get; private set; }
        }
    }
}
