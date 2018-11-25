using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using static FzLib.Geography.Format.GpxHelper;

namespace FzLib.Geography.Format
{
    public class GpxInfo
    {
        public static GpxInfo FromString(string gpxString)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(gpxString);
            XmlElement xmlGpx = xmlDoc["gpx"];
            if (xmlGpx == null)
            {
                throw new Exception("没有找到gpx元素");
            }
            GpxInfo info = new GpxInfo(xmlGpx);

            return info;
        }

        private GpxInfo(XmlElement xml)
        {
            LoadGpxInfoProperties(this, xml);
        }


        public string Creator { get; set; }
        public string Version { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string Url { get; set; }


        public string UrlName { get; set; }
        public DateTime Time { get; set; }
        public string KeyWords { get; set; }
        public double Distance { get; set; }

        public Dictionary<string, string> OtherProperties { get; private set; } = new Dictionary<string, string>();

        public List<GpxTrackInfo> Tracks { get; private set; } = new List<GpxTrackInfo>();
    }
    public class GpxTrackInfo
    {

        internal GpxTrackInfo(XmlNode xml)
        {
            LoadGpxTrackInfoProperties(this, xml);
        }
        public Dictionary<string, string> OtherProperties { get; private set; } = new Dictionary<string, string>();
        public string Name { get; set; }
        public string Description { get; set; }
        public List<GpxTrackPointInfo> Points { get; private set; } = new List<GpxTrackPointInfo>();
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

    }
    public class GpxTrackPointInfo
    {
        public GpxTrackPointInfo() { }
        internal GpxTrackPointInfo(XmlNode xml)
        {
            LoadGpxTrackPointInfoProperties(this, xml);
        }
        /// <summary>
        /// 经度
        /// </summary>
        public double Longitude { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public double Latitude { get; set; }
        public double Altitude { get; set; }
        public DateTime Time { get; set; }
        public Dictionary<string, string> OtherProperties { get; internal set; } = new Dictionary<string, string>();

    }

    internal class GpxHelper
    {
        public static void LoadGpxInfoProperties(GpxInfo info, XmlNode xml)
        {
            LoadGpxInfoProperties(info, xml.Attributes.Cast<XmlAttribute>());
            LoadGpxInfoProperties(info, xml.ChildNodes.Cast<XmlElement>());

        }
        public static void LoadGpxTrackInfoProperties(GpxTrackInfo info, XmlNode xml)
        {
            LoadGpxTrackInfoProperties(info, xml.Attributes.Cast<XmlAttribute>());
            LoadGpxTrackInfoProperties(info, xml.ChildNodes.Cast<XmlElement>());
        }
        public static void LoadGpxTrackPointInfoProperties(GpxTrackPointInfo info, XmlNode xml)
        {
            LoadGpxTrackPointInfoProperties(info, xml.Attributes.Cast<XmlAttribute>());
            LoadGpxTrackPointInfoProperties(info, xml.ChildNodes.Cast<XmlElement>());
        }
        private static void LoadGpxInfoProperties(GpxInfo info, IEnumerable<XmlNode> nodes)
        {
            foreach (var node in nodes)
            {
                switch (node.Name)
                {
                    case "creator":
                        info.Creator = node.InnerText;
                        break;
                    case "version":
                        info.Version = node.InnerText;
                        break;
                    case "name":
                        info.Name = node.InnerText;
                        break;
                    case "author":
                        info.Author = node.InnerText;
                        break;

                    case "url":
                        info.Url = node.InnerText;
                        break;
                    case "urlname":
                        info.UrlName = node.InnerText;
                        break;
                    case "time":
                        info.Time = DateTime.Parse(node.InnerText);
                        break;
                    case "keywords":
                        info.KeyWords = node.InnerText;
                        break;
                    case "distance":
                        info.Distance = double.Parse(node.InnerText);
                        break;
                    case "trk":
                        info.Tracks.Add(new GpxTrackInfo(node));
                        break;
                    default:
                        info.OtherProperties.Add(node.Name, node.InnerText);
                        break;

                }
            }
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

                            info.Points.Add(new GpxTrackPointInfo(ptNode));
                        }
                        break;
                    default:
                        info.OtherProperties.Add(node.Name, node.InnerText);
                        break;
                }
            }
        }
        private static void LoadGpxTrackPointInfoProperties(GpxTrackPointInfo info, IEnumerable<XmlNode> nodes)
        {
            foreach (var node in nodes)
            {
                switch (node.Name)
                {
                    case "lat":
                        info.Latitude = double.Parse(node.InnerText);
                        break;
                    case "lon":
                        info.Longitude = double.Parse(node.InnerText);
                        break;
                    case "ele":
                        info.Altitude = double.Parse(node.InnerText);
                        break;
                    case "time":
                        info.Time = DateTime.Parse(node.InnerText);
                        break;
                    default:
                        info.OtherProperties.Add(node.Name, node.InnerText);
                        break;
                }
            }
        }
    }
}
