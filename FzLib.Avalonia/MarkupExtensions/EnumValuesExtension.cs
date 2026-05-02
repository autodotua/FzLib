using Avalonia.Markup.Xaml;
using Avalonia.Metadata;
using System;
using System.Collections;
using System.Collections.Generic;

namespace FzLib.Avalonia.MarkupExtensions
{
    public class EnumValuesExtension : MarkupExtension
    {
        public EnumValuesExtension()
        {
        }

        public EnumValuesExtension(Type enumType)
        {
            this.EnumType = enumType;
        }

        [ConstructorArgument("enumType")]
        public Type EnumType { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (this.EnumType == null)
            {
                throw new ArgumentException("枚举类型不存在");
            }
            //下面方法不支持AOT
            //return Enum.GetValues(this.EnumType);

            //https://github.com/irihitech/Ursa.Avalonia/blob/main/src%2FUrsa%2FControls%2FEnumSelector%2FEnumSelector.cs
            var values = Enum.GetValuesAsUnderlyingType(EnumType);
            var list = new List<object>();
            foreach (var value in values)
            {
                var enumValue = Enum.ToObject(EnumType, value);
                list.Add(enumValue);
            }
            return list;
        }
    }

    public class EnumValuesExtension<T> : MarkupExtension where T : struct, Enum
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Enum.GetValues<T>();
        }
    }
}