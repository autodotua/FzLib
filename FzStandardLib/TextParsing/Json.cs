using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace FzLib.TextParsing
{
    public class Json
    {
        public static string GetJson(object value,bool format)
        {
            return JsonConvert.SerializeObject(value,format? Formatting.Indented: Formatting.None);
        }
      
        public static T GetObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
        public static object GetObject(string json)
        {
            return JsonConvert.DeserializeObject(json);
        }
    }
}
