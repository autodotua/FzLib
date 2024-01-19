using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace FzLib
{
    public static class DateTimeExtension
    {
        public static DateTime GetAverageDateTime(this IEnumerable<DateTime> dateTimes)
        {
            BigInteger totalTicks = new BigInteger(0);
            int count = 0;
            foreach (var time in dateTimes)
            {
                count++;
                totalTicks += time.Ticks;
            }
            return new DateTime((long)(totalTicks / count));
        }

        public static DateTime GetAverageDateTime(params DateTime[] dateTimes)
        {
            return GetAverageDateTime(dateTimes as IEnumerable<DateTime>);
        }
    }
}