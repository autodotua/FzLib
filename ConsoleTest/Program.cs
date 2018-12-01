using FzLib.Extension;
using FzLib.Geography.Analysis;
using FzLib.Geography.Format;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using static System.Math;

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

            //    GpxInfo info = GpxInfo.FromString(File.ReadAllText(@"E:\旧事重提\多媒体\家庭\大学\20180816和李凯去梅山附近\上桥.gpx"));
            //    foreach (var item in info.Tracks[0].Points)
            //    {
            //        double speed = info.Tracks[0].Points.GetSpeed(item , 3);
            //        item.Speed = speed;
            //    }
            //    var info2 = info.Clone();
            // Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(info) );
            //Console.WriteLine(info.ToGpxXml());
            //   Console.WriteLine(info.Tracks[0].GetMovingAverageSpeed(0.3)*3.6);
            //   Console.WriteLine(info.Tracks[0].GetMaxSpeed(8,1)*3.6);
            //  var speeds = SpeedAnalysis.GetSpeeds(info.Tracks[0].Points);
            //  var speeds2=      Filter.MedianValueFilter(speeds,p=>p.Speed,5,1);

            // Console.WriteLine(info.Tracks[0].GetMovingTime(0.3));
            // var a = SpeedAnalysis.GetFilteredSpeeds(info.Tracks[0].Points,10,10);
            //Shapefile.ExportEmptyPointShapefile(@"C:\Users\autod\Desktop\1121XY", "a");

            //   List<int> list = new List<int>();
            //   list.Add(5);
            //   list.Add(3);
            //   list.Add(15330);
            //   list.Add(334);
            //   list.Add(12340);
            //   list.Add(35);
            //   list.Add(210);


            //var a=   Filter.MedianValueFilter(list, p => p, 3, 1);
            //   foreach (var item in a)
            //   {
            //       Console.WriteLine(item.SelectedItem);

            //   }

            var lat = 29;
            var lng = 121;
            var level = 8;
            var (tileX, tileY) = FzLib.Geography.Coordinate.Convert.TileNumber.GeoPointToTile(lat, lng, level);

            Console.WriteLine($"http://webrd01.is.autonavi.com/appmaptile?lang=zh_cn&size=1&scale=1&style=7&x={tileX}&y={tileY}&z={level}");
            Console.Read();

        }
    }

}
