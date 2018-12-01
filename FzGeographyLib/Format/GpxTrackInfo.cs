using FzLib.Geography.Analysis;
using FzLib.Geography.Coordinate;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;

namespace FzLib.Geography.Format
{
    public class GpxTrackInfo:ICloneable
    {
        internal GpxInfo GpxInfo { get; set; }
        internal GpxTrackInfo(XmlNode xml, GpxInfo parent)
        {
            GpxInfo = parent;
            LoadGpxTrackInfoProperties(this, xml);
            Points.CollectionChanged += (p1, p2) =>
            {
                distance = -1;
                maxSpeed = -1;
                totalTime = null;
            };
        }
        public Dictionary<string, string> OtherProperties { get; private set; } = new Dictionary<string, string>();
        public string Name { get; set; }
        public string Description { get; set; }
        public GeoPointCollection Points { get; private set; } = new GeoPointCollection();

        public double distance = -1;
        public double Distance
        {
            get
            {
                if (distance == -1)
                {
                    distance = 0;
                    GeoPoint last = null;
                    foreach (var point in Points.TimeOrderedPoints)
                    {
                        if (last != null)
                        {
                            distance += DistanceAnalysis.GetDistance(last, point);
                        }
                        last = point;
                    }
                }
                return distance;
            }
        }

        public double AverageSpeed => Distance / TotalTime.TotalSeconds;
        private TimeSpan? totalTime = null;
        public TimeSpan TotalTime
        {
            get
            {
                if (totalTime == null)
                {
                    totalTime = Points.TimeOrderedPoints[Points.TimeOrderedPoints.Count - 1].Time.Value - Points.TimeOrderedPoints[0].Time.Value;
                }
                return totalTime.Value;
            }
        }
        private double maxSpeed = -1;
        public double MaxSpeed
        {
            get
            {
                if(maxSpeed==-1)
                {
                    maxSpeed = SpeedAnalysis.GetSpeeds(Points).Max(p => p.Speed);
                }
                return maxSpeed;
            }
        }

        public double GetMaxSpeed(int sampleCount=8, int jump=1)
        {
            return SpeedAnalysis.GetFilteredSpeeds(Points, sampleCount, jump).Max(p => p.Speed) ;
        }
        public TimeSpan GetMovingTime(double speedDevaluation=0.3)
        {
            double totalDistance = 0;
            double totalSeconds = 0;
            GeoPoint last = null;
            foreach (var point in Points.TimeOrderedPoints)
            {
                if (last != null)
                {
                    double distance = DistanceAnalysis.GetDistance(last, point);
                    double second = (point.Time.Value - last.Time.Value).TotalSeconds;
                    double speed = distance / second;
                    if (speed > speedDevaluation)
                    {
                        totalDistance += distance;
                        totalSeconds += second;
                    }
                }
                last = point;
            }
            return TimeSpan.FromSeconds(totalSeconds);
        }
        public double GetMovingAverageSpeed(double speedDevaluation=0.3)
        {
            double totalDistance = 0;
            double totalSeconds = 0;
            GeoPoint last = null;
            foreach (var point in Points.TimeOrderedPoints)
            {
                if (last != null)
                {
                    double distance = DistanceAnalysis.GetDistance(last, point);
                    double second = (point.Time.Value - last.Time.Value).TotalSeconds;
                    double speed = distance / second;
                    if (speed > speedDevaluation)
                    {
                        totalDistance += distance;
                        totalSeconds += second;
                    }
                }
                last = point;
            }
            return totalDistance / totalSeconds;
        }

        //public IEnumerable<MapPoint> GetLatLngPoints() => Points.Select(p => p.GetLatLng());
        //public List<GpxTrackPointInfo> GetOffsetPoints(double north, double east)
        //  {
        //      List<GpxTrackPointInfo> newPoints = new List<GpxTrackPointInfo>();
        //      foreach (var point in Points)
        //      {
        //          var newPoint = new GpxTrackPointInfo()
        //          {
        //              Altitude = point.Altitude,
        //              OtherProperties = point.OtherProperties,
        //              Time = point.Time,
        //              Latitude = point.Latitude + north / meterPerDegree,
        //              Longitude = point.Longitude + east / meterPerDegree * Math.Cos(Math.Abs( point.Latitude )* Math.PI / 180),
        //          };
        //          if(newPoint.Longitude>180)
        //          {
        //              newPoint.Longitude = newPoint.Longitude - 360;
        //          }
        //          else if(newPoint.Longitude<-180)
        //          {
        //              newPoint.Longitude = newPoint.Longitude + 360;
        //          }
        //          newPoints.Add(newPoint);
        //      }
        //      return newPoints;
        //  }

        public static void LoadGpxTrackInfoProperties(GpxTrackInfo info, XmlNode xml)
        {
            LoadGpxTrackInfoProperties(info, xml.Attributes.Cast<XmlAttribute>());
            LoadGpxTrackInfoProperties(info, xml.ChildNodes.Cast<XmlElement>());
        }
        private static void LoadGpxTrackInfoProperties(GpxTrackInfo info, IEnumerable<XmlNode> nodes)
        {
            foreach (var node in nodes)
            {
                switch (node.Name)
                {
                    case "name":
                        info.Name = node.InnerText;
                        break;
                    case "desc":
                        info.Description = node.InnerText;
                        break;
                    case "trkseg":
                        foreach (XmlNode ptNode in node.ChildNodes)
                        {
                            GeoPoint point = new GeoPoint();
                            GeoPoint.LoadGpxTrackPointInfoProperties(point, ptNode);
                            info.Points.Add(point);
                        }
                        break;
                    default:
                        info.OtherProperties.Add(node.Name, node.InnerText);
                        break;
                }
            }
        }

        internal void WriteGpxXml(XmlDocument doc, XmlNode trk)
        {
            AppendChildNode("name", Name);
            AppendChildNode("desc", Description);
            foreach (var item in OtherProperties)
            {
                AppendChildNode(item.Key, item.Value);
            }
            XmlElement pointsNode = doc.CreateElement("trkseg");
            trk.AppendChild(pointsNode);
            foreach (var point in Points)
            {
                XmlElement node = doc.CreateElement("trkpt");
                point.WriteGpxXml(doc, node);
                pointsNode.AppendChild(node);
            }
            void AppendChildNode(string name, string value)
            {
                XmlElement child = doc.CreateElement(name);
                child.InnerText = value;
                trk.AppendChild(child);
            }
        }

        public GpxTrackInfo Clone()
        {
            GpxTrackInfo info = MemberwiseClone() as GpxTrackInfo;
            info.Points = Points.Clone();
            return info;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}
