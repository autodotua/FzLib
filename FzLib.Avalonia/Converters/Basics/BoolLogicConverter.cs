using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace FzLib.Avalonia.Converters
{
    public class BoolLogicConverter : IMultiValueConverter
    {
        public LogicalOperator Operator { get; set; } = LogicalOperator.And;

        public enum LogicalOperator
        {
            And,
            Or,
            Nor,
            Xor,
            Nand
        }

        public object Convert(IList<object> values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Count == 0)
            {
                return false;
            }

            bool result = (bool)values[0];
            for (int i = 1; i < values.Count; i++)
            {
                bool current = (bool)values[i];
                result = Operator switch
                {
                    LogicalOperator.And => result && current,
                    LogicalOperator.Or => result || current,
                    LogicalOperator.Nor => !(result || current),
                    LogicalOperator.Xor => result ^ current,
                    LogicalOperator.Nand => !(result && current),
                };
            }

            // 对于 Nor 和 Nand，我们需要在最后取反
            if (Operator is LogicalOperator.Nor or LogicalOperator.Nand)
            {
                result = !result;
            }

            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}