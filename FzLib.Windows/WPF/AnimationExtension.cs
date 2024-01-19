using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace FzLib.WPF
{
    public static class AnimationExtension
    {
        public static T StopWhenComplete<T>(this T animation) where T : Timeline
        {
            animation.FillBehavior = FillBehavior.Stop;
            return animation;
        }

        public static T EnableAutoReverse<T>(this T animation) where T : Timeline
        {
            animation.AutoReverse = true;
            return animation;
        }

        public static T SetRepeat<T>(this T animation, double count) where T : Timeline
        {
            animation.RepeatBehavior = new RepeatBehavior(count);
            return animation;
        }

        public static T SetRepeat<T>(this T animation, TimeSpan span) where T : Timeline
        {
            animation.RepeatBehavior = new RepeatBehavior(span);
            return animation;
        }

        public static T SetInOutCubicEase<T>(this T animation) where T : DoubleAnimation
        {
            return animation.SetEasing<T, CubicEase>(EasingMode.EaseInOut);
        }

        public static T SetEasing<T, TE>(this T animation, EasingMode mode)
            where T : DoubleAnimation
            where TE : EasingFunctionBase, new()
        {
            animation.EasingFunction = new TE() { EasingMode = mode };
            return animation;
        }

        public static T SetStoryboard<T>(this T animation, DependencyProperty property, DependencyObject control) where T : AnimationTimeline
        {
            return animation.SetStoryboard(property == null ? (PropertyPath)null : new PropertyPath(property.Name), control);
        }

        public static T SetStoryboard<T>(this T animation, string propertyPath, DependencyObject control) where T : AnimationTimeline
        {
            return animation.SetStoryboard(propertyPath == null ? (PropertyPath)null : new PropertyPath(propertyPath), control);
        }

        public static T SetStoryboard<T>(this T animation, PropertyPath propertyPath, DependencyObject control) where T : AnimationTimeline
        {
            if (propertyPath != null)
            {
                Storyboard.SetTargetProperty(animation, propertyPath);
            }
            if (control != null)
            {
                Storyboard.SetTarget(animation, control);
            }
            return animation;
        }

        public static T AddTo<T>(this T animation, Storyboard storyboard) where T : Timeline
        {
            storyboard.Children.Add(animation);
            return animation;
        }

        public static Task BeginAsync(this Storyboard storyboard)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            if (storyboard == null)
            {
                tcs.SetException(new ArgumentNullException());
            }
            else
            {
                EventHandler onComplete = null;
                onComplete = (s, e) =>
                {
                    storyboard.Completed -= onComplete;
                    tcs.SetResult(true);
                };
                storyboard.Completed += onComplete;
                storyboard.Begin();
            }
            return tcs.Task;
        }

        public static Task BeginAsync(this AnimationTimeline animation, UIElement element, DependencyProperty property)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            if (animation == null)
            {
                tcs.SetException(new ArgumentNullException());
            }
            else
            {
                EventHandler onComplete = null;
                onComplete = (s, e) =>
                {
                    animation.Completed -= onComplete;
                    tcs.SetResult(true);
                };
                animation.Completed += onComplete;
                element.BeginAnimation(property, animation);
            }
            return tcs.Task;
        }
    }
}