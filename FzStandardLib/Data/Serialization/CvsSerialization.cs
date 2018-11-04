using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FzLib.Data.Serialization
{
    public static class CvsSerialization
    {
        public static void ExportToCsvByClassProperties<T>(IEnumerable<T> objs, IEnumerable<string> propertyNames, string path)
        {
            ExportToCsvByClassProperties(objs, propertyNames, path, Encoding.UTF8);
        }
        public static void ExportToCsvByClassProperties<T>(IEnumerable<T> objs, IEnumerable<string> propertyNames, string path, Encoding encoding)
        {
            Type type = typeof(T);
            ExportToCsvByClassProperties(objs, type.GetProperties().Where(p => propertyNames.Contains(p.Name)), path, encoding);

        }
        public static void ExportToCsvByClassProperties<T>(IEnumerable<T> objs, string path)
        {
            ExportToCsvByClassProperties(objs, path, Encoding.UTF8);
        }
        public static void ExportToCsvByClassProperties<T>(IEnumerable<T> objs, string path, Encoding encoding)
        {
            Type type = typeof(T);
            ExportToCsvByClassProperties(objs, type.GetRuntimeProperties(), path, encoding);

        }
        public static void ExportToCsvByClassProperties<T>(IEnumerable<T> objs, IEnumerable<PropertyInfo> properties, string path)
        {
            ExportToCsvByClassProperties(objs, properties, path, Encoding.UTF8);

        }
        public static void ExportToCsvByClassProperties<T>(IEnumerable<T> objs, IEnumerable<PropertyInfo> properties, string path, Encoding encoding)
        {
            if (!File.Exists(path))
            {
                using (File.Create(path)) { }
            }
            //List<string> title = properties.Select(p => p.Name).ToList();

            List<dynamic> list = new List<dynamic>();

            foreach (var obj in objs)
            {
                dynamic target = new ExpandoObject();
                foreach (var prop in properties)
                {
                    object value = prop.GetValue(obj);
                    (target as IDictionary<string, object>).Add(prop.Name, value);
                }
                list.Add(target);
            }



            using (StreamWriter stream = new StreamWriter(File.Open(path, FileMode.Create), encoding))
            {
                CsvHelper.CsvWriter writer = new CsvHelper.CsvWriter(stream);
                writer.WriteRecords(list);
            }
        }
        public static void ExportToCsvByStringList(IEnumerable<IEnumerable<string>> stringLists, IEnumerable<string> header, string path)
        {
            ExportToCsvByStringList(stringLists, header, path, Encoding.UTF8);
        }
        public static void ExportToCsvByStringList(IEnumerable<IEnumerable<string>> stringLists, IEnumerable<string> header, string path, Encoding encoding)
        {
            if (!File.Exists(path))
            {
                using (File.Create(path)) { }
            }


            int count = stringLists.First().Count();
            if (stringLists.Any(p => p.Count() != count))
            {
                throw new Exception("长度不一致");
            }
            if (header.Count() != count)
            {
                throw new Exception("长度不一致");
            }
            string[] headerList = header.ToArray();

            List<dynamic> list = new List<dynamic>();

            foreach (var stringList in stringLists)
            {
                dynamic target = new ExpandoObject();
                int index = 0;
                foreach (var str in stringList)
                {
                    (target as IDictionary<string, object>).Add(headerList[index++], str);
                }
                list.Add(target);
            }



            using (StreamWriter stream = new StreamWriter(File.Open(path, FileMode.Create), encoding))
            {
                CsvHelper.CsvWriter writer = new CsvHelper.CsvWriter(stream);
                writer.WriteRecords(list);
            }
        }
        public static void ExportToCsv<T>(IEnumerable<T> objs, string path)
        {

            ExportToCsv(objs, path, Encoding.UTF8);
        }
        public static void ExportToCsv<T>(IEnumerable<T> objs, string path, Encoding encoding)
        {

            using (StreamWriter stream = new StreamWriter(File.Open(path, FileMode.Create), encoding))
            {
                CsvHelper.CsvWriter writer = new CsvHelper.CsvWriter(stream);
                writer.WriteRecords(objs);
            }
        }

        public static T[] ImportFromCsv<T>(string path)
        {
            return ImportFromCsv<T>(path, Encoding.UTF8);
        }

        public static T[] ImportFromCsv<T>(string path, Encoding encoding)
        {
            T[] result = null;
            using (StreamReader stream = new StreamReader(File.Open(path, FileMode.Open), encoding))
            {
                CsvHelper.CsvReader reader = new CsvHelper.CsvReader(stream);
                result = reader.GetRecords<T>().ToArray();
            }
            return result;
        }

        public static string StringListToString(IEnumerable<string> list,string separator)
        {
            bool first = true;
            StringBuilder result = new StringBuilder();
            foreach (var str in list)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    result.Append(separator);
                }
                result.Append(str);
            }
            return result.ToString();
        }

    }
}
