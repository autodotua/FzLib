using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Markup;

namespace FzLib.WPF.MarkupExtensions
{
    [MarkupExtensionReturnType(typeof(object[]))]
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
                throw new ArgumentException("枚举类型不存在");
            return Enum.GetValues(this.EnumType);
        }
    }
}