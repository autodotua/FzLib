using FzLib.Basic.Collection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using static System.Math;

namespace FzLib.Basic
{
    public static class Math
    {
        public static long ExtendedEuclid(long large, long small, bool ensurePositive = true)
        {
            if (large <= small)
            {
                throw new Exception("大的数小于或等于小的数");
            }
            long x1 = 1;
            long y2 = 1;
            long x2 = 0;
            long y1 = 0;
            long x3 = large;
            long y3 = small;
            while (true)
            {
                if (y3 == 0)
                {
                    throw new Exception("两个数不是互素的");
                    // *result = x3; /* 两个数不互素则result为两个数的最大公约数，此时返回值为零 */
                    // return 0;
                }
                if (y3 == 1)
                {
                    if (ensurePositive && y2 < 0)
                    {
                        y2 = large + y2 % large;
                    }
                    return y2;
                }
                long q = x3 / y3;
                long t1 = x1 - q * y1;
                long t2 = x2 - q * y2;
                long t3 = x3 - q * y3;
                x1 = y1;
                x2 = y2;
                x3 = y3;
                y1 = t1;
                y2 = t2;
                y3 = t3;
            }
        }
        public static long GetCommonDivisor(long num1, long num2)
        {
            if (num1 < 0)
            {
                num1 = -num1;
            }
            if (num2 < 0)
            {
                num2 = -num2;
            }
            //辗转相除法
            long remainder = 0;
            while (num1 % num2 > 0)
            {
                remainder = num1 % num2;
                num1 = num2;
                num2 = remainder;
            }
            return remainder;
        }
        public static bool AreMutualPrime(long num1, long num2)
        {
            return GetCommonDivisor(num1, num2) == 1;
        }
        public static bool IsPrime(long n)
        {
            bool b = true;
            if (n == 2)
            {
                return true;
            }
            long sqr = Convert.ToInt64(System.Math.Sqrt(n));
            for (long i = sqr; i > 2; i--)
            {
                if (n % i == 0)
                {
                    b = false;
                }
            }

            return b;
        }
        public static IEnumerable<BigInteger> GetProbablePrimes(BigInteger from,BigInteger to)
        {
            for(BigInteger i =from;i<=to;i++)
            {
                if (IsProbablePrime(i))
                {
                    yield return i;
                }
            }
        }
        public static bool IsProbablePrime(BigInteger num)
        {
            long certainty = 2;
            if (num == 2 || num == 3)
            {
                return true;
            }
            if (num < 2 || num % 2 == 0)
            {
                return false;
            }

            BigInteger d = num - 1;
            long s = 0;

            while (d % 2 == 0)
            {
                d /= 2;
                s += 1;
            }

            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            byte[] bytes = new byte[num.ToByteArray().LongLength];
            BigInteger a;

            for (long i = 0; i < certainty; i++)
            {
                do
                {
                    rng.GetBytes(bytes);
                    a = new BigInteger(bytes);
                }
                while (a < 2 || a >= num - 2);

                BigInteger x = BigInteger.ModPow(a, d, num);
                if (x == 1 || x == num - 1)
                {
                    continue;
                }

                for (long r = 1; r < s; r++)
                {
                    x = BigInteger.ModPow(x, 2, num);
                    if (x == 1)
                    {
                        return false;
                    }
                    if (x == num - 1)
                    {
                        break;
                    }
                }

                if (x != num - 1)
                {
                    return false;
                }
            }

            return true;
        }

        public static (long Factor,int Index)[] DecomposeFacter(long num)
        {
            ExtendedDictionary<long, int> result = new ExtendedDictionary<long, int>();
            long sqrtNum = (long)Sqrt(num);
            for (long i = 2; i <= sqrtNum; i++)
            {
                while (num >= i)
                {
                    if (num % i == 0)//能被整除
                    {
                        result[i]++;
                        num /= i;//模仿短除法
                        sqrtNum = (long)Sqrt(num);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            if (num != 1)
            {
                result[num]++;
            }
            return result.Select(p => (p.Key, p.Value)).ToArray();

        }

        public class FastRandom
        {
            public FastRandom(uint state)
            {
                CurrentUInt32State  = state;
                CurrentUInt64State = state;
            }
            public FastRandom(ulong state)
            {
                CurrentUInt32State = (uint)(state % uint.MaxValue);
                CurrentUInt64State = state;
            }
            public FastRandom()
            {
                Random r = new Random();
                CurrentUInt32State =(uint) r.Next();
                CurrentUInt64State = (uint)r.Next();
            }
            public uint CurrentUInt32State { get; private set; }
            public ulong CurrentUInt64State { get; private set; }
            public uint GetUInt32()
            {
                /* Algorithm "xor" from p. 4 of Marsaglia, "Xorshift RNGs" */
                uint x = CurrentUInt32State;
                x ^= x << 13;
                x ^= x >> 17;
                x ^= x << 5;
                CurrentUInt32State = x;
                return x;
            }

            public ulong GetUInt64()
            {
                ulong x = CurrentUInt64State;
                x ^= x << 13;
                x ^= x >> 7;
                x ^= x << 17;
                CurrentUInt64State = x;
                return x;
            }

        }
    }
}
