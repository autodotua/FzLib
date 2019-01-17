using FzLib.Basic.Collection;
using FzLib.Extension;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using TaskScheduler;
using static FzLib.Basic.Collection.Loop;
using static FzLib.Basic.Math;

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

            // var lat = 29;
            // var lng = 121;
            // var level = 8;
            // var (tileX, tileY) = FzLib.Geography.Coordinate.Convert.TileNumber.GeoPointToTile(lat, lng, level);
            //
            // Console.WriteLine($"http://webrd01.is.autonavi.com/appmaptile?lang=zh_cn&size=1&scale=1&style=7&x={tileX}&y={tileY}&z={level}");

            //新建任务



            //       foreach (var item in points.Split(new string[] {Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            //       {
            //           var strs = item.Split(new string[] { " ","\t" }, StringSplitOptions.RemoveEmptyEntries);
            //           Console.WriteLine(FzLib.Geography.Coordinate.Convert.GeoCoordConverter.BD09ToGCJ02(new FzLib.Geography.Coordinate.GeoPoint(double.Parse(strs[1]), double.Parse(strs[0]))).ToString().Replace("(", "").Replace(")", ""));
            //       }
            //  var a=     FzLib.Geography.Coordinate.Convert.GeoCoordConverter.BD09ToGCJ02(new FzLib.Geography.Coordinate.GeoPoint(29.896044, 121.557295));


            //for (int large = 2; large <100; large++)
            //{
            //    Console.WriteLine("=={0}==",large);
            //    for (int small = 1; small < large/2; small++)
            //    {
            //        try
            //        {
            //            Console.WriteLine(small + " - " + ExtendedEuclid(large, small));
            //        }
            //        catch
            //        {

            //        }
            //    }
            //    Console.WriteLine();
            //}
            //Stopwatch sw = new Stopwatch();
            //sw.Start();
            //FzLib.Basic.Math.FastRandom random = new FastRandom();
            //Loop.ForRange(100000000, i => random.GetUInt64());
            //sw.Stop();
            //Console.WriteLine(sw.Elapsed);

            //Random r = new Random();
            //sw.Restart();
            //for (int i = 0; i < 100000000; i++)
            //{
            //    r.Next();
            //}
            //sw.Stop();
            //Console.WriteLine(sw.Elapsed);
            //Jk(new List<int>() { 1, 1, 1, 0, 1, 0 }, new List<int>() { 0, 0, 1, 0, 1, 1 }).ForEach(p=>Console.WriteLine(p));
            //Stopwatch sw = new Stopwatch();
            //int count = 10000;
            //Random r = new Random();
            //HashSet<int> ints = new HashSet<int>(count+10);
            //sw.Start();
            //int a = 0;
            //Loop.ForRange(count, p => ints.Add(r.Next()));
            //sw.Stop();
            //Console.WriteLine("生成用时：" + sw.Elapsed);


            //BinarySortTree<int> tree = new BinarySortTree<int>();
            //sw.Restart();
            //ints.ForEach(p => tree.Add(p));
            //sw.Stop();
            //Console.WriteLine("插入BinarySortTree用时：" + sw.Elapsed);
            //sw.Restart();
            //tree.ToArray();
            //sw.Stop();
            //Console.WriteLine("遍历BinarySortTree用时：" + sw.Elapsed);

            //SortedDictionary<int, int> dic = new SortedDictionary<int, int>();
            //sw.Restart();
            //ints.ForEach(p => dic.Add(p,p));
            //sw.Stop();
            //Console.WriteLine("插入SortedDictionary用时：" + sw.Elapsed);
            //sw.Restart();
            //dic.ToArray();
            //sw.Stop();
            //Console.WriteLine("SortedDictionary：" + sw.Elapsed);

            //SortedSet<int> set = new SortedSet<int>();
            //sw.Restart();
            //ints.ForEach(p => set.Add(p));
            //sw.Stop();
            //Console.WriteLine("插入SortedSet用时：" + sw.Elapsed);
            //sw.Restart();
            //set.ToArray();
            //sw.Stop();
            //Console.WriteLine("SortedSet：" + sw.Elapsed);


            //RedBlackTree<int> rbt = new RedBlackTree<int>();
            //sw.Restart();
            //ints.ForEach(p => rbt.Add(p));
            //sw.Stop();
            //Console.WriteLine("插入RedBlackTree用时：" + sw.Elapsed);
            //sw.Restart();
            //set.ToArray();
            //sw.Stop();
            //Console.WriteLine("遍历RedBlackTree用时：" + sw.Elapsed);



            //AutoSortList<int> list = new AutoSortList<int>(p=>p);
            //sw.Restart();
            //ints.ForEach(p => list.Add(p));
            //sw.Stop();
            //Console.WriteLine("插入列表用时：" + sw.Elapsed);

            //sw.Restart();
            //ints.OrderBy(p=>p);
            //sw.Stop();
            //Console.WriteLine("排序用时：" + sw.Elapsed);

            //for (var num = BigInteger.Pow(10, 20); num < BigInteger.Pow(10, 50) + 1000 ;num++)
            //{
            //    int yes = 0;
            //    int no = 0;
            //    for(int i=0;i<100;i++)
            //    {
            //        if(IsProbablePrime(num))
            //        {
            //            yes++;
            //        }
            //        else
            //        {
            //            no++;
            //        }
            //    }
            //    if(yes!=100 && no!=100)
            //    Console.WriteLine(num + "    " + yes + "    " + no);
            //}

            //Stopwatch sw = new Stopwatch();
            //sw.Start();
            //Random r = new Random();
            //ForRange(10000,p=>
            //FzLib.Basic.Math.DecomposeFacter(r.Next()).ForEach(q => Console.WriteLine(q)));
            //Stopwatch sw = new Stopwatch();
            //sw.Start();
            //DecomposeFacter(156164561566).ForEach(q => Console.WriteLine(q));
            //sw.Stop();
            //Console.WriteLine(sw.Elapsed);

            //ExtendedDictionary<int, int> a = new ExtendedDictionary<int, int>();
            //A[] As = new A[] { new A(), new A() };
            //JObject json = new JObject();
            //json.Add("array",JArray.FromObject(As));

            //IEnumerable<A> array = json.GetValue("array").Select(p => p.ToObject<A>());

            string path = @"C:\Users\autod\OneDrive\同步";
            int index = 0;
            Stopwatch sw = new Stopwatch();
            var files = Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories).ToArray();
            sw.Start();
            foreach (var file in files)
            {
                var a = Exists(file);
            }
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
         

        Console.Read();

        }
        [DllImport("Shlwapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private extern static bool PathFileExists(StringBuilder path);

        static bool Exists(string path)
        {
            // A StringBuilder is required for interops calls that use strings
            StringBuilder builder = new StringBuilder();
            builder.Append(path);
            return  PathFileExists(builder);
        }

        public class A
        {
            public string B { get; set; } = "dsd";
            public int C { get; set; } = 4567;
        }

        //private static IList<int> Jk(IList<int> a, IList<int> b)
        //{
        //    List<int> c = new List<int>() { a[0] };
        //    Loop.ForRange(1, Math.Min(a.Count, b.Count), i => c.Add(((a[i] + b[i] + 1) * c[i - 1] + a[i]) % 2));
        //    return c;
        //}

        private void CreatTask(string author, string desc, string name, string path)
        {
            TaskSchedulerClass scheduler = new TaskSchedulerClass();
            //连接
            scheduler.Connect(null, null, null, null);
            //获取创建任务的目录
            ITaskFolder folder = scheduler.GetFolder("\\");
            //设置参数
            ITaskDefinition task = scheduler.NewTask(0);
            task.RegistrationInfo.Author = author;//创建者
            task.RegistrationInfo.Description = desc;//描述
                                                     //设置触发机制（此处是 登陆后）
            task.Triggers.Create(_TASK_TRIGGER_TYPE2.TASK_TRIGGER_LOGON);
            //设置动作（此处为运行exe程序）
            IExecAction action = (IExecAction)task.Actions.Create(_TASK_ACTION_TYPE.TASK_ACTION_EXEC);
            action.Path = path;//设置文件目录
            task.Settings.ExecutionTimeLimit = "PT0S"; //运行任务时间超时停止任务吗? PTOS 不开启超时
            task.Settings.DisallowStartIfOnBatteries = false;//只有在交流电源下才执行
            task.Settings.RunOnlyIfIdle = false;//仅当计算机空闲下才执行

            IRegisteredTask regTask =
                folder.RegisterTaskDefinition(name, task,//此处需要设置任务的名称（name）
                (int)_TASK_CREATION.TASK_CREATE, null, //user
                null, // password
                _TASK_LOGON_TYPE.TASK_LOGON_INTERACTIVE_TOKEN,
                "");
            IRunningTask runTask = regTask.Run(null);
        }
    }



}
