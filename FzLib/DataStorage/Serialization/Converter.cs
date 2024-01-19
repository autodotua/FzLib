using Newtonsoft.Json;
using System;
using System.IO;
using System.Xml;
using Formatting = Newtonsoft.Json.Formatting;

namespace FzLib.DataStorage.Serialization
{
    [Obsolete]
    internal static class Converter
    {
        public static string GetJson(object obj, bool format = false)
        {
            return JsonConvert.SerializeObject(obj, format ? Formatting.Indented : Formatting.None);
        }

        public static string GetJson(object obj, JsonSerializerSettings settings)
        {
            if (settings == null)
            {
                settings = new JsonSerializerSettings();
            }
            return JsonConvert.SerializeObject(obj, settings);
        }

        public static T GetObjectFromJson<T>(string text, JsonSerializerSettings settings)
        {
            if (settings == null)
            {
                settings = new JsonSerializerSettings();
            }
            return JsonConvert.DeserializeObject<T>(text, settings);
        }

        public static string ConvertXmlToJson(string text)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(text);

            return JsonConvert.SerializeXmlNode(doc);
        }

        public static string ConvertJsonToXml(string text, string rootName = "root")
        {
            using (StringWriter sw = new StringWriter())
            {
                JsonConvert.DeserializeXmlNode(text, rootName).Save(sw);
                return sw.ToString();
            }
        }

        public static string GetXml<T>(T obj, JsonSerializerSettings settings)
        {
            return ConvertJsonToXml(GetJson(obj, settings));
        }

        public static string GetXml<T>(T obj)
        {
            return ConvertJsonToXml(GetJson(obj));
            //using (StringWriter sw = new StringWriter())
            //{
            //    XmlSerializer xml = new XmlSerializer(typeof(T));
            //    xml.Serialize(sw, obj);
            //    return sw.ToString();
            //}
        }

        public static T GetObjectFromXml<T>(string text)
        {
            return GetObjectFromJson<T>(ConvertXmlToJson(text), null);
            //   T obj = default;
            //   using (StreamReader reader = new StreamReader(text))
            //   {
            //       XmlSerializer xml = new XmlSerializer(typeof(T));
            //       obj = (T)xml.Deserialize(reader);
            //       return obj;
            //   }
        }
    }
}