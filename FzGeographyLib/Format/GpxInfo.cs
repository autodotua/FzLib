using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;

namespace FzLib.Geography.Format
{
    public class GpxInfo: ICloneable
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
        public GpxInfo()
        {

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


        public static void LoadGpxInfoProperties(GpxInfo info, XmlNode xml)
        {
            LoadGpxInfoProperties(info, xml.Attributes.Cast<XmlAttribute>());
            LoadGpxInfoProperties(info, xml.ChildNodes.Cast<XmlElement>());

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
                    case "distance":
                        if (double.TryParse(node.InnerText, out double result))
                        {
                            info.Distance = result;
                        }
                        break;
                    case "time":
                        if (DateTime.TryParse(node.InnerText,  CultureInfo.CurrentCulture,DateTimeStyles.AdjustToUniversal, out DateTime time))
                        {
                            info.Time = time;
                        }
                        break;
                    case "keywords":
                        info.KeyWords = node.InnerText;
                        break;

                    case "trk":
                        info.Tracks.Add(new GpxTrackInfo(node, info));
                        break;
                    default:
                        info.OtherProperties.Add(node.Name, node.InnerText);
                        break;

                }
            }
        }

        public string ToGpxXml()
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "utf-8", "no");
            doc.AppendChild(dec);


            XmlElement root = doc.CreateElement("gpx"); doc.AppendChild(root);
            


            root.SetAttribute("version", Version);
            root.SetAttribute("creator", Creator);
            AppendChildNode( "name", Name);
            AppendChildNode( "author", Author);
            AppendChildNode( "url", Url);
            AppendChildNode( "urlname", UrlName);
            AppendChildNode( "time", Time.ToString(Constant. GpxTimeFormat));
            AppendChildNode( "keywords", KeyWords);
            AppendChildNode( "distance", Distance.ToString());
            foreach (var item in OtherProperties)
            {
                if (item.Key.Contains("xmlns") )
                {
                }
                else if (item.Key.Contains(":"))
                {
                    
                }
                else
                {
                    AppendChildNode(item.Key, item.Value);
                }
            }
            foreach (var trk in Tracks)
            {
                XmlElement node = doc.CreateElement("trk");
                trk.WriteGpxXml(doc, node);
                root.AppendChild(node);
            }

            return GetXmlString();

            void AppendChildNode(string name, string value)
            {
                XmlElement child = doc.CreateElement(name);
                child.InnerText = value;
                root.AppendChild(child);
            }
            string GetXmlString()
            {
                string xmlString = null;
                using (var stringWriter = new StringWriter())
                {
                    XmlWriterSettings xmlSettingsWithIndentation = new XmlWriterSettings
                    {
                        Indent = true,
                        IndentChars = "\t",
                    };
                    using (var xmlTextWriter = XmlWriter.Create(stringWriter, xmlSettingsWithIndentation))
                    {
                        doc.WriteTo(xmlTextWriter);
                        xmlTextWriter.Flush();
                        xmlString = stringWriter.GetStringBuilder().ToString();
                    }
                }

                return xmlString;
            }

        }


        public GpxInfo Clone()
        {
            var info = MemberwiseClone() as GpxInfo;
            info.Tracks = Tracks.Select(p => p.Clone()).ToList();
            return info;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}
