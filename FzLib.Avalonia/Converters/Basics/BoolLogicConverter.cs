using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;

namespace FzLib.Avalonia.Converters
{
    public class BoolLogicConverter : IMultiValueConverter
    {
        public LogicalOperator Operator { get; set; } = LogicalOperator.And;

        public NullOrUnsetHandling NullOrUnsetBehavior { get; set; } = NullOrUnsetHandling.ReturnFalse;

        public enum LogicalOperator
        {
            And,
            Or,
            Nor,
            Xor,
            Nand
        }

        public enum NullOrUnsetHandling
        {
            ReturnTrue,
            ReturnFalse,
            SeenAsTrue,
            SeenAsFalse,
            ThrowException
        }

        public object Convert(IList<object> values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Count == 0)
            {
                return false;
            }

            for (int i = 0; i < values.Count; i++)
            {
                if (values[i] is bool)
                {
                    continue;
                }

                if (values[i] is null || values[i] == AvaloniaProperty.UnsetValue)
                {
                    switch (NullOrUnsetBehavior)
                    {
                        case NullOrUnsetHandling.ReturnTrue:
                            return true;
                        case NullOrUnsetHandling.ReturnFalse:
                            return false;
                        case NullOrUnsetHandling.SeenAsTrue:
                            values[i] = true;
                            break;
                        case NullOrUnsetHandling.SeenAsFalse:
                            values[i] = false;
                            break;
                        case NullOrUnsetHandling.ThrowException:
                            throw new ArgumentException($"values[{i}]为空", nameof(values));
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    continue;
                }

                throw new ArgumentException($"values[{i}]不为bool类型", nameof(values));
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
                    _ => throw new ArgumentOutOfRangeException()
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