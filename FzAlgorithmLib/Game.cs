using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FzLib.Algorithm
{
    /// <summary>
    /// 博弈
    /// </summary>
    public static class Game
    {
        public enum Winner
        {
            FirstEmpty,
            LastEmpty
        }
        /// <summary>
        /// 八什博弈
        /// </summary>
        /// <param name="count"></param>
        /// <param name="each"></param>
        /// <param name="winner"></param>
        public static void BashGame(int count, int each, Winner winner = Winner.FirstEmpty)
        {
            /*
           一：取光为胜时
           ①当 n = (m + 1) r， 先者取 k (k <= m)，后者再取 m + 1 - k 个，保持先者取时是 m + 1 的倍数，则后者必胜 → n % (m + 1) == 0 则后者必胜。

           ②当 n = (m + 1) r + s，先者首先要取掉 s 个， 这样留给后者(对手)的个数为 m + 1 的倍数，后者无论取多少，
           先者只要保持取掉一定个数使得轮到后者的时候还剩 m + 1 的倍数个，这样最后先者必胜 → n % (m + 1) != 0 则先者必胜。

           结论：保持对手取的时候数量为 m + 1 的倍数则必胜。

           二：取光为负时
           ①当 n = (m + 1) r + 1，先者取 k(k <= m)，后者取掉一定数量使得轮到先者时数量为： (m + 1)的倍数 + 1
            这样最后一轮后者无论取掉多少，先者必定能取剩 1个- -，当然先者取光咯，所以后者必胜 → (n - 1) % (m + 1) == 0 则后者必胜。

           ②当 n = (m + 1) r + s，(s != 1)，先者首先要取掉 s - 1 个，这样留给后者(对手)数量为 (m + 1) r + 1 ，后者取 k (k <= m)，
           轮到先者的只会是 (m + 1) r 或者  (m + 1) (r - 1) + 2，这样始终取掉一定数量使得留给后者(对手)的数量为： (m + 1)的倍数 + 1
           这样最后一轮时，先者肯定取剩 1 个- -，当然后者取光咯，所以先者必胜 → (n - 1) % (m + 1) != 0 则先者必胜。

           ③当 n = (m + 1) r，先者取 m - 1 个，则又变成②了。

           结论：保持对手取的时候数量为 (m + 1)的倍数 + 1 则必胜。
           */
            int rawCount = count;
            int first = count % (each + 1);
            if (winner == Winner.FirstEmpty)
            {
                if (first == 0)//后者必胜
                {
                    StringBuilder display = new StringBuilder();
                    display.Append("先手\t后手\t剩余" + Environment.NewLine);
                    Print(display);
                    while (count > 0)
                    {
                        int current;
                        while (!(int.TryParse(Console.ReadLine(), out current)))
                            ;
                        display.Append($"{current}\t{each + 1 - current}\t{count -= (each + 1)}{Environment.NewLine}");
                        Print(display);
                    }
                    Console.WriteLine($"此时{rawCount} mod ({each}+1)=0，因此后手必胜。");
                }
                else//先者必胜
                {
                    StringBuilder display = new StringBuilder();
                    display.Append("后手\t先手\t剩余" + Environment.NewLine);
                    display.Append($"0\t{ first}\t{ count -= first}{Environment.NewLine}");
                    Print(display);
                    while (count > 0)
                    {
                        int current;
                        while (!(int.TryParse(Console.ReadLine(), out current)))
                            ;
                        display.Append($"{current}\t{each + 1 - current}\t{count -= (each + 1)}{Environment.NewLine}");
                        Print(display);
                    }
                    Console.WriteLine($"此时{rawCount} mod ({each}+1)≠0，因此先手必胜。");
                }
            }
            else
            {
                if (first == 1)
                {
                    StringBuilder display = new StringBuilder();
                    display.Append("先手\t后手\t剩余" + Environment.NewLine);
                    Print(display);
                    while (count > 1)
                    {
                        int current;
                        while (!(int.TryParse(Console.ReadLine(), out current)))
                            ;
                        display.Append($"{current}\t{each + 1 - current}\t{count -= (each + 1)}{Environment.NewLine}");
                        Print(display);
                    }
                    Console.WriteLine("1\t0\t0");
                    Console.WriteLine($"此时{rawCount} mod ({each}+1)=1，因此后手必胜。");

                }
                else
                {
                    StringBuilder display = new StringBuilder();
                    display.Append("后手\t先手\t剩余" + Environment.NewLine);
                    display.Append($"0\t{((first == 0) ? (each) : (first - 1))}\t{ count -= ((first == 0) ? (each) : (first - 1))}{Environment.NewLine}");
                    Print(display);
                    while (count > each)
                    {
                        int current;
                        while (!(int.TryParse(Console.ReadLine(), out current)))
                            ;
                        display.Append($"{current}\t{each + 1 - current}\t{count -= (each + 1)}{Environment.NewLine}");
                        Print(display);
                    }
                    Console.WriteLine($"{count}\t0\t0");
                    Console.WriteLine($"此时{rawCount} mod ({each}+1)≠1，因此先手必胜。");

                }
            }

        }
        /// <summary>
        /// 威佐夫博奕
        /// </summary>
        /// <param name="heap1"></param>
        /// <param name="heap2"></param>
        public static void WythoffGame(int heap1, int heap2)
        {
            int max = heap1 > heap2 ? heap1 : heap2;
            int min = heap1 + heap2 - max;
            int k = max - min;
            if (Math.Floor(k * ((1 + Math.Sqrt(5)) / 2)) == min)
            {
                Console.WriteLine("后手必赢。");
            }
            else
            {
                Console.WriteLine("先手必赢。");
            }
            k++;
            while (--k >= 0)
            {
                Console.WriteLine($"({Math.Floor(k * ((1 + Math.Sqrt(5)) / 2))},{k + Math.Floor(k * ((1 + Math.Sqrt(5)) / 2))})");
            }
        }
        /// <summary>
        /// 尼姆博奕
        /// </summary>
        /// <param name="heap"></param>
        public static void NimmGame(int[] heap)
        {
            int sum = 0;
            foreach (var i in heap)
            {
                sum ^= i;
            }
            if (sum == 0)
            {
                Console.WriteLine("后手必赢。");
            }
            else
            {
                Console.WriteLine("先手必赢。");

            }
        }

        private static void Print(StringBuilder str, bool clearAll = true)
        {
            if (clearAll)
            {
                Console.Clear();
            }
            Console.Write(str);
        }
    }
}