using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;

namespace FzLib.Geography.Coordinate
{
    public class GeoPoint : ICloneable
    {
        public double Latitude { get; set; }

        public double Longitude { get; set; }
        public double? Altitude { get; set; }
        public double? Speed { get; set; }
        public GeoPoint()
        {
        }
        public GeoPoint(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
        public GeoPoint(double latitude, double longitude, DateTime time)
        {
            Latitude = latitude;
            Longitude = longitude;
            Time = time;
        }
        public Dictionary<string, string> OtherProperties { get; private set; } = new Dictionary<string, string>();
        public DateTime? Time { get; set; }

        public static void LoadGpxTrackPointInfoProperties(GeoPoint info, XmlNode xml)
        {
            LoadGpxTrackPointInfoProperties(info, xml.Attributes.Cast<XmlAttribute>());
            LoadGpxTrackPointInfoProperties(info, xml.ChildNodes.Cast<XmlElement>());
        }

        private static void LoadGpxTrackPointInfoProperties(GeoPoint info, IEnumerable<XmlNode> nodes)
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
                        if (double.TryParse(node.InnerText, out double result))
                        {
                            info.Altitude = result;
                        }
                        break;
                    case "time":
                        if (DateTime.TryParse(node.InnerText, CultureInfo.CurrentCulture, DateTimeStyles.AdjustToUniversal, out DateTime time))
                        {
                            info.Time = time;
                        }
                        break;
                    default:
                        info.OtherProperties.Add(node.Name, node.InnerText);
                        break;
                }
            }
        }

        internal void WriteGpxXml(XmlDocument doc, XmlElement trkpt)
        {
            trkpt.SetAttribute("lat", Latitude.ToString());
            trkpt.SetAttribute("lon", Longitude.ToString());
            AppendChildNode("ele", Altitude.ToString());
            foreach (var item in OtherProperties)
            {
                AppendChildNode(item.Key, item.Value);
            }
            if (Time != null)
            {
                AppendChildNode("time", Time.Value.ToString(Constant.GpxTimeFormat));
            }
            void AppendChildNode(string name, string value)
            {
                XmlElement child = doc.CreateElement(name);
                child.InnerText = value;
                trkpt.AppendChild(child);
            }
        }


        public GeoPoint Clone()
        {
            return MemberwiseClone() as GeoPoint;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}
