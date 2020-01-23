using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace FzLib.UI.Picker
{
    public class FontPicker : FzLib.UI.FlatStyle.UneditableComboBox
    {
        public FontPicker()
        {
            foreach (var item in Fonts.SystemFontFamilies)
            {
                LanguageSpecificStringDictionary fontDic = item.FamilyNames;
                if (fontDic.ContainsKey(XmlLanguage.GetLanguage("zh-cn")))
                {
                    if (fontDic.TryGetValue(XmlLanguage.GetLanguage("zh-cn"), out string fontName))
                    {
                        Items.Add(GetNewItem(fontName, item));
                    }
                }
            }
            foreach (var item in Fonts.SystemFontFamilies)
            {
                LanguageSpecificStringDictionary fontDic = item.FamilyNames;
                if (fontDic.ContainsKey(XmlLanguage.GetLanguage("zh-cn")))
                {
                    if (!fontDic.TryGetValue(XmlLanguage.GetLanguage("zh-cn"), out string fontName))
                    {
                            Items.Add(GetNewItem(item.ToString(), item));
                        
                    }
                }
                else
                {
                    Items.Add(GetNewItem(item.ToString(), item));

                }
            }

        }

        private StackPanel GetNewItem(string name, FontFamily font)
        {
            StackPanel stk = new StackPanel() { Orientation = Orientation.Horizontal };
            stk.Children.Add(new TextBlock() { Text = name, Margin = new System.Windows.Thickness(0, 0, 8, 0) ,VerticalAlignment=System.Windows.VerticalAlignment.Center});
            stk.Children.Add(new TextBlock() { Text = name, FontFamily = font ,VerticalAlignment=System.Windows.VerticalAlignment.Center});
            stk.Tag = font;
            return stk;
        }

        public FontFamily SelectedFont => SelectedIndex == -1 ? null : (SelectedItem as StackPanel).Tag as FontFamily;
        
        public bool SetSelectedFontByString(string name)
        {
            foreach (var item in Items)
            {
                FontFamily font = (item as StackPanel).Tag as FontFamily;
                LanguageSpecificStringDictionary fontDic = font.FamilyNames;
                if (fontDic.Any(p=>p.Value==name))
                {
                    SelectedItem = item;
                    return true;
                }
            }
            SelectedItem = -1;
            return false;
        }

        public  string GetPreferChineseFontName()
        {
            return GetPreferChineseFontName(SelectedFont);
        }

        public static string GetPreferChineseFontName(FontFamily font)
        {
            if(font==null)
            {
                return null;
            }
            LanguageSpecificStringDictionary fontDic = font.FamilyNames;
            if (fontDic.ContainsKey(XmlLanguage.GetLanguage("zh-cn")))
            {
                if (!fontDic.TryGetValue(XmlLanguage.GetLanguage("zh-cn"), out string fontName))
                {
                    {
                        return fontName;
                    }
                }
            }
            return font.ToString();
        }
    }
}
