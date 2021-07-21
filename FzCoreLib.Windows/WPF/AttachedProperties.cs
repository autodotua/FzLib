using System.Windows;
using System.Windows.Media;

namespace FzLib.WPF
{
    public static class AttachedProperties
    {
        public static DependencyProperty EasyRotateProperty = DependencyProperty.RegisterAttached(
            "EasyRotate", typeof(double), typeof(AttachedProperties),
            new PropertyMetadata(new PropertyChangedCallback((s, e) =>
            {
                UIElement element = s as UIElement;
                if (element == null)
                {
                    return;
                }
                if (e.NewValue is double d)
                {
                    element.RenderTransformOrigin = new Point(0.5, 0.5);
                    element.RenderTransform = new RotateTransform(d);
                }
            }))
            );

        public static void SetEasyRotate(UIElement element, double value)
        {
            element.SetValue(EasyRotateProperty, value);
        }

        public static double GetEasyRotate(UIElement element)
        {
            return (double)element.GetValue(EasyRotateProperty);
        }
    }
}