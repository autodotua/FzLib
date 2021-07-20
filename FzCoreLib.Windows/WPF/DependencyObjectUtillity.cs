using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FzLib.WPF
{
    public static class DependencyObjectExtension
    {
        public static T GetChild<T>(this DependencyObject obj) where T : DependencyObject
        {
            DependencyObject child = null;
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                child = VisualTreeHelper.GetChild(obj, i) as Visual;
                if (child != null && child is T)
                {
                    break;
                }
                else if (child != null)
                {
                    child = child.GetChild<T>();
                    if (child != null && child is T)
                    {
                        break;
                    }
                }
            }
            return child as T;
        }

        public static List<T> GetChilds<T>(this DependencyObject obj) where T : DependencyObject
        {
            List<T> results = new List<T>();
            Get(obj);
            void Get(DependencyObject visual)
            {
                Visual child;
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(visual); i++)
                {
                    child = VisualTreeHelper.GetChild(visual, i) as Visual;
                    if (child != null && child is T t)
                    {
                        results.Add(t);
                    }
                    if (child != null)
                    {
                        Get(child);
                    }
                }
            }
            return results;
        }

        public static T GetParent<T>([NotNull] this FrameworkElement obj) where T : DependencyObject
        {
            DependencyObject result = obj.Parent;
            while (result != null && !(result is T))
            {
                if (result is FrameworkElement f)
                {
                    result = f.Parent;
                }
                else
                {
                    return null;
                }
            }
            return result as T;
        }
    }
}