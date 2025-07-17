using FzLib.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using static System.Math;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

using M = System.Math;

namespace FzLib.Numeric
{
    public static class NumberTheory
    {
        private static RandomNumberGenerator randomGenerator = RandomNumberGenerator.Create();

        public static bool AreMutualPrime(long num1, long num2)
        {
            return GetGreatestCommonDivisor(num1, num2) == 1;
        }

        public static IReadOnlyDictionary<long, int> DecomposeFactor(long num)
        {
            if (num <= 1)
            {
                return new ReadOnlyDictionary<long, int>(new Dictionary<long, int>());
            }
            Dictionary<long, int> result = new Dictionary<long, int>();
            long sqrtNum = (long)Sqrt(num);
            for (long i = 2; i <= sqrtNum; i++)
            {
                while (num >= i)
                {
                    if (num % i == 0)//能被整除
                    {
                        result[i] = (result.ContainsKey(i) ? result[i] : 0) + 1;
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
                result[num] = (result.ContainsKey(num) ? result[num] : 0) + 1;
            }
            return new ReadOnlyDictionary<long, int>(result);
        }

        public static long GetGreatestCommonDivisor(long num1, long num2)
        {
            if (num1 == 0 && num2 == 0)
            {
                throw new ArgumentException("两个数不能同时为零");
            }
            if (num2 == 0)
            {
                return num1;
            }
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

        public static long GetModularInverse(long modulus, long number, bool ensurePositive = true)
        {
            if (modulus <= number)
            {
                throw new ArgumentException("模数必须大于被除数", nameof(modulus));
            }
            long x1 = 1;
            long y2 = 1;
            long x2 = 0;
            long y1 = 0;
            long x3 = modulus;
            long y3 = number;
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
                        y2 = modulus + y2 % modulus;
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

        public static IEnumerable<BigInteger> GetProbablePrimes(BigInteger from, BigInteger to)
        {
            for (BigInteger i = from; i <= to; i++)
            {
                if (IsProbablePrime(i))
                {
                    yield return i;
                }
            }
        }

        public static bool IsPrime(long n)
        {
            if (n <= 1)
            {
                return false;
            }
            if (n == 2)
            {
                return true;
            }
            if (n % 2 == 0)
            {
                return false;
            }
            long sqr = Convert.ToInt64(Sqrt(n));
            for (long i = 3; i <= sqr; i += 2)
            {
                if (n % i == 0)
                {
                    return false;
                }
            }
            return true;
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

            byte[] bytes = new byte[num.ToByteArray().LongLength];
            BigInteger a;

            for (long i = 0; i < certainty; i++)
            {
                do
                {
                    randomGenerator.GetBytes(bytes);
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

    }

}