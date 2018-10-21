using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace FzLib.Wpf.Control.Win10Style
{
    public static class CommonResources
    {
       public static Collection<ResourceDictionary> Resources =>
            new Collection<ResourceDictionary>()    {
                    new ResourceDictionary(){Source=new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Colors.xaml") },
                    new ResourceDictionary(){Source=new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Fonts.xaml") },
                    new ResourceDictionary(){Source=new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/Blue.xaml") },
                    new ResourceDictionary(){Source=new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/BaseLight.xaml") }
        };

        public static void InitializeControlResouces(ResourceDictionary resource)
        {
            foreach (var item in Resources)
            {
                resource.MergedDictionaries.Add(item);
            }
            //resource.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Controls.xaml") });
        }

        public static void InitializeControlResouces(ResourceDictionary resource,params string[] paths)
        {
            foreach (var item in Resources)
            {
                resource.MergedDictionaries.Add(item);
            }
            foreach (var path in paths)
            {
                resource.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri($"pack://application:,,,/MahApps.Metro;component/Styles/{path}.xaml") });
            }

        }
        public static void InitializeControlResoucesInThemes(ResourceDictionary resource, params string[] paths)
        {
            foreach (var item in Resources)
            {
                resource.MergedDictionaries.Add(item);
            }
            foreach (var path in paths)
            {
                resource.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri($"pack://application:,,,/MahApps.Metro;component/Themes/{path}.xaml") });
            }

        }
    }
}
