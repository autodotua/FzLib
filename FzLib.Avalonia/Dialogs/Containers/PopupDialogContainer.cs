using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Media;
using Avalonia.VisualTree;
using FzLib.Avalonia.Controls;
using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls.Primitives;
using Avalonia.Media.Transformation;
using Avalonia.Threading;
using Avalonia.Xaml.Interactivity;
using FzLib.Avalonia.Behaviors;

namespace FzLib.Avalonia.Dialogs
{
    public class PopupDialogContainer : ContentControl, IDialogHostContainer<Grid>
    {
        public static readonly TimeSpan AnimationDuration = TimeSpan.FromMilliseconds(100);

        TaskCompletionSource<object> tcs;

        public void Close()
        {
            if (tcs == null)
            {
                throw new Exception($"还未调用{nameof(ShowDialog)}");
            }

            HideAndRemoveSelf();

            tcs.SetResult(null);
        }

        private void HideAndRemoveSelf()
        {
            //开始退出动画
            bdDialog.Opacity = 0;
            bdDialog.RenderTransform = TransformOperations.Parse("scale(0.98)  translate(0,-10px)");
            bdBackground.Opacity = 0.0;
            Task.Delay(AnimationDuration).ContinueWith(_ =>
            {
                Dispatcher.UIThread.Post(() =>
                {
                    ((Panel)Parent).Children.Remove(this);
                });
            });
        }

        public void Close(object result)
        {
            if (tcs == null)
            {
                throw new Exception($"还未调用{nameof(ShowDialog)}");
            }

            HideAndRemoveSelf();
            tcs.SetResult(result);
        }

        public async Task<T> ShowDialog<T>(Grid container, DialogHost dialogHost)
        {
            container.Margin.Deconstruct(out double left, out double top, out double right, out double bottom);
            Margin = new Thickness(-left, -top, -right, -bottom);
            HorizontalAlignment = HorizontalAlignment.Stretch;
            VerticalAlignment = VerticalAlignment.Stretch;

            container.Children.Add(this);
            Content = dialogHost;
            tcs = new TaskCompletionSource<object>();
            await tcs.Task;
            return (T)tcs.Task.Result;
        }

        protected override void OnLoaded(RoutedEventArgs e)
        {
            base.OnLoaded(e);

            //开始打开动画
            bdDialog.Opacity = 1;
            bdDialog.RenderTransform = TransformOperations.Parse("scale(1)");
            bdBackground.Opacity = 0.5;
            ((DropShadowEffect)bdDialog.Effect).Color = Colors.Black;

            //实现拖放
            var thumb = this.FindThumb();
            if (thumb is null)
            {
                return;
            }

            var behavior = new VisualDragBehavior
            {
                Target = bdDialog
            };
            //下面这行，放在上面几行的上面就不行，很神奇
            Interaction.GetBehaviors(thumb).Add(behavior);
        }

        private Border bdDialog;
        private Border bdBackground;

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);
            bdDialog = e.NameScope.Find<Border>("PART_Dialog");
            bdBackground = e.NameScope.Find<Border>("PART_Background");
        }

        protected override Type StyleKeyOverride => typeof(PopupDialogContainer);

        public Task ShowDialog(Grid container, DialogHost dialogHost)
        {
            return ShowDialog<object>(container, dialogHost);
        }
    }
}