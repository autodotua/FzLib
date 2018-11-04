using FzLib.Data;
using FzLib.Data.Serialization;
using FzLib.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
           var a = FzLib.IO.FileProperty.Photo.Exif.Get(@"C:\Users\autod\Desktop\新建文件夹 (2)\DSC08518(Lr).JPG");


            Console.Read();
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
