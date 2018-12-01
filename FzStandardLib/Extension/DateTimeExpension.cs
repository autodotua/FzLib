using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace FzLib.Extension
{
  public  class DateTimeExtension
    {
        public static DateTime GetAverageDateTime(IEnumerable<DateTime> dateTimes)
        {
            BigInteger totalTicks = new BigInteger(0);
            int count = 0;
            foreach (var time in dateTimes)
            {
                count++;
                totalTicks+= time.Ticks;
            }
            return new DateTime((long)(totalTicks / count));
        }
    }
}
