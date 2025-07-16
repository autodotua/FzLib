using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace FzLib.Avalonia.Converters
{
    public abstract class Value2BoolConverter<T> : IValueConverter
    {
        public virtual bool ThrowIfNull { get; set; } = false;

        public virtual bool NullValue { get; set; } = false;

        public virtual bool InverseResult { get; set; } = false;

        protected abstract bool ConvertImpl(T value, Type targetType, object parameter, CultureInfo culture);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                if (ThrowIfNull)
                {
                    throw new ArgumentNullException(nameof(value));
                }
                return InverseResult ? !NullValue : NullValue;
            }
            if (value is not T)
            {
                throw new ArgumentException($"Value的类别不是{typeof(T).Name}", nameof(value));
            }
            var result = ConvertImpl((T)value, targetType, parameter, culture);
            return InverseResult ? !result : result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}