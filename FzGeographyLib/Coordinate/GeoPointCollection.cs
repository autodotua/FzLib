using FzLib.Geography.Analysis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace FzLib.Geography.Coordinate
{
    public class GeoPointCollection : ObservableCollection<GeoPoint>,ICloneable
    {
        public GeoPointCollection(IEnumerable<GeoPoint> collection) : base(collection)
        {
        }

        public GeoPointCollection()
        {
        }

        public GeoPointCollection(List<GeoPoint> list) : base(list)
        {
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);
            isOrdered = null;
            timeOrderedPoints = null;
        }
        private GeoPointCollection timeOrderedPoints = null;
        public GeoPointCollection TimeOrderedPoints
        {
            get
            {
                if (IsOrdered)
                {
                    return this;
                }
                if (timeOrderedPoints == null)
                {
                    timeOrderedPoints = new GeoPointCollection(this.Where(p => p.Time.HasValue).OrderBy(p => p.Time.Value));
                    timeOrderedPoints.isOrdered = true;
                }
                return timeOrderedPoints;
            }
        }

        public bool? isOrdered = null;
        
        public bool IsOrdered
        {
            get
            {
                if (isOrdered == null)
                {
                    bool ok = true;
                    GeoPoint last = default;
                    foreach (var item in this)
                    {
                        if (last != default)
                        {
                            if (last.Time.HasValue && item.Time.HasValue)
                            {
                                if (last.Time.Value > item.Time.Value)
                                {
                                    ok = false;
                                    break;
                                }
                            }
                        }

                        last = item;
                    }
                    isOrdered = ok;
                }
                return isOrdered.Value;
            }
        }
        public double GetSpeed(GeoPoint point, int unilateralSampleCount)
        {
            GeoPointCollection points = this;
            if (!IsOrdered)
            {
                points = TimeOrderedPoints;
            }
            return points.GetSpeed(IndexOf(point), unilateralSampleCount);
        }
            public double GetSpeed(int index, int unilateralSampleCount)
        {
            if (!IsOrdered)
            {
                throw new Exception("点集合不符合时间顺序");
            }
            if(Count<=1)
            {
                throw new Exception("集合拥有的点过少");
            }

            int min = index - unilateralSampleCount;
            if(min<0)
            {
                min = 0;
            }

            int max = index + unilateralSampleCount;
            if(max > Count-1)
            {
                max = Count - 1;
            }
            double totalDistance = 0;
            TimeSpan totalTime = TimeSpan.Zero;

            for(int i=min;i<max;i++)
            {
                totalDistance += DistanceAnalysis.GetDistance(this[i], this[i + 1]);
                totalTime += this[i+1].Time.Value - this[i].Time.Value;
            }
            return totalDistance / totalTime.TotalSeconds;

        }
        
        public GeoPointCollection Clone()
        {
            return new GeoPointCollection(this.Select(p => p.Clone()));
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}
