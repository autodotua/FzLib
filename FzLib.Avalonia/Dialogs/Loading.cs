using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Layout;
using Avalonia.Markup.Xaml.MarkupExtensions;
using System.Threading;
using Avalonia.Threading;
using Avalonia.Interactivity;
using Avalonia.Animation;
using Avalonia.Styling;

namespace FzLib.Avalonia.Dialogs
{
    public class LoadingOverlay : Border
    {
        private static readonly TimeSpan AnimationTime = TimeSpan.FromSeconds(0.25);
        public LoadingOverlay()
        {
            this.Child = new ProgressRing()
            {
                IsActive = true,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Width = 64,
                Height = 64,
            };
            Opacity = 0;
            Transitions =
            [
                new DoubleTransition()
                {
                    Property = OpacityProperty,
                    Duration = TimeSpan.FromSeconds(0.25)
                },
            ];
            this[!BackgroundProperty] = new DynamicResourceExtension("SystemAltMediumColor");
        }


        public static CancellationTokenSource ShowLoading(Visual visual, TimeSpan delay = default)
        {
            var topLevel = TopLevel.GetTopLevel(visual) ?? throw new ArgumentException("找不到TopLevel", nameof(visual));
            bool canWindowDialog = topLevel is Window;//在桌面端，TopLevel是窗口
            Grid container = null;
            if (topLevel.Content is Grid g)
            {
                container = g;
            }
            else if (topLevel.Content is ContentControl cc && cc.Content is Grid g2)
            {
                container = g2;
            }
            if (container == null)
            {
                throw new NotSupportedException("找不到顶层Grid");
            }
            LoadingOverlay overlay = new LoadingOverlay();
            Grid.SetColumnSpan(overlay, int.MaxValue);
            Grid.SetRowSpan(overlay, int.MaxValue);
            container.Margin.Deconstruct(out double left, out double top, out double right, out double bottom);
            overlay.Margin = new Thickness(-left, -top, -right, -bottom);
            overlay.HorizontalAlignment = HorizontalAlignment.Stretch;
            overlay.VerticalAlignment = VerticalAlignment.Stretch;

            container.Children.Add(overlay);
            CancellationTokenSource cts = new CancellationTokenSource();
            bool canceled = false;
            CancellationTokenRegistration? cancellationTokenRegistration = null;
            if (delay != default)
            {
                //如果在延迟期间就已经取消，那么不展示动画，直接移除
                cancellationTokenRegistration= cts.Token.Register(() =>
                {
                    canceled = true;
                    container.Children.Remove(overlay);
                });
                Task.Delay(delay).ContinueWith(t => Begin());
            }
            else
            {
                Begin();
            }

            return cts;

            void Begin()
            {
                //如果在延迟期间就已经取消，那么不再执行后续操作
                if (canceled)
                {
                    return;
                }
                //在开启延迟的情况下，如果上面已经注册过，那么要先把上面的给反注册
                if (cancellationTokenRegistration.HasValue)
                {
                    cancellationTokenRegistration.Value.Dispose();
                }
                cts.Token.Register(() =>
                {
                    Dispatcher.UIThread.Invoke(async () =>
                    {
                        overlay.Opacity = 0;
                        await Task.Delay(AnimationTime);
                        container.Children.Remove(overlay);
                    });
                });
                if (!cts.IsCancellationRequested)
                {
                    Dispatcher.UIThread.Invoke(() =>
                    {
                        overlay.Opacity = 1;
                    });
                }
            }
        }
    }
}
