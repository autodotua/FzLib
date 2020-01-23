using FzLib.Geography;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using NetTopologySuite.Geometries;
using Geo = NetTopologySuite.Geometries.Geometry;

namespace FzLib.Geography.IO.Gpx
{
    public class GpxPoint : Point
    {
        public GpxPoint():base(0,0,double.NaN) {
            SRID = 4326;
        }
        internal GpxPoint(XmlNode xml):this()
        {
            LoadGpxTrackPointInfoProperties(xml);
        }
        public DateTime Time { get; set; }
        public double Speed { get; set; }
        public Dictionary<string, string> OtherProperties { get; internal set; } = new Dictionary<string, string>();


        public void LoadGpxTrackPointInfoProperties(IEnumerable<XmlNode> nodes)
        {

            foreach (var node in nodes)
            {
                switch (node.Name)
                {
                    case "lat":
                        Y = double.Parse(node.InnerText);
                        break;
                    case "lon":
                        X = double.Parse(node.InnerText);
                        break;
                    case "ele":
                        if (double.TryParse(node.InnerText, out double result))
                        {
                            Z = result;
                        }
                        break;
                    case "time":
                        if (DateTime.TryParse(node.InnerText, CultureInfo.CurrentCulture, DateTimeStyles.AdjustToUniversal, out DateTime time))
                        {
                            Time = time;
                        }
                        break;
                    default:
                        OtherProperties.Add(node.Name, node.InnerText);
                        break;
                }
            }

        }

        public void LoadGpxTrackPointInfoProperties(XmlNode xml)
        {
            try
            {
                LoadGpxTrackPointInfoProperties(xml.Attributes.Cast<XmlAttribute>());
                LoadGpxTrackPointInfoProperties(xml.ChildNodes.Cast<XmlElement>());
            }
            catch (Exception ex)
            {
                throw new Exception("解析点失败", ex);
            }
        }

        internal void WriteGpxXml(XmlDocument doc, XmlElement trkpt)
        {
            trkpt.SetAttribute("lat", Y.ToString());
            trkpt.SetAttribute("lon",X.ToString());
            AppendChildNode("ele", Z.ToString());
            //foreach (var item in OtherProperties)
            //{
            //    AppendChildNode(item.Key, item.Value);
            //}

            AppendChildNode("time", Time.ToString(Gpx.GpxTimeFormat));

            void AppendChildNode(string name, string value)
            {
                XmlElement child = doc.CreateElement(name);
                child.InnerText = value;
                trkpt.AppendChild(child);
            }
        }

        protected override Geo CopyInternal()
        {
            GpxPoint p = new GpxPoint()
            {
                X = X,
                Y = Y,
                Z = Z,
                Time = Time,
                Speed = Speed,
            };
            foreach (var o in OtherProperties)
            {
                p.OtherProperties.Add(o.Key, o.Value);
            }
            return p;
        }
    }
}
