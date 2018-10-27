using FzLib.Data;
using FzLib.Data.Serialization;
using System;
using System.Collections.Generic;
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

            var a = FzLib.IO.FileProperty.Photo.Exif.Get(@"C:\Users\autod\Desktop\测试\DSC07346(Lr).JPG");


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
