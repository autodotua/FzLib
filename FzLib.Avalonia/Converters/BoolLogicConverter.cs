using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace FzLib.Avalonia.Converters
{
    /// <summary>
    /// 例如：values={true,true,false}，parameter=or，返回true。参数支持or、and、nor。
    /// </summary>
    public class BoolLogicConverter : IMultiValueConverter
    {
        public object Convert(IList<object> values, Type targetType, object parameter, CultureInfo culture)
        {
            if ( !(parameter is string))
            {
                throw new ArgumentNullException();
            }
            switch (parameter as string)
            {
                case "or":
                    foreach (bool b in values)
                    {
                        if (b)
                        {
                            return true;
                        }
                    }
                    return false;

                case "nor":
                    foreach (bool b in values)
                    {
                        if (b)
                        {
                            return false;
                        }
                    }
                    return true;

                case "and":
                    foreach (bool b in values)
                    {
                        if (!b)
                        {
                            return false;
                        }
                    }
                    return true;

                default:
                    throw new ArgumentException(nameof(parameter));
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}