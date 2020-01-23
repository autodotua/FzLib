﻿using FzLib.DataAnalysis;
using FzLib.Extension;
using FzLib.Geography;
using FzLib.Geography.IO.Gpx;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FzLib.Geography.Analysis
{
    public class SpeedAnalysis
    {
        public static double GetSpeed(IEnumerable<GpxPoint> points)
        {
            if (points.Any(p => p.Time == null))
            {
                throw new Exception("其中一个点的时间为空");
            }
            var sortedPoints = points.OrderBy(p => p.Time);
            TimeSpan totalTime = sortedPoints.Last().Time - sortedPoints.First().Time;
            double totalDistance = 0;
            GpxPoint last = null;
            foreach (var point in sortedPoints)
            {
                if (last != null)
                {
                    totalDistance += Calculate.Distance(last, point);
                }
                last = point;
            }
            return totalDistance / totalTime.TotalSeconds;
        }
        public static double GetSpeed(GpxPoint point1, GpxPoint point2)
        {
            if (point1.Time == null || point2.Time == null)
            {
                throw new Exception("其中一个点的时间为空");
            }
            return GetSpeed(point1, point2, TimeSpan.FromMilliseconds(Math.Abs((point1.Time - point2.Time).TotalMilliseconds)));
        }
        public static double GetSpeed(Point point1, Point point2, TimeSpan time)
        {
            double distance = Calculate.Distance(point1, point2);
            return distance / time.TotalSeconds;
        }

        public static IEnumerable<SpeedInfo> GetUsableSpeeds(GpxPointCollection points, int sampleCount = 2)
        {
            //Queue<GpxPoint> previousPoints = new Queue<GpxPoint>();
            //foreach (var point in points.TimeOrderedPoints)
            //{
            //    if (previousPoints.Count < sampleCount - 1)
            //    {
            //        previousPoints.Enqueue(point);
            //    }
            //    else
            //    {
            //        previousPoints.Enqueue(point);
            //        yield return new SpeedInfo(previousPoints);
            //        previousPoints.Dequeue();
            //    }
            //}
            //GpxPoint last = null;
            //foreach (var point in points)
            //{
            //    if (last != null)
            //    {
            //        SpeedInfo info = new SpeedInfo(last, point);
            //        if (!(double.IsNaN(info.Speed) || double.IsInfinity(info.Speed)))
            //        {
            //            yield return info;
            //        }
            //    }
            //    last = point;
            //}

            return GetSpeeds(points,sampleCount).Where(p=>!(double.IsNaN(p.Speed) || double.IsInfinity(p.Speed)));
        }
        public static IEnumerable<SpeedInfo> GetSpeeds(GpxPointCollection points,int sampleCount=2)
        {
            Queue<GpxPoint> previousPoints = new Queue<GpxPoint>();
            foreach (var point in points.TimeOrderedPoints)
            {
                if(previousPoints.Count< sampleCount-1)
                {
                    previousPoints.Enqueue(point);
                }
                else
                {
                    previousPoints.Enqueue(point);
                    yield return new SpeedInfo(previousPoints);
                    previousPoints.Dequeue();
                }
            }
        }

        /// <summary>
        /// 获取一组点经过滤波后的速度
        /// </summary>
        /// <param name="points">点的集合</param>
        /// <param name="sampleCount">每一组采样点的个数</param>
        /// <param name="jump">每一次循环跳跃的个数。比如设置5，采样10，那么第一轮1-10，第二轮6-15</param>
        /// <returns></returns>
        public static IEnumerable<SpeedInfo> GetMeanFilteredSpeeds(GpxPointCollection points, int sampleCount, int jump ,double min=double.MinValue,double max=double.MaxValue)
        {
            var sortedPoints = points.TimeOrderedPoints;
            if (sampleCount > sortedPoints.Count)
            {
                return new SpeedInfo[] { new SpeedInfo(sortedPoints) };
            }
            GpxPoint last = null;
            List<double> distances = new List<double>();
            foreach (var point in points)
            {
                if (last != null)
                {
                    distances.Add(Calculate.Distance(last, point));
                }
                last = point;
            }
            List<SpeedInfo> speedList = new List<SpeedInfo>();
            for (int i = sampleCount - 1; i < sortedPoints.Count; i+=jump)
            {
                DateTime minTime = sortedPoints[i - sampleCount + 1].Time;
                DateTime maxTime = sortedPoints[i].Time;
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
        public static IEnumerable<SpeedInfo> GetMedianFilteredSpeeds(GpxPointCollection points,
            int sampleCount, int jump, TimeSpan? maxTimeSpan=null,
            double min=double.MinValue,double max=double.MaxValue
           )
        {
           var filterResult= Filter.MedianValueFilter(GetSpeeds(points), p => p.Speed, sampleCount, jump);

            //List<SpeedInfo> speeds = new List<SpeedInfo>();
            foreach (var item in filterResult)
            {
                if(item.SelectedItem.Speed>max || item.SelectedItem.Speed<min)
                {
                    continue;
                }
                var maxTime = item.ReferenceItems.First().CenterTime;
                var minTime = item.ReferenceItems.Last().CenterTime;
                if(maxTimeSpan.HasValue &&maxTime - minTime> maxTimeSpan)
                {
                    continue;
                }
                SpeedInfo speed = new SpeedInfo(minTime,maxTime, item.SelectedItem.Speed);
                yield return speed;
            }

            //var sortedPoints = points.TimeOrderedPoints;
            //if (sampleCount > sortedPoints.Count)
            //{
            //    return new SpeedInfo[] { new SpeedInfo(sortedPoints) };
            //}
            //GpxPoint last = null;
            //List<double> distances = new List<double>();
            //foreach (var point in points)
            //{
            //    if (last != null)
            //    {
            //        distances.Add(Calculate.Distance(last, point));
            //    }
            //    last = point;
            //}
            //List<SpeedInfo> speedList = new List<SpeedInfo>();
            //for (int i = sampleCount - 1; i < sortedPoints.Count; i+=jump)
            //{
            //    DateTime minTime = sortedPoints[i - sampleCount + 1].Time;
            //    DateTime maxTime = sortedPoints[i].Time;
            //    double totalDistance = 0;
            //    for (int j = i - sampleCount + 1; j < i; j++)
            //    {
            //        totalDistance += distances[j];
            //    }
            //    double speed = totalDistance / (maxTime - minTime).TotalSeconds;
            //    if(speed<min)
            //    {
            //        continue;
            //    }
            //    if(speed>max)
            //    {
            //        continue;
            //    }
            //    speedList.Add(new SpeedInfo(minTime, maxTime, speed));

            //}
            //return speedList;
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
            public SpeedInfo(IEnumerable<GpxPoint> points) : this(points.ToArray())
            {

            }
            public SpeedInfo(params GpxPoint[] points)
            {
                List<GpxPoint> relatedPointList = new List<GpxPoint>();
                DateTime minTime = DateTime.MaxValue;
                DateTime maxTime = DateTime.MinValue;
                foreach (var point in points)
                {
                    if (point.Time < minTime)
                    {
                        minTime = point.Time;
                    }
                    if (point.Time > maxTime)
                    {
                        maxTime = point.Time;
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

            public GpxPoint[] RelatedPoints { get; private set; }
            public TimeSpan TimeSpan { get; private set; }
            public DateTime CenterTime { get; private set; }
            public double Speed { get; private set; }
        }
    }
}
