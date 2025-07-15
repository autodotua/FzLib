using System;

namespace FzLib
{
    public static class NumberConverter
    {
        public static string ByteToFitString(long size, int decimalDigits = 2)
        {
            string[] defaultUnits = { "B", "KB", "MB", "GB", "TB" };
            return ByteToFitString(size, defaultUnits, decimalDigits);
        }

        public static string ByteToFitString(long size, string[] units, int decimalDigits = 2)
        {
            if (size < 0 || units == null || units.Length == 0)
            {
                return "";
            }

            double dSize = size;
            int unitIndex = 0;

            while (dSize >= 1024 && unitIndex < units.Length - 1)
            {
                dSize /= 1024;
                unitIndex++;
            }

            string format = unitIndex == 0 ? "0" : $"N{decimalDigits}";
            return dSize.ToString(format) + units[unitIndex];
        }

        public static string MeterToFitString(double lengthInMeter, int decimalDigits = 2)
        {
            string[] defaultUnits = { "毫米", "厘米", "米", "千米" };
            double[] scales = { 1000, 100, 1, 0.001 };
            return ScaleToFitString(lengthInMeter, defaultUnits, scales, decimalDigits);
        }

        public static string MeterToFitString(double lengthInMeter, string[] units, double[] scales, int decimalDigits = 2)
        {
            return ScaleToFitString(lengthInMeter, units, scales, decimalDigits);
        }

        public static string SecondToFitString(long seconds, int decimalDigits = 0)
        {
            string[] defaultUnits = { "秒", "分", "小时", "天", "周" };
            long[] scales = { 60, 60, 24, 7 };
            return TimeToFitString(seconds, defaultUnits, scales, decimalDigits);
        }

        public static string SecondToFitString(long seconds, string[] units, long[] scales, int decimalDigits = 0)
        {
            return TimeToFitString(seconds, units, scales, decimalDigits);
        }

        public static string SquareMeterToFitString(double area, int decimalDigits = 2)
        {
            string[] defaultUnits = { "平方毫米", "平方厘米", "平方米", "公顷", "平方千米" };
            double[] scales = { 1000000, 10000, 1, 0.0001, 0.000001 };
            return ScaleToFitString(area, defaultUnits, scales, decimalDigits);
        }

        public static string SquareMeterToFitString(double area, string[] units, double[] scales, int decimalDigits = 2)
        {
            return ScaleToFitString(area, units, scales, decimalDigits);
        }

        private static string ScaleToFitString(double value, string[] units, double[] scales, int decimalDigits)
        {
            if (value < 0 || units == null || scales == null || units.Length != scales.Length + 1)
            {
                throw new ArgumentException("参数无效");
            }

            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                throw new ArgumentException("数值无效");
            }

            if (value == 0)
            {
                return 0.ToString("N" + decimalDigits) + units[units.Length / 2]; // 默认返回中间单位
            }

            for (int i = 0; i < scales.Length; i++)
            {
                if (value < scales[i])
                {
                    value *= (i == 0 ? 1 : 1 / scales[i - 1]);
                    return value.ToString("N" + decimalDigits) + units[i];
                }
            }

            value /= scales[scales.Length - 1];
            return value.ToString("N" + decimalDigits) + units[units.Length - 1];
        }

        private static string TimeToFitString(long seconds, string[] units, long[] scales, int decimalDigits)
        {
            if (seconds < 0 || units == null || scales == null || units.Length != scales.Length + 1)
            {
                throw new ArgumentException("参数无效");
            }

            string result = "";
            if (seconds < 0)
            {
                result += "-";
                seconds = -seconds;
            }

            for (int i = scales.Length - 1; i >= 0; i--)
            {
                if (seconds >= scales[i])
                {
                    long value = seconds / scales[i];
                    result += value.ToString() + units[i + 1];
                    seconds %= scales[i];
                }
            }

            if (seconds > 0 || result == "")
            {
                result += seconds.ToString() + units[0];
            }

            return result;
        }
    }
}