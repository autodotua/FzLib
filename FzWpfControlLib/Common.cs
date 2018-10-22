using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;
using Windows.UI.ViewManagement;

namespace FzLib.Control
{
    public static class Common
    {
        public static Color GetSystemThemeColor()
        {

            UISettings ui = new UISettings();
            var color = ui.GetColorValue(UIColorType.Accent);
            return new Color() { A = color.A, R = color.R, G = color.G, B = color.B };
        }

        public static T GetFirstChildOfType<T>(DependencyObject dependencyObject) where T : DependencyObject
        {
            if (dependencyObject == null)
            {
                return null;
            }

            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(dependencyObject); i++)
            {
                var child = VisualTreeHelper.GetChild(dependencyObject, i);

                var result = (child as T) ?? GetFirstChildOfType<T>(child);

                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }
        public class StorageOperationEventArgs : EventArgs
        {
            public StorageOperationEventArgs(string name, CommonFileDialogResult result)
            {
                Name = name;
                Result = result;
            }
            public StorageOperationEventArgs(CommonFileDialogResult result)
            {
                Result = result;
            }
            public StorageOperationEventArgs(string[] names, CommonFileDialogResult result)
            {
                Names = names;
                Name = names[0];
                Result = result;
            }

            public string Name { get; private set; }
            public string[] Names { get; private set; }
            public CommonFileDialogResult Result { get; private set; }
        }

        public static T CloneXaml<T>(T source)
        {
            string xaml = XamlWriter.Save(source);
            StringReader sr = new StringReader(xaml);
            XmlReader xr = XmlReader.Create(sr);
            return (T)XamlReader.Load(xr);
        }

    }

}
