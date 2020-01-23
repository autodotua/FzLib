using FzLib.Geography.Base;
using NetTopologySuite.Geometries;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace GIS.Geometry
{
    public abstract class ObservableGeometryCollection<T> : ObservableCollection<T>, ICloneable<ObservableGeometryCollection<T>> where T : Point
    {

        public ObservableGeometryCollection()
        {
        }

        public ObservableGeometryCollection(IEnumerable<T> points)
        {
            if (points == null)
            {
                throw new ArgumentNullException(nameof(points));
            }
            foreach (var point in points)
            {
                base.Add(point);
            }

            //GeoCollection = new ObservableCollection<T>(points) ?? throw new ArgumentNullException(nameof(points));
        }


        public abstract Envelope Extent { get; }
        public abstract ObservableGeometryCollection<T> Clone();
    }
}
