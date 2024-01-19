using System.Windows;
using System.Windows.Media;

namespace FzLib.WPF
{
    public static class EasyTransform
    {
        public static DependencyProperty EasyRotateProperty = DependencyProperty.RegisterAttached(
            "EasyRotate", typeof(double), typeof(EasyTransform),
            new PropertyMetadata(new PropertyChangedCallback((s, e) =>
            {
                if (!(s is UIElement element))
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