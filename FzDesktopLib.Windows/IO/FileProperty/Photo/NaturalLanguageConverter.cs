using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace FzLib.IO.FileProperty.Photo
{
    public static class NaturalLanguageConverter
    {
        //https://www.sno.phy.queensu.ca/~phil/exiftool/TagNames/EXIF.html
        static NaturalLanguageConverter()
        {
            Converters = new Dictionary<int, Func<object, string>>()
            {
                {306, converterDateTime },
                {36867, converterDateTime },
                {36868, converterDateTime },
                {20531, converterDateTime },
                {274, converterOrientation },
                {41992,converterNormalLowHigh },
                {41993,converterNormalLowHigh },
                {41994,converterNormalLowHigh },
                {41990,converterSceneCaptureType },
                {41987,converterAutoManual },
                {41986,converterExposureMode },
                {41985,converterCustomRendered },
                {37385,converterFlash },
                {37384,converterLightSource },
                {37383,converterMeteringMode },
                {34864,converterSensitivityType },
                {34850,converterExposureProgram },
                {33437,converterF },
                {33434,converterExposureTime },
                {282,converterResolution },
                {283,converterResolution },
                {37390,converterResolution },
                {37391,converterResolution },
                {262 ,converterPhotometricInterpretation },
                {50970 ,converterColorSpace },
                {40961 ,converterColorSpace },
                {41989 ,converterFocalLength },
                {37386 ,converterFocalLength },
            };
        }
        public static readonly Func<object, string> converterFocalLength = new Func<object, string>(o =>
           {
               return ((o is ushort) ? o.ToString() : ((URational)o).DecimalValue.ToString()) + "mm";
           });
        public static readonly Func<object, string> converterDateTime = new Func<object, string>(o =>
           {
               string value = o as string;
               int[] parts = value.Split(':', ' ').Select(p => int.Parse(p)).ToArray();
               DateTime time = new DateTime(parts[0], parts[1], parts[2], parts[3], parts[4], parts[5]);
               return time.ToString();
           });
        public static readonly Func<object, string> converterResolution = new Func<object, string>(o =>
        {
            URational value = (URational)o;
            return value.DecimalValue.ToString();

        });
        public static readonly Func<object, string> converterF = new Func<object, string>(o =>
        {
            URational value = (URational)o;
            return "F/" + value.DecimalValue.ToString("0.0");

        });
        public static readonly Func<object, string> converterExposureTime = new Func<object, string>(o =>
                {
                    URational value = (URational)o;
                    if (value.Numerator > value.Denominator)
                    {
                        return Math.Round(1.0 * value.Numerator / value.Denominator, 2).ToString();
                    }
                    else if (value.Numerator * 5 > value.Denominator)
                    {
                        return "1/" + Math.Round(1.0 * value.Denominator / value.Numerator, 1).ToString();
                    }
                    else
                    {
                        return "1/" + Math.Round(1.0 * value.Denominator / value.Numerator).ToString();

                    }

                });
        public static readonly Func<object, string> converterColorSpace = new Func<object, string>(o =>
        {
            ushort value = (ushort)o;
            Dictionary<int, string> values = new Dictionary<int, string>()
           {

                {0x1,"sRGB"},
                {0x2,"Adobe GRB"},
                {0xfffd,"广色域RGB"},
                {0xfffe,"ICC配置文件"},
                {0xffff,"未校准"},
           };
            return values[value];
        });
        public static readonly Func<object, string> converterPhotometricInterpretation = new Func<object, string>(o =>
        {
            ushort value = (ushort)o;
            Dictionary<int, string> values = new Dictionary<int, string>()
           {

                {0,"White Is Zero"},
                {1,"Black Is Zero"},
                {2,"RGB"},
                {3,"RGB Palette"},
                {4,"Transparency Mask"},
                {5,"CMYK"},
                {6,"YCbCr"},
                {8,"CIE Lab"},
                {9,"ICC Lab"},
                {10,"ITU Lab"},
                {32803,"Color Filter Array"},
                {32844,"Pixar Log L"},
                {32845,"Pixar Log Luv"},
                {32892,"Sequential Color Filter"},
                {34892,"Linear Raw"},
           };
            return values[value];
        });
        public static readonly Func<object, string> converterLightSource = new Func<object, string>(o =>
        {
            ushort value = (ushort)o;
            Dictionary<int, string> values = new Dictionary<int, string>()
            {
                //{0, "Unknown"},
                //{ 1, "Daylight"},
                //{ 2, "Fluorescent"},
                //{ 3, "Tungsten (Incandescent)"},
                //{ 4, "Flash"},
                //{ 9, "Fine Weather"},
                //{ 10, "Cloudy"},
                //{ 11, "Shade"},
                //{ 12, "Daylight Fluorescent"},
                //{ 13, "Day White Fluorescent"},
                //{ 14, "Cool White Fluorescent"},
                //{ 15, "White Fluorescent"},
                //{ 16, "Warm White Fluorescent"},
                //{ 17, "Standard Light A"},
                //{ 18, "Standard Light B"},
                //{ 19, "Standard Light C"},
                //{ 20, "D55"},
                //{ 21, "D65"},
                //{ 22, "D75"},
                //{ 23, "D50"},
                //{ 24, "ISO Studio Tungsten"},
                //{ 255, "Other"},
                {0, "未知"},
                { 1, "日光"},
                { 2, "荧光灯"},
                { 3, "白织灯"},
                { 4, "闪光灯"},
                { 9, "好天气"},
                { 10, "多云"},
                { 11, "阴天"},
                { 12, "日光、荧光灯"},
                { 13, "白天、荧光灯"},
                { 14, "冷色荧光灯"},
                { 15, "白色荧光灯"},
                { 16, "暖色荧光灯"},
                { 17, "标准光A"},
                { 18, "标准光B"},
                { 19, "标准光V"},
                { 20, "D55"},
                { 21, "D65"},
                { 22, "D75"},
                { 23, "D50"},
                { 24, "ISO工作室白织灯"},
                { 255, "其他"},

            };
            return values[value];
        });
        public static readonly Func<object, string> converterFlash = new Func<object, string>(o =>
       {
           ushort value = (ushort)o;
           Dictionary<int, string> values = new Dictionary<int, string>()
           {
                //{0x0, "No Flash"},
                //{0x1,"Fired"},
                //{0x5,"Fired, Return not detected"},
                //{0x7,"Fired, Return detected"},
                //{0x8,"On, Did not fire"},
                //{0x9,"On, Fired"},
                //{0xd,"On, Return not detected"},
                //{0xf, "On, Return detected"},
                //{0x10,"Off, Did not fire"},
                //{0x14,"Off, Did not fire, Return not detected"},
                //{0x18,"Auto, Did not fire"},
                //{0x19,"Auto, Fired"},
                //{0x1d,"Auto, Fired, Return not detected"},
                //{0x1f, "Auto, Fired, Return detected"},
                //{0x20,"No flash function"},
                //{0x30,"Off, No flash function"},
                //{0x41,"Fired, Red-eye reduction"},
                //{0x45,"Fired, Red-eye reduction, Return not detected"},
                //{0x47,"Fired, Red-eye reduction, Return detected"},
                //{0x49,"On, Red-eye reduction"},
                //{0x4d,"On, Red-eye reduction, Return not detected"},
                //{0x4f, "On, Red-eye reduction, Return detected"},
                //{0x50,"Off, Red-eye reduction"},
                //{0x58,"Auto, Did not fire, Red-eye reduction"},
                //{0x59,"Auto, Fired, Red-eye reduction"},
                //{0x5d,"Auto, Fired, Red-eye reduction, Return not detected"},
                //{0x5f, "Auto, Fired, Red-eye reduction, Return detected"   }
                {0x0, "无"},
                {0x1,"闪光"},
                {0x5,"闪光, 未检测到返回"},
                {0x7,"闪光, 检测到返回"},
                {0x8,"开, 未闪光"},
                {0x9,"开, 闪光"},
                {0xd,"开, 未检测到返回"},
                {0xf, "开, 检测到返回"},
                {0x10,"关, 未闪光"},
                {0x14,"关, 未闪光, 未检测到返回"},
                {0x18,"自动, 未闪光"},
                {0x19,"自动, 闪光"},
                {0x1d,"自动, 闪光, 未检测到返回"},
                {0x1f, "自动, 闪光, 检测到返回"},
                {0x20,"无闪光灯"},
                {0x30,"关, 无闪光灯"},
                {0x41,"闪光, 红眼"},
                {0x45,"闪光, 红眼, 未检测到返回"},
                {0x47,"闪光, 红眼, 检测到返回"},
                {0x49,"开, 红眼"},
                {0x4d,"开, 红眼, 未检测到返回"},
                {0x4f, "开, 红眼, 检测到返回"},
                {0x50,"关, 红眼"},
                {0x58,"自动, 未闪光, 红眼"},
                {0x59,"自动, 闪光, 红眼"},
                {0x5d,"自动, 闪光, 红眼, 未检测到返回"},
                {0x5f, "自动, 闪光, 红眼, 检测到返回"   }
           };
           return values[value];
       });
        public static readonly Func<object, string> converterNormalLowHigh = new Func<object, string>(o =>
        {
            ushort value = (ushort)o;
            switch (value)
            {
                case 0:
                    return "正常";
                case 1:
                    return "低";
                case 2:
                    return "高";
                default:
                    throw new ArgumentException("旋转值超出范围");
            }
        });
        public static readonly Func<object, string> converterExposureProgram = new Func<object, string>(o =>
        {
            ushort value = (ushort)o;
            switch (value)
            {
                case 0:
                    return "未定义";
                case 1:
                    return "手动";
                case 2:
                    return "程序";
                case 3:
                    return "光圈优先";
                case 4:
                    return "快门优先";
                case 5:
                    return "低速";
                case 6:
                    return "高速";
                case 7:
                    return "肖像";
                case 8:
                    return "风景";
                case 9:
                    return "灯光";
                default:
                    throw new ArgumentException("旋转值超出范围");
            }
        });
        public static readonly Func<object, string> converterSensitivityType = new Func<object, string>(o =>
        {
            ushort value = (ushort)o;
            switch (value)
            {
                case 0:
                    return "未知";
                case 1:
                    return "标准";
                case 2:
                    return "推荐";
                case 3:
                    return "ISO";
                case 4:
                    return "标准、推荐";
                case 5:
                    return "标准、ISO";
                case 6:
                    return "推荐、ISO";
                case 7:
                    return "标准、推荐、ISO";
                default:
                    throw new ArgumentException("旋转值超出范围");
            }
        });
        public static readonly Func<object, string> converterMeteringMode = new Func<object, string>(o =>
        {
            ushort value = (ushort)o;
            switch (value)
            {
                case 0:
                    return "未知";
                case 1:
                    return "平均";
                case 2:
                    return "中心";
                case 3:
                    return "点";
                case 4:
                    return "多点";
                case 5:
                    return "多段";
                case 6:
                    return "局部";
                case 255:
                    return "其他";
                default:
                    throw new ArgumentException("旋转值超出范围");
            }
        });
        public static readonly Func<object, string> converterCustomRendered = new Func<object, string>(o =>
        {
            ushort value = (ushort)o;
            switch (value)
            {
                case 0:
                    return "正常";
                case 1:
                    return "自定义";
                case 3:
                    return "HDR";
                case 6:
                    return "全景";
                case 8:
                    return "人像";
                default:
                    throw new ArgumentException("旋转值超出范围");
            }
        });
        public static readonly Func<object, string> converterAutoManual = new Func<object, string>(o =>
        {
            ushort value = (ushort)o;
            switch (value)
            {
                case 0:
                    return "自动";
                case 1:
                    return "手动";
                default:
                    throw new ArgumentException("旋转值超出范围");
            }
        });
        public static readonly Func<object, string> converterSceneCaptureType = new Func<object, string>(o =>
        {
            ushort value = (ushort)o;
            switch (value)
            {
                case 0:
                    return "标准";
                case 1:
                    return "风景";
                case 2:
                    return "人像";
                case 3:
                    return "夜景";
                default:
                    throw new ArgumentException("值超出范围");
            }
        });
        public static readonly Func<object, string> converterExposureMode = new Func<object, string>(o =>
        {
            ushort value = (ushort)o;
            switch (value)
            {
                case 0:
                    return "自动";
                case 1:
                    return "手动";
                case 2:
                    return "自动等级";
                default:
                    throw new ArgumentException("旋转值超出范围");
            }
        });
        public static readonly Func<object, string> converterOrientation = new Func<object, string>(o =>
        {
            ushort value = (ushort)o;
            switch (value)
            {
                case 1:
                    return "无旋转";
                case 2:
                    return "水平翻转";
                case 3:
                    return "180°";
                case 4:
                    return "垂直翻转";
                case 5:
                    return "顺时针90°水平翻转";
                case 6:
                    return "顺时针90°翻转";
                case 7:
                    return "顺时针90°翻转垂直翻转";
                case 8:
                    return "逆时针90°翻转";

                default:
                    throw new ArgumentException("旋转值超出范围");

            }
        });
        public static Dictionary<int, Func<object, string>> Converters { get; set; }
        public static string TryConvertValue(ExifItem item)
        {
            if (!Converters.ContainsKey(item.Id))
            {
                return null;
            }
            try
            {
                return ConvertValue(item);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static string ConvertValue(ExifItem item)
        {
            if (Converters.ContainsKey(item.Id))
            {
                return Converters[item.Id](item.Value);
            }
            throw new Exception("不存在ID为" + item.Id + "的转换器");
        }
    }
}
