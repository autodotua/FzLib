using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Drawing;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Threading;

namespace FzLib.Algorithm.Search
{
    public class MazePoint
    {
        #region 坐标系
        /*
        _____________________________y
        |
        |
        |
        |
        |
        |
        |
        |
        |x

            */

        #endregion


        /// <summary>
        /// X坐标
        /// </summary>
        public int X;
        /// <summary>
        /// Y坐标
        /// </summary>
        public int Y;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public MazePoint(int x, int y)
        {
            X = x;
            Y = y;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="neighborDirection">相邻的方向</param>
        public MazePoint(int x, int y, int neighborDirection)
        {
            X = x;
            Y = y;
            NeighborDirection = neighborDirection;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="p"></param>
        /// <param name="includeWall"></param>
        public MazePoint(Point p)
        {

            X = p.X;
            Y = p.Y;


        }
        /// <summary>
        /// 转换到Point类型
        /// </summary>
        /// <returns></returns>
        public Point ToGraphPoint()
        {
            return new Point(X,Y);
        }
        /// <summary>
        /// 左边的一个
        /// </summary>
        public MazePoint LeftPoint { get => new MazePoint(X - 1, Y); }
        /// <summary>
        /// 右边的一个
        /// </summary>
        public MazePoint RightPoint { get => new MazePoint(X + 1, Y); }
        /// <summary>
        /// 上面的一个
        /// </summary>
        public MazePoint UpPoint { get => new MazePoint(X, Y - 1); }
        /// <summary>
        /// 下面的一个
        /// </summary>
        public MazePoint DownPoint { get => new MazePoint(X, Y + 1); }
        /// <summary>
        /// 左边的第二个
        /// </summary>
        public MazePoint Left2Point { get => new MazePoint(X - 2, Y); }
        /// <summary>
        /// 右边的第二个
        /// </summary>
        public MazePoint Right2Point { get => new MazePoint(X + 2, Y); }
        /// <summary>
        /// 上面的第二个
        /// </summary>
        public MazePoint Up2Point { get => new MazePoint(X, Y - 2); }
        /// <summary>
        /// 下面的第二个
        /// </summary>
        public MazePoint Down2Point { get => new MazePoint(X, Y + 2); }
        /// <summary>
        /// 根据数字来获取身边的一个
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public MazePoint GetSurroundingPoint(int direction)
        {
            switch (direction)
            {
                case 0:
                    return LeftPoint;
                case 1:
                    return RightPoint;
                case 2:
                    return UpPoint;
                case 3:
                    return DownPoint;
                default:
                    return null;
            }
        }
        /// <summary>
        /// 相邻的方向
        /// </summary>
        public int NeighborDirection { get; set; }
        /// <summary>
        /// 用于回溯的上一个点
        /// </summary>
        public MazePoint LastPoint { get; set; }
    }
    /// <summary>
    /// 复杂地图的点类型
    /// </summary>
    public enum PointType
    {
        StartPoint = 0x01,
        EndPoint = 0x02,
        Wall = 0x04,
        Blank = 0x08,
        Way = 0x10,
    }
    /// <summary>
    /// 迷宫基类
    /// </summary>
    public class MazeBase
    {
        public bool IsLegal(Point p)
        {
            return ExteriorWall ? (p.X % 2 + p.Y % 2 == 2) : (p.X % 2 + p.Y % 2 == 0);
        }
        /// <summary>
        /// 是否在迷宫范围内
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool IsLegal(Size size)
        {
            return size.Width % 2 + size.Height % 2 == 2;
        }
        /// <summary>
        /// 是否在迷宫范围内
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        protected bool InRange(int x, int y)
        {
            return x >= 0 && x < Width && y > 0 && y < Height;
        }
        /// <summary>
        /// 是否在迷宫范围内
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        protected bool InRange(MazePoint p)
        {
            return p.X >= 0 && p.X < Width && p.Y >= 0 && p.Y < Height;
        }
        /// <summary>
        /// 获取一个随机数
        /// </summary>
        /// <param name="from">开始</param>
        /// <param name="smallerThan">之前</param>
        /// <returns></returns>
        protected int GetRandomNumber(int from, int smallerThan)
        {
            RNGCryptoServiceProvider r = new RNGCryptoServiceProvider();
            byte[] b = new byte[8];
            r.GetBytes(b);
            return (b[0]+ b[1]+ b[2]+ b[3]+ b[4] + b[5] + b[6] + b[7]) % (smallerThan - from) + from;
        }
        /// <summary>
        /// 根据
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        protected int[,] visited;
        /// <summary>
        /// 地图本身
        /// </summary>
        protected bool[,] map;
        /// <summary>
        /// 地图
        /// </summary>
        public bool[,] Map { get => map; set => map = value; }
        /// <summary>
        /// 复杂地图，用于Solve
        /// </summary>
        public PointType[,] ComplexMap { get; set; }
        /// <summary>
        /// 根据无墙地图坐标获取有墙地图坐标
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        protected MazePoint GetWall(MazePoint p1, MazePoint p2)
        {
            return new MazePoint(p1.X + p2.X + 1, p1.Y + p2.Y + 1);
        }
        /// <summary>
        /// 是否在生成地图时显示过程
        /// </summary>
        public bool PrintWhenGenerating { get => printWhenGenerating; set => printWhenGenerating = value; }
        /// <summary>
        /// 地图宽度，在不同场景下意义有区别
        /// </summary>
        public int Width;
        /// <summary>
        /// 地图高度，在不同场景下意义有区别
        /// </summary>
        public int Height;
        /// <summary>
        /// 用于在生成地图时显示过程时标记当前点
        /// </summary>
        public MazePoint currentPoint;
        /// <summary>
        /// 是否在生成地图时显示过程
        /// </summary>
        private bool printWhenGenerating = false;
        /// <summary>
        /// 是否有外墙
        /// </summary>
        protected bool ExteriorWall { get; set; }
        /// <summary>
        /// 用于演示的速度，每一次的休眠时间，毫秒
        /// </summary>
        public int SleepWhenPrintingWhenProcessing = 0;
        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="processing"></param>
        public void PrintMap(bool processing = false)
        {
            if (processing && !PrintWhenGenerating)
            {
                return;
            }
            Console.CursorVisible = false;
            if (processing)
            {
                if (SleepWhenPrintingWhenProcessing != 0)
                {
                    Thread.Sleep(SleepWhenPrintingWhenProcessing);
                }
                Console.CursorLeft = 0;
                Console.CursorTop = 0;
            }
            StringBuilder str = new StringBuilder();
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (currentPoint != null && currentPoint.X == i && currentPoint.Y == j)
                    {
                        str.Append("〇");
                    }
                    else if (map[i, j])
                    {
                        //Console.ForegroundColor = ConsoleColor.Red;
                        str.Append("█");
                    }
                    else if (visited?[i, j] == 2)
                    {
                        str.Append(" .");
                    }
                    else
                    {
                        //Console.ForegroundColor = ConsoleColor.White;
                        str.Append("  ");
                    }
                    //Console.Write($"█");
                }
                str.Append(Environment.NewLine);

            }
            Console.WriteLine(str.ToString());

        }
        /// <summary>
        /// 打印复杂地图，用于Solve时
        /// </summary>
        /// <param name="processing"></param>
        public void PrintComplexMap(bool processing = false)
        {
            Console.CursorVisible = false;
            if (processing)
            {
                if (SleepWhenPrintingWhenProcessing != 0)
                {
                    Thread.Sleep(SleepWhenPrintingWhenProcessing);
                }
                Console.CursorLeft = 0;
                Console.CursorTop = 0;
            }
            StringBuilder str = new StringBuilder();
            for (int i = 0; i < ComplexMap.GetLength(0); i++)
            {
                for (int j = 0; j < ComplexMap.GetLength(1); j++)
                {
                    if (currentPoint != null && currentPoint.X == i && currentPoint.Y == j)
                    {
                        str.Append("●");
                    }
                    else if (ComplexMap[i, j] == PointType.StartPoint)
                    {
                        //Console.ForegroundColor = ConsoleColor.Red;
                        str.Append("▲");
                    }
                    else if (ComplexMap[i, j] == PointType.EndPoint)
                    {
                        //Console.ForegroundColor = ConsoleColor.Red;
                        str.Append("★");
                    }
                    else if (ComplexMap[i, j] == PointType.Wall)
                    {
                        //Console.ForegroundColor = ConsoleColor.Red;
                        str.Append("█");
                    }
                    else if (ComplexMap[i, j] == PointType.Way)
                    {
                        //Console.ForegroundColor = ConsoleColor.Red;
                        str.Append("∷");
                    }
                    else if (visited[i, j] == 1)
                    {
                        str.Append(" .");
                    }
                    else
                    {
                        //Console.ForegroundColor = ConsoleColor.White;
                        str.Append("  ");
                    }
                    //Console.Write($"█");
                }
                str.Append(Environment.NewLine);

            }
            Console.WriteLine(str.ToString());

        }

    }
    public class GenrateMazeWithDepthFirst : MazeBase
    {
        private new bool InRange(MazePoint p)
        {
            if (ExteriorWall)
            {
                return p.X > 0 && p.X < Width - 1 && p.Y > 0 && p.Y < Height - 1;
            }
            return p.X >= 0 && p.X < Width && p.Y >= 0 && p.Y < Height;
        }
        /// <summary>
        /// 是否四周都是被标记过的
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private bool Bolcked(MazePoint p)
        {
            for (int i = 0; i < 4; i++)
            {
                MazePoint next = p.GetSurroundingPoint(i).GetSurroundingPoint(i);
                //MazePoint wallPoint = getWall(p, next);
                if (/*!Walls[wallPoint.X,wallPoint.Y] &&*/
        InRange(next) && HasVisited(next) == 0)
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 获取下一个随机方向的点
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private MazePoint GetNext(MazePoint p)
        {
            MazePoint next = new MazePoint(p.X, p.Y);
            int r = GetRandomNumber(0, 4);
            switch (r)
            {
                case 0:
                    return p.Left2Point;
                case 1:
                    return p.Right2Point;
                case 2:
                    return p.Up2Point;
                case 3:
                    return p.Down2Point;
            }
            return null;
        }
        /// <summary>
        /// 是否被标记过
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private int HasVisited(MazePoint p)
        {
            return visited[p.X, p.Y];
        }
        /// <summary>
        /// 设置标记状态
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private void SetVisited(MazePoint p)
        {
            visited[p.X, p.Y]++;
        }
        /// <summary>
        /// 设置两个点中间的墙的访问性，用于可视化
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        private void SetVisited(MazePoint p1, MazePoint p2)
        {
            MazePoint p = GetCenterPoint(p1, p2);
            visited[p.X, p.Y]++;
        }
        /// <summary>
        /// 获得两个点中间的墙的坐标
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        private MazePoint GetCenterPoint(MazePoint p1, MazePoint p2)
        {
            return new MazePoint((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
        }
        /// <summary>
        /// 产生一个迷宫
        /// </summary>
        /// <param name="size"></param>
        /// <param name="startMazePoint"></param>
        public void Genrate(Size size, Point? startMazePoint = null, bool exteriorWall = true)
        {
            ExteriorWall = exteriorWall;
            if (!IsLegal(size))
            {
                throw new Exception("长度必须为奇数！");
            }
            Width = size.Width;
            Height = size.Height;
            visited = new int[Width, Height];
            int count = 0;
            map = new bool[Width, Height];
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j] = exteriorWall ? (i % 2 == 0 || j % 2 == 0) : (i % 2 == 1 || j % 2 == 1);
                }
            }


            MazePoint current;
            if (startMazePoint == null)
            {

                current = new MazePoint(1,1);
            }
            else
            {
                if (!IsLegal((Point)startMazePoint))
                {
                    throw new Exception("起点坐标应在有外墙时为奇数，无外墙时为偶数！");
                }
                current = new MazePoint((Point)startMazePoint);
            }



            SetVisited(current);
            Stack<MazePoint> s = new Stack<MazePoint>();
            s.Push(current);
            while (count < map.Length)
            {
                if (!Bolcked(current))
                {
                    MazePoint next;
                    do
                    {
                        next = GetNext(current);
                    } while (!InRange(next) || HasVisited(next) > 0);
                    s.Push(next);
                    MazePoint wall = GetCenterPoint(current, next);
                    Map[wall.X, wall.Y] = false;
                    SetVisited(current, next);
                    SetVisited(next);
                    current = next;
                    count++;
                }
                else if (s.Count > 0)
                {

                    current = s.Pop();
                    SetVisited(current);
                    if (s.Count > 0)
                    {
                        SetVisited(s.Peek(), current);
                    }
                }
                else
                {
                    break;
                }
                currentPoint = new MazePoint(current.X, current.Y);
                PrintMap(true);
            }
            currentPoint = null;
        }
    }


    public class GenrateMazeWithRecursiveSegmentation : MazeBase
    {
        private new bool IsLegal(Size size)
        {
            return ExteriorWall ? size.Width % 2 + size.Height % 2 == 2 : size.Width % 2 + size.Height % 2 == 0;
        }
        /// <summary>
        /// 生成迷宫地图
        /// </summary>
        /// <param name="size"></param>
        public void Genrate(Size size, bool exteriorWall=true)
        {
            ExteriorWall = exteriorWall;
            if (!IsLegal(size))
            {
                throw new Exception("长度应在有边框时为奇数，无边框时为偶数！");
            }
            map = new bool[size.Width, size.Height];
            Width = size.Width;
            Height = size.Height;
            if (exteriorWall)
            {
                for (int i = 0; i < map.GetLength(0); i++)
                {
                    for (int j = 0; j < map.GetLength(1); j++)
                    {
                        map[i, j] = i == 0 || i == Width - 1 || j == 0 || j == Height - 1;
                    }
                }
                Cut(1, 1, Height - 1, Width - 1);
            }
            else
            {
                Cut(0, 0, Height - 1, Width - 1);
            }
            //  
        }

        /// <summary>
        /// 递归切割
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="right"></param>
        /// <param name="bottom"></param>
        private void Cut(int left, int top, int right, int bottom)
        {
            PrintMap(true);

            // 分各区域的坐标和面积
            int rdHeight = bottom - top;
            int rdWidth = right - left;
            int area = (rdHeight + 1) * (rdWidth + 1);
            if (area < 10 || (rdHeight <= 1 && rdWidth <= 1))
                return;

            // 计算分割点坐标并在分割方向上补上墙
            int recursiceX = -1;
            int recursiceY = -1;
            if (rdWidth > 1)
            {
                recursiceY = left + 1 + GetRandomNumber(0, rdWidth / 2) * 2;
                for (int i = top; i < bottom; i++)
                {
                    map[i, recursiceY] = true;
                }
            }
            if (rdHeight > 1)
            {
                recursiceX = top + 1 + GetRandomNumber(0, rdHeight / 2) * 2;
                for (int i = left; i <= right; i++)
                {
                    map[recursiceX, i] = true;
                }
            }


            if (rdWidth > 1 && rdHeight > 1)
            {
                // 选择要打通的墙，确保连通性，打通三面
                // 0：上，1：下，2：左，3：右
                int side = GetRandomNumber(0, 4);
                if (side != 0)
                {
                    var upIndex = GetRandomNumber(0, (recursiceX - 1 - top) / 2 + 1) * 2;
                    map[top + upIndex, recursiceY] = false;
                }
                if (side != 1)
                {
                    var downIndex = GetRandomNumber(0, (bottom - recursiceX - 1) / 2 + 1) * 2;
                    map[recursiceX + 1 + downIndex, recursiceY] = false;
                }
                if (side != 2)
                {
                    var leftIndex = GetRandomNumber(0, (recursiceY - 1 - left) / 2 + 1) * 2;
                    map[recursiceX, left + leftIndex] = false;
                }
                if (side != 3)
                {
                    var rightIndex = GetRandomNumber(0, (right - recursiceY - 1) / 2 + 1) * 2;
                    map[recursiceX, recursiceY + 1 + rightIndex] = false;
                }
                // 递归
                Cut(left, top, recursiceY - 1, recursiceX - 1);
                Cut(recursiceY + 1, top, right, recursiceX - 1);
                Cut(left, recursiceX + 1, recursiceY - 1, bottom);
                Cut(recursiceY + 1, recursiceX + 1, right, bottom);
            }
            else
            {
                if (rdWidth <= 1)
                {
                    var index = GetRandomNumber(0, rdWidth / 2 + 1) * 2;
                    map[recursiceX, left + index] = false;
                    Cut(left, top, right, recursiceX - 1);
                    Cut(left, recursiceX + 1, right, bottom);
                }
                if (rdHeight <= 1)
                {
                    var index = GetRandomNumber(0, rdHeight / 2 + 1) * 2;
                    map[top + index, recursiceY] = false;
                    Cut(left, top, recursiceY - 1, bottom);
                    Cut(recursiceY + 1, top, right, bottom);
                }
            }
        }
    }

    public class GenrateMazeWithRandomPrim : MazeBase
    {
        /// <summary>
        /// 生成迷宫地图
        /// </summary>
        /// <param name="size"></param>
        public void Genrate(Size size, Point? startPoint = null, bool exteriorWall = true)
        {
            Width = size.Width;
            Height = size.Height;
            ExteriorWall = exteriorWall;
            if (!IsLegal(size))
            {
                throw new Exception("只接受奇数长与宽！");
            }

            //int temp = 0;
            //if (exteriorWall)
            //{
            //    temp = 2;
            //}
            map = new bool[Width, Height];
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    map[i, j] = true;
                }
            }
            if (startPoint != null)
            {
                if (!IsLegal((Point)startPoint))
                {
                    throw new Exception("起点坐标应在有外墙时为奇数，无外墙时为偶数！");
                }
                Prim(((Point)startPoint).X, ((Point)startPoint).Y);
            }
            else
            {
                Prim(Width / 2, Height / 2);
            }
            //if (border)
            //{
            //    for (int i = Width + temp - 2; i >= 0; i--)
            //    {
            //        for (int j = Height + temp - 2; j >= 0; j--)
            //        {
            //            map[i + 1, j + 1] = map[i, j];
            //        }
            //    }
            //    for (int i = 0; i < Width + temp; i++)
            //    {
            //        map[i, 0] = true;
            //    }
            //    for (int i = 0; i < Height + temp; i++)
            //    {
            //        map[0, i] = true;
            //    }
            //    for (int i = 0; i < Width + temp; i++)
            //    {
            //        map[i, Height + temp - 1] = true;
            //    }
            //    for (int i = 0; i < Height + temp; i++)
            //    {
            //        map[Width + temp - 1, 0] = true;
            //    }
            //    Width += temp;
            //    Height += temp;
            //    currentPoint = null;
            //}

        }
        /// <summary>
        /// 普利姆迷宫生成法
        /// </summary>
        /// <param name="startX">起始点X坐标</param>
        /// <param name="startY">起始点Y坐标</param>
        private void Prim(int startX, int startY)
        {
            //邻墙列表
            var blockList = new List<MazePoint>();
            //将起点作为目标格
            MazePoint current = new MazePoint(startX, startY);
            //将起点标记为通路
            map[current.X, current.Y] = false;
            //记录邻墙
            if (current.Y > 1)
            {
                blockList.Add(new MazePoint(current.X, current.Y - 1, 0));
            }
            if (current.X <= Width)
            {
                blockList.Add(new MazePoint(current.X + 1, current.Y, 1));
            }
            if (current.Y <= Height)
            {
                blockList.Add(new MazePoint(current.X, current.Y + 1, 2));
            }
            if (current.X > 1)
            {
                blockList.Add(new MazePoint(current.X - 1, current.Y, 3));
            }
            while (blockList.Count > 0)
            {
                //随机选一堵墙
                var blockIndex = GetRandomNumber(0, blockList.Count);
                //找到墙对面的墙
                switch (blockList[blockIndex].NeighborDirection)
                {
                    case 0:
                        current.X = blockList[blockIndex].X;
                        current.Y = blockList[blockIndex].Y - 1;
                        break;
                    case 1:
                        current.X = blockList[blockIndex].X + 1;
                        current.Y = blockList[blockIndex].Y;
                        break;
                    case 2:
                        current.X = blockList[blockIndex].X;
                        current.Y = blockList[blockIndex].Y + 1;
                        break;
                    case 3:
                        current.X = blockList[blockIndex].X - 1;
                        current.Y = blockList[blockIndex].Y;
                        break;

                }
                //如果目标格未连通
                if (map[current.X, current.Y])
                {
                    //联通目标格
                    map[blockList[blockIndex].X, blockList[blockIndex].Y] = false;
                    map[current.X, current.Y] = false;
                    //添加目标格相邻格
                    if (current.Y > 1 && map[current.X, current.Y - 1] && map[current.X, current.Y - 2])
                    {
                        blockList.Add(new MazePoint(current.X, current.Y - 1, 0));
                    }
                    if (current.X + 2 < Width && map[current.X + 1, current.Y] && map[current.X + 2, current.Y])
                    {
                        blockList.Add(new MazePoint(current.X + 1, current.Y, 1));
                    }
                    if (current.Y + 2 < Height && map[current.X, current.Y + 1] && map[current.X, current.Y + 2])
                    {
                        blockList.Add(new MazePoint(current.X, current.Y + 1, 2));
                    }
                    if (current.X > 1 && map[current.X - 1, current.Y] && map[current.X - 2, current.Y])
                    {
                        blockList.Add(new MazePoint(current.X - 1, current.Y, 3));
                    }
                }
                currentPoint = current;
                blockList.RemoveAt(blockIndex);
                PrintMap(true);

            }
        }
    }
    
    public class SolveMaze : MazeBase
    {
        /// <summary>
        /// 是否没有被访问过
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private bool HasNotVisited(MazePoint p)
        {
            return visited[p.X, p.Y] == 0;
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="rawMap"></param>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        private void Initialize(bool[,] rawMap, Point startPoint, Point endPoint)
        {

            Width = rawMap.GetLength(0);
            Height = rawMap.GetLength(1);
            ComplexMap = new PointType[Width, Height];
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    if (i == startPoint.X && j == startPoint.Y)
                    {
                        ComplexMap[i, j] = PointType.StartPoint;
                    }
                    else if (i == endPoint.X && j == endPoint.Y)
                    {
                        ComplexMap[i, j] = PointType.EndPoint;
                    }
                    else if (rawMap[i, j])
                    {
                        ComplexMap[i, j] = PointType.Wall;
                    }
                    else
                    {
                        ComplexMap[i, j] = PointType.Blank;
                    }
                }
            }
            visited = new int[Width, Height];
        }
        /// <summary>
        /// 深度优先方法，求出来的不一定是最短路线
        /// </summary>
        /// <param name="rawMap"></param>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        public void DepthFirstSearchMethod(bool[,] rawMap, Point startPoint, Point endPoint)
        {
            Initialize(rawMap, startPoint, endPoint);
            Stack<MazePoint> s = new Stack<MazePoint>();
            s.Push(new MazePoint(startPoint));
            visited[startPoint.X, startPoint.Y]++;
            while (s.Count > 0)
            {
                MazePoint current = s.Pop();
                currentPoint = current;
                PrintComplexMap(true);
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (Math.Abs(i + j) == 1)
                        {
                            MazePoint next = new MazePoint(current.X + i, current.Y + j) { LastPoint = current };
                            if (InRange(next)&&ComplexMap[next.X, next.Y] == PointType.EndPoint)
                            {
                                currentPoint = next;
                                PrintComplexMap(true);
                                Backtracking(startPoint, next);
                                return;
                            }
                            if (InRange(next) && HasNotVisited(next) && ComplexMap[next.X, next.Y] == PointType.Blank)
                            {
                                visited[next.X, next.Y]++;
                                s.Push(next);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 广度优先方法，求出来的是最短路径
        /// </summary>
        /// <param name="rawMap"></param>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        public void BreadthFirstSearchMethod(bool[,] rawMap, Point startPoint, Point endPoint)
        {
            Initialize(rawMap, startPoint, endPoint);
            Queue<MazePoint> q = new Queue<MazePoint>();
            q.Enqueue(new MazePoint(startPoint));
            visited[startPoint.X, startPoint.Y]++;
            while (q.Count > 0)
            {
                MazePoint current = q.Dequeue();
                currentPoint = current;
                PrintComplexMap(true);
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (Math.Abs(i + j) == 1)
                        {
                            MazePoint next = new MazePoint(current.X + i, current.Y + j) { LastPoint = current };
                            if (InRange(next) && ComplexMap[next.X, next.Y] == PointType.EndPoint)
                            {
                                currentPoint = next;
                                PrintComplexMap(true);
                                Backtracking(startPoint, next);
                                return;
                            }
                            if (InRange(next) && HasNotVisited(next) && ComplexMap[next.X, next.Y] == PointType.Blank)
                            {
                                visited[next.X, next.Y]++;
                                q.Enqueue(next);
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 回溯
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        private void Backtracking(Point startPoint, MazePoint endPoint)
        {
            Way = new List<Point>();
            MazePoint current = endPoint;
            while (current.X != startPoint.X || current.Y != startPoint.Y)
            {
                Way.Add(current.ToGraphPoint());
                current = current.LastPoint;
                ComplexMap[current.X, current.Y] = PointType.Way;
                currentPoint = current;
                PrintComplexMap(true);
            }

            Way.Add(current.ToGraphPoint());
            Way.Reverse();
            StepCount = Way.Count;
        }
        /// <summary>
        /// 步数
        /// </summary>
        public int StepCount { get; private set; }
        /// <summary>
        /// 路线
        /// </summary>
        public List<Point> Way { get; set; }
    }

}


//    map = new bool[size.Width * 2 + 1, size.Height * 2 + 1];
//    Width = size.Width * 2 + 1;
//    Height = size.Height * 2 + 1;
//    for (int i = 0; i < map.GetLength(0); i++)
//    {
//        for (int j = 0; j < map.GetLength(1); j++)
//        {
//            map[i, j] = true;
//        }
//    }

//    List<MazePoint> list = new List<MazePoint>();
//    MazePoint current = new MazePoint(startMazePoint);
//    map[current.X, current.Y] = false;
//    for(int i=0;i<4;i++)
//    {
//        var neighbor = current.GetSurroundingPoint(i);
//        neighbor.Neighbor = current;
//        list.Add(neighbor);
//    }
//    while(list.Count>0)
//    {
//        current = list[GetRandomNumber(0, list.Count)];

//        MazePoint next = CheckSurroundingPoints(current);
//        if(next!=null)
//        {
//            map[next.X, next.Y] = false;
//            list.Remove(current);
//            for (int i = 0; i < 2; i++)
//            {
//                for (int j = 0; j< 2;j++)
//                {
//                    if(Math.Abs(i+j)==1)
//                    {
//                        int x = next.X+i;
//                        int y = next.Y+j;
//                        if (InRange(x,y) && map[x, y])
//                        {
//                            var neighbor = new MazePoint(x, y);
//                            neighbor.Neighbor = current;
//                            list.Add(neighbor);
//                        }
//                    }

//                } }
//        }
//        else
//        {
//            list.Remove(current);
//        }

//    }
