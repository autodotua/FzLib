using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FzLib.Basic
{
    public static class Number
    {
        public static string ByteToFitString(long size, int decimalDigits = 2,string B="B",string KB="KB",string MB="MB",string GB="GB",string TB="TB")
        {
            if (size < 0)
            {
                return "";
            }
            double dSize = size;
            string format = "N" + decimalDigits.ToString();
            if (dSize < 1024)
            {
                return dSize.ToString() + B;
            }
            dSize /= 1024;
            if (dSize < 1024)
            {
                return dSize.ToString(format) +KB;
            }
            dSize /= 1024;
            if (dSize < 1024)
            {
                return dSize.ToString(format) + MB;
            }
            dSize /= 1024;
            if (dSize < 1024)
            {
                return dSize.ToString(format) + GB;
            }
            dSize /= 1024;
            return dSize.ToString(format) + TB;
        }

        public static string SecondToFitString(long seconds, bool week = false, string secondUnit = "秒", string minuteUnit = "分", string hourUnit = "小时", string dayUnit = "天", string weekUnit = "周")
        {
            string result = "";
            if (seconds < 0)
            {
                seconds = -seconds;
                result += "-";
            }
            const long secondPerWeek = 3600 * 24 * 7;
            const long secondPerDay = 3600 * 24;
            const long secondPerHour = 3600;
            const long secondPerMinute = 60;

            long per = 0;
            if (seconds >= secondPerWeek)
            {
                if (week)
                {
                    per = seconds / secondPerWeek;
                    result += per.ToString() + weekUnit;
                    seconds %= secondPerWeek;
                }
            }

            if (seconds >= secondPerDay)
            {
                per = seconds / secondPerDay;
                result += per.ToString() + dayUnit;
                seconds %= secondPerDay;
            }

            if (seconds >= secondPerHour)
            {
                per = seconds / secondPerHour;
                result += per.ToString() + hourUnit;
                seconds %= secondPerHour;
            }

            if (seconds >= secondPerMinute)
            {
                per = seconds / secondPerMinute;
                result += per.ToString() + minuteUnit;
                seconds %= secondPerMinute;
            }

            if (seconds > 0)
            {
                result += seconds + secondUnit;
            }

            return result;
        }
    }
}
