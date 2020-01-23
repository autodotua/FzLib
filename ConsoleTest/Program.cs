using FzLib.Algorithm;
using FzLib.Algorithm.DataStructure.Tree;
using FzLib.Basic;
using FzLib.Basic.Collection;
using FzLib.Extension;
using FzLib.Geography.Analysis;
using FzLib.Geography.IO.Gpx;
using NetTopologySuite.Geometries;
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
using static FzLib.Basic.Loop;
using FzLib.Basic.Math;
using System.Threading.Tasks;
using FzLib.DataStorage.SQLite;

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

            //string path = @"C:\Users\autod\OneDrive\同步";
            //int index = 0;
            //Stopwatch sw = new Stopwatch();
            //var files = Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories).ToArray();
            //sw.Start();
            //foreach (var file in files)
            //{
            //    var a = Exists(file);
            //}
            //sw.Stop();
            //Console.WriteLine(sw.Elapsed);

            //BinarySearchTree<int> tree = new BinarySearchTree<int>();
            //tree.Add(1);
            //tree.Add(4);
            //tree.Add(6);
            //tree.Add(2);
            //tree.Add(756);
            //tree.Add(324);
            //tree.Add(24);
            //tree.Add(5);
            //tree.Add(1048);
            //tree.Add(2048);



            ////Console.WriteLine(tree.ToString(BinaryTreeBase<int, BinarySearchTree<int>.Node>.OrderType.InOrder, " "));

            //Console.WriteLine(tree.ToVisualString());
            //Console.WriteLine(tree.ToString());
            //tree.Remove(tree.Search(6));
            //tree.Remove(tree.Search(756));
            //tree.Remove(tree.Search(1));
            //Console.WriteLine(tree.ToVisualString());
            //Console.WriteLine(tree.ToString());

            //Random r = new Random();
            //for (int j = 0; j < 100; j++)
            //{
            //    RedBlackTree<int> tree = new RedBlackTree<int>();
            //    for (int i = 0; i < 10; i++)
            //    {
            //        int num = r.Next(100);
            //        Console.Write(num + " ");
            //        tree.Add(num);
            //    }
            //    Console.WriteLine();
            //    Console.WriteLine(tree.ToString());
            //    Console.WriteLine();
            //}

            //Random r = new Random();
            //while (true)
            //{
            //    List<int> array = new List<int>();
            //    Loop.ForRange(50, p => array.Add(r.Next(100)));
            //    var results = new SortResult<int>[]
            //    {
            //    Sort.BubbleSort(array),
            //    Sort.InsertSort(array),
            //    Sort.BinaryInsertionSort(array),
            //    Sort.ShellSort(array),
            //    Sort.MergeSortWithoutRecursiveTimes(array),
            //    Sort.MergeSortWithRecursiveTimes(array),
            //    Sort.HeapSort(array),
            //    };
            //    Loop.ForRange(results[0].Result.Length, p =>
            //    {
            //        if (results.Any(q => q.Result[p] != results[0].Result[p]))
            //        {
            //            foreach (var item in results)
            //            {
            //                Console.WriteLine(string.Join(" ", item.Result));
            //            }
            //        }
            //    });
            //    Console.WriteLine("OK");
            //}
            //Random r = new Random();
            //List<int> array = new List<int>();
            //ForRange(10000, p => array.Add(r.Next(10000)));
            //ForRange(10000, p => array.Add(r.Next(60000, 100000)));
            //ForRange(10000, p => array.Add(r.Next(10000, 20000)));
            //ForRange(3000, p => array.Add(p));
            //Stopwatch sw = new Stopwatch();
            //sw.Start();
            //var a = array.Select(p => -p).ToArray();
            //sw.Stop();
            //Console.WriteLine("Linq" + "   " + sw.ElapsedMilliseconds);
            //sw.Reset();
            //Func<IList<int>, SortResult<int>>[] methods = new Func<IList<int>, SortResult<int>>[]
            //{
            //    Sort.BubbleSort,
            //    Sort.SelectionSort,
            //    Sort.InsertSort,
            //    Sort.BinaryInsertionSort,
            //    Sort.ShellSort,
            //    Sort.MergeSortWithoutRecursiveTimes,
            //    Sort.MergeSortWithRecursiveTimes,
            //    Sort.HeapSort,
            //    Sort.QuickSort,
            //    Sort.QuickSort2,
            //    Sort.QuickSort3,
            //};
            //foreach (var item in methods)
            //{
            //    sw.Start();
            //    item(array);
            //    sw.Stop();
            //    Console.WriteLine(item.Method.Name + "   " + sw.ElapsedMilliseconds);
            //    sw.Reset();
            //}


            //Gpx gpx = Gpx.FromFile(@"C:\Users\admin\Desktop\植物园轨迹\20191025_154822.gpx");
            //var a = gpx.Tracks.First().MaxSpeed;

            //FzLib.Cryptography.Hash h = new FzLib.Cryptography.Hash();
            //Console.WriteLine(h.GetString("md5", "admin"));



            //List<string> exps = new List<string>();
            //string exp = "sin(x)+cos(y)*log(x,y)^y";
            //for (double x = 1; x < 10; x += 0.1235)
            //{
            //    for (double y = 1; y < 10; y += (System.Math.PI / System.Math.E) / 3)
            //    {
            //        exps.Add(exp.Replace("x", x.ToString()).Replace("y", y.ToString()));
            //    }
            //}

            //Calculation c = new Calculation();
            //var sw = Stopwatch.StartNew();
            //foreach (var e in exps)
            //{

            //    c.Calc(e);
            //}

            //sw.Stop();
            //Console.WriteLine(sw.ElapsedMilliseconds * 1.0 / exps.Count);
            //Console.ReadLine();

            //Calculation c = new Calculation();
            //string exp = null;
            //do
            //{
            //    try
            //    {
            //        exp = Console.ReadLine();
            //        var result = c.Calc(exp);
            //        result.Steps.ForEach(p => Console.WriteLine(p));
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex.ToString());
            //    }
            //    Console.WriteLine();

            //} while (!string.IsNullOrEmpty(exp));

            //decimal pi = 3.141592653589793238462643383279502884197169399375105820974944592307816406286m;
            //decimal a90 = 90 * pi / 180;
            //Console.WriteLine(DecimalMath.Tan(90));

            //Console.ReadLine();

            SQLiteDatabaseHelper db = SQLiteDatabaseHelper.OpenOrCreate("test.db");
            db.CreateTable("d"+Guid.NewGuid().ToString().Replace("-",""), "id", 
                new SQLiteColumn("test", SQLiteDataType.Integer),
                new SQLiteColumn("test2", SQLiteDataType.Text,true),
                new SQLiteColumn("test3", SQLiteDataType.Text,true,"hello"),
                new SQLiteColumn("test4", SQLiteDataType.Text,true)
                
                );
        }


    }

    /// <summary>
    /// Analogy of Syatem.Math class for decimal types 
    /// </summary>
    public static class DecimalMath
    {
        /// <summary>
        /// represents PI
        /// </summary>
        public const decimal Pi = 3.14159265358979323846264338327950288419716939937510M;

        /// <summary>
        /// represents PI
        /// </summary>
        public const decimal Epsilon = 0.0000000000000000001M;

        /// <summary>
        /// represents 2*PI
        /// </summary>
        private const decimal PIx2 = 6.28318530717958647692528676655900576839433879875021M;

        /// <summary>
        /// represents E
        /// </summary>
        public const decimal E = 2.7182818284590452353602874713526624977572470936999595749M;

        /// <summary>
        /// represents PI/2
        /// </summary>
        private const decimal PIdiv2 = 1.570796326794896619231321691639751442098584699687552910487M;

        /// <summary>
        /// represents PI/4
        /// </summary>
        private const decimal PIdiv4 = 0.785398163397448309615660845819875721049292349843776455243M;

        /// <summary>
        /// represents 1.0/E
        /// </summary>
        private const decimal Einv = 0.3678794411714423215955237701614608674458111310317678M;

        /// <summary>
        /// log(10,E) factor
        /// </summary>
        private const decimal Log10Inv = 0.434294481903251827651128918916605082294397005803666566114M;

        /// <summary>
        /// Zero
        /// </summary>
        public const decimal Zero = 0.0M;

        /// <summary>
        /// One
        /// </summary>
        public const decimal One = 1.0M;

        /// <summary>
        /// Represents 0.5M
        /// </summary>
        private const decimal Half = 0.5M;

        /// <summary>
        /// Max iterations count in Taylor series
        /// </summary>
        private const int MaxIteration = 100;

        /// <summary>
        /// Analogy of Math.Exp method
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static decimal Exp(decimal x)
        {
            var count = 0;
            while (x > One)
            {
                x--;
                count++;
            }
            while (x < Zero)
            {
                x++;
                count--;
            }
            var iteration = 1;
            var result = One;
            var fatorial = One;
            decimal cachedResult;
            do
            {
                cachedResult = result;
                fatorial *= x / iteration++;
                result += fatorial;
            } while (cachedResult != result);
            if (count != 0) result = result * PowerN(E, count);
            return result;
        }

        /// <summary>
        /// Analogy of Math.Pow method
        /// </summary>
        /// <param name="value"></param>
        /// <param name="pow"></param>
        /// <returns></returns>
        public static decimal Power(decimal value, decimal pow)
        {
            if (pow == Zero) return One;
            if (pow == One) return value;
            if (value == One) return One;

            if (value == Zero && pow == Zero) return One;

            if (value == Zero)
            {
                if (pow > Zero)
                {
                    return Zero;
                }

                throw new Exception("Invalid Operation: zero base and negative power");
            }

            if (pow == -One) return One / value;

            var isPowerInteger = IsInteger(pow);
            if (value < Zero && !isPowerInteger)
            {
                throw new Exception("Invalid Operation: negative base and non-integer power");
            }

            if (isPowerInteger && value > Zero)
            {
                int powerInt = (int)(pow);
                return PowerN(value, powerInt);
            }

            if (isPowerInteger && value < Zero)
            {
                int powerInt = (int)pow;
                if (powerInt % 2 == 0)
                {
                    return Exp(pow * Log(-value));
                }
                else
                {
                    return -Exp(pow * Log(-value));
                }
            }

            return Exp(pow * Log(value));
        }

        private static bool IsInteger(decimal value)
        {
            long longValue = (long)value;
            if (Abs(value - longValue) <= Epsilon)
            {
                return true;
            }

            return false;
        }
        /// <summary>
        /// Power to the integer value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="power"></param>
        /// <returns></returns>
        public static decimal PowerN(decimal value, int power)
        {
            if (power == Zero) return One;
            if (power < Zero) return PowerN(One / value, -power);

            var q = power;
            var prod = One;
            var current = value;
            while (q > 0)
            {
                if (q % 2 == 1)
                {
                    // detects the 1s in the binary expression of power
                    prod = current * prod; // picks up the relevant power
                    q--;
                }
                current *= current; // value^i -> value^(2*i)
                q = q / 2;
            }

            return prod;
        }

        /// <summary>
        /// Analogy of Math.Log10
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static decimal Log10(decimal x)
        {
            return Log(x) * Log10Inv;
        }

        /// <summary>
        /// Analogy of Math.Log
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static decimal Log(decimal x)
        {
            if (x <= Zero)
            {
                throw new ArgumentException("x must be greater than zero");
            }
            var count = 0;
            while (x >= One)
            {
                x *= Einv;
                count++;
            }
            while (x <= Einv)
            {
                x *= E;
                count--;
            }
            x--;
            if (x == 0) return count;
            var result = Zero;
            var iteration = 0;
            var y = One;
            var cacheResult = result - One;
            while (cacheResult != result && iteration < MaxIteration)
            {
                iteration++;
                cacheResult = result;
                y *= -x;
                result += y / iteration;
            }
            return count - result;
        }

        /// <summary>
        /// Analogy of Math.Cos
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static decimal Cos(decimal x)
        {
            while (x > PIx2)
            {
                x -= PIx2;
            }
            while (x < -PIx2)
            {
                x += PIx2;
            }
            // now x in (-2pi,2pi)
            if (x >= Pi && x <= PIx2)
            {
                return -Cos(x - Pi);
            }
            if (x >= -PIx2 && x <= -Pi)
            {
                return -Cos(x + Pi);
            }
            x = x * x;
            //y=1-x/2!+x^2/4!-x^3/6!...
            var xx = -x * Half;
            var y = One + xx;
            var cachedY = y - One;//init cache  with different value
            for (var i = 1; cachedY != y && i < MaxIteration; i++)
            {
                cachedY = y;
                decimal factor = i * (i + i + 3) + 1; //2i^2+2i+i+1=2i^2+3i+1
                factor = -Half / factor;
                xx *= x * factor;
                y += xx;
            }
            return y;
        }

        /// <summary>
        /// Analogy of Math.Tan
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static decimal Tan(decimal x)
        {
            var cos = Cos(x);
            if (cos == Zero) throw new ArgumentException(nameof(x));
            return Sin(x) / cos;
        }

        /// <summary>
        /// Analogy of Math.Sin
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static decimal Sin(decimal x)
        {
            var cos = Cos(x);
            var moduleOfSin = Sqrt(One - (cos * cos));
            var sineIsPositive = IsSignOfSinePositive(x);
            if (sineIsPositive) return moduleOfSin;
            return -moduleOfSin;
        }

        private static bool IsSignOfSinePositive(decimal x)
        {
            //truncating to  [-2*PI;2*PI]
            while (x >= PIx2)
            {
                x -= PIx2;
            }

            while (x <= -PIx2)
            {
                x += PIx2;
            }

            //now x in [-2*PI;2*PI]
            if (x >= -PIx2 && x <= -Pi) return true;
            if (x >= -Pi && x <= Zero) return false;
            if (x >= Zero && x <= Pi) return true;
            if (x >= Pi && x <= PIx2) return false;

            //will not be reached
            throw new ArgumentException(nameof(x));
        }

        /// <summary>
        /// Analogy of Math.Sqrt
        /// </summary>
        /// <param name="x"></param>
        /// <param name="epsilon">lasts iteration while error less than this epsilon</param>
        /// <returns></returns>
        public static decimal Sqrt(decimal x, decimal epsilon = Zero)
        {
            if (x < Zero) throw new OverflowException("Cannot calculate square root from a negative number");
            //initial approximation
            decimal current = (decimal)global::System.Math.Sqrt((double)x), previous;
            do
            {
                previous = current;
                if (previous == Zero) return Zero;
                current = (previous + x / previous) * Half;
            } while (Abs(previous - current) > epsilon);
            return current;
        }
        /// <summary>
        /// Analogy of Math.Sinh
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static decimal Sinh(decimal x)
        {
            var y = Exp(x);
            var yy = One / y;
            return (y - yy) * Half;
        }

        /// <summary>
        /// Analogy of Math.Cosh
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static decimal Cosh(decimal x)
        {
            var y = Exp(x);
            var yy = One / y;
            return (y + yy) * Half;
        }

        /// <summary>
        /// Analogy of Math.Sign
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static int Sign(decimal x)
        {
            return x < Zero ? -1 : (x > Zero ? 1 : 0);
        }

        /// <summary>
        /// Analogy of Math.Tanh
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static decimal Tanh(decimal x)
        {
            var y = Exp(x);
            var yy = One / y;
            return (y - yy) / (y + yy);
        }

        /// <summary>
        /// Analogy of Math.Abs
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static decimal Abs(decimal x)
        {
            if (x <= Zero)
            {
                return -x;
            }
            return x;
        }

        /// <summary>
        /// Analogy of Math.Asin
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static decimal Asin(decimal x)
        {
            if (x > One || x < -One)
            {
                throw new ArgumentException("x must be in [-1,1]");
            }
            //known values
            if (x == Zero) return Zero;
            if (x == One) return PIdiv2;
            //asin function is odd function
            if (x < Zero) return -Asin(-x);

            //my optimize trick here

            // used a math formula to speed up :
            // asin(x)=0.5*(pi/2-asin(1-2*x*x)) 
            // if x>=0 is true

            var newX = One - 2 * x * x;

            //for calculating new value near to zero than current
            //because we gain more speed with values near to zero
            if (Abs(x) > Abs(newX))
            {
                var t = Asin(newX);
                return Half * (PIdiv2 - t);
            }
            var y = Zero;
            var result = x;
            decimal cachedResult;
            var i = 1;
            y += result;
            var xx = x * x;
            do
            {
                cachedResult = result;
                result *= xx * (One - Half / (i));
                y += result / (2 * i + 1);
                i++;
            } while (cachedResult != result);
            return y;
        }

        /// <summary>
        /// Analogy of Math.Atan
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static decimal ATan(decimal x)
        {
            if (x == Zero) return Zero;
            if (x == One) return PIdiv4;
            return Asin(x / Sqrt(One + x * x));
        }
        /// <summary>
        /// Analogy of Math.Acos
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static decimal Acos(decimal x)
        {
            if (x == Zero) return PIdiv2;
            if (x == One) return Zero;
            if (x < Zero) return Pi - Acos(-x);
            return PIdiv2 - Asin(x);
        }

        /// <summary>
        /// Analogy of Math.Atan2
        /// for more see this
        /// <seealso cref="http://i.imgur.com/TRLjs8R.png"/>
        /// </summary>
        /// <param name="y"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static decimal Atan2(decimal y, decimal x)
        {
            if (x > Zero)
            {
                return ATan(y / x);
            }
            if (x < Zero && y >= Zero)
            {
                return ATan(y / x) + Pi;
            }
            if (x < Zero && y < Zero)
            {
                return ATan(y / x) - Pi;
            }
            if (x == Zero && y > Zero)
            {
                return PIdiv2;
            }
            if (x == Zero && y < Zero)
            {
                return -PIdiv2;
            }
            throw new ArgumentException("invalid atan2 arguments");
        }
    }
}
