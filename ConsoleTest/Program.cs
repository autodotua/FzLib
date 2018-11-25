using FzLib.Data;
using FzLib.Data.Serialization;
using FzLib.Geography.Format;
using FzLib.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //   Config config = FzLib.Data.JsonDataBase.OpenOrCreat<Config>();
            //   config.Save();
            //Config config = Config.OpenOrCreat<Config>();
            //config.Save();
            //    Stopwatch watch = new Stopwatch();
            //    watch.Start();
            //  var a=  FileSystem.CompareFiles(@"C:\Users\autod\OneDrive", @"E:\备份\20181101-121029\C\Users\autod\OneDrive");
            //    watch.Stop();
            //    Console.WriteLine(watch.ElapsedMilliseconds);

            GpxInfo info =  GpxInfo.FromString(File.ReadAllText(@"E:\旧事重提\多媒体\家庭\2017暑假\20170827~0901日本旅行\第三天1：清水寺\轨迹.gpx"));
            info.Tracks[0].GetOffsetPoints(540,540);
        }
    }
    [XmlInclude(typeof(Config))]
  public  class Config: JsonSerializationBase
    {
        public string A { get; set; } = "sadsa";
        public StringSplitOptions S { get; set; } = StringSplitOptions.RemoveEmptyEntries;
     public   DateTime Date { get; set; } = DateTime.Today;
    }
}
