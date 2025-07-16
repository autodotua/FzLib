using Avalonia.Markup.Xaml;
using System;
using System.Collections.Generic;

namespace FzLib.Avalonia.MarkupExtensions
{
    public class StringToDictionaryExtension : MarkupExtension
    {
        public StringToDictionaryExtension()
        {
        }

        public StringToDictionaryExtension(string keyValuePairs)
        {
            this.KeyValuePairs = keyValuePairs;
        }

        [ConstructorArgument("keyValuePairs")]
        public string KeyValuePairs { get; set; }

        public char KeyValueSeparator { get; set; } = ':';

        public char PairSeparator { get; set; } = ';';
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var dictionary = new Dictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(KeyValuePairs))
            {
                var pairs = KeyValuePairs.Split(new[] { PairSeparator }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var pair in pairs)
                {
                    var keyValue = pair.Split(new[] { KeyValueSeparator }, 2);
                    if (keyValue.Length == 2)
                    {
                        dictionary[keyValue[0].Trim()] = keyValue[1].Trim();
                    }
                }
            }

            return dictionary;
        }
    }
}