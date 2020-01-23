﻿using FzLib.Geography;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using NetTopologySuite.Geometries;
using FzLib.Geography.Geometry;
using FzLib.Geography.Base;
using FzLib.Geography.Analysis;
using GIS.Geometry;

namespace FzLib.Geography.IO.Gpx
{
    public class GpxPointCollection : ObservableGeometryCollection<GpxPoint>
    {

        public new GpxPoint this[int index] => base[index] as GpxPoint;

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            isOrdered = null;
            timeOrderedPoints = null;
            //switch (e.Action)
            //{
            //    case NotifyCollectionChangedAction.Add:
            //        extent.Update(e.NewItems as IEnumerable<Point>);
            //        break;
            //    default:
            //        extent = null;
            //        break;
            //}
            base.OnCollectionChanged(e);
        }
        private GpxPointCollection timeOrderedPoints = null;
        public GpxPointCollection TimeOrderedPoints
        {
            get
            {
                if (IsOrdered)
                {
                    return this;
                }
                if (timeOrderedPoints == null)
                {
                    timeOrderedPoints = new GpxPointCollection(this.OrderBy(p => p.Time));
                    timeOrderedPoints.isOrdered = true;
                }
                return timeOrderedPoints;
            }
        }

        private bool? isOrdered = null;

        public GpxPointCollection()
        {
        }

        public GpxPointCollection(IEnumerable<GpxPoint> points) 
        {
            foreach (var p in points)
            {
                Add(p);
            }
        }



        public bool IsOrdered
        {
            get
            {
                if (isOrdered == null)
                {
                    bool ok = true;
                    GpxPoint last = default;
                    foreach (var item in this)
                    {
                        if (last != default)
                        {
                            if (last.Time > item.Time)
                            {
                                ok = false;
                                break;
                            }
                        }

                        last = item;
                    }
                    isOrdered = ok;
                }
                return isOrdered.Value;
            }
        }
        public double GetSpeed(GpxPoint point, int unilateralSampleCount)
        {
            GpxPointCollection points = this;
            if (!IsOrdered)
            {
                points = TimeOrderedPoints;
            }
            return points.GetSpeed(points.IndexOf(point), unilateralSampleCount);
        }
        public double GetSpeed(int index, int unilateralSampleCount)
        {
            if (!IsOrdered)
            {
                throw new Exception("点集合不符合时间顺序");
            }
            if (Count <= 1)
            {
                throw new Exception("集合拥有的点过少");
            }

            int min = index - unilateralSampleCount;
            if (min < 0)
            {
                min = 0;
            }

            int max = index + unilateralSampleCount;
            if (max > Count - 1)
            {
                max = Count - 1;
            }
            double totalDistance = 0;
            TimeSpan totalTime = TimeSpan.Zero;

            for (int i = min; i < max; i++)
            {
                totalDistance += Calculate.Distance(this[i], this[i + 1]);
                totalTime += this[i + 1].Time - this[i].Time;
            }
            return totalDistance / totalTime.TotalSeconds;

        }

        public override Envelope Extent
        {
            get
            {
                if (extent == null)
                {
                    extent = GeometryHelper.GetEnvelope(this);
                }
                return extent;
            }
        }


        private Envelope extent;
 
 
        public override ObservableGeometryCollection<GpxPoint> Clone()
        {
            GpxPoint[] points = new GpxPoint[Count];
            for (int i = 0; i < Count; i++)
            {
                points[i] = this[i].Copy() as GpxPoint;
            }
            return new GpxPointCollection(points);
        }
    }

}
