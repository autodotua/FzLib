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

        public static readonly double DefaultBackgroundOpacity = 0.5;
        public static readonly double DefaultDialogOpacity = 1;
        public static readonly TransformOperations DefaultDialogTransform = TransformOperations.Parse("scale(1)");
        public static readonly double InitialBackgroundOpacity = 0;
        public static readonly double InitialDialogOpacity = 0;

        public static readonly TransformOperations InitialDialogTransform =
            TransformOperations.Parse("scale(0.98)  translate(0,20px)");

        private Border bdBackground;
        private Border bdDialog;
        TaskCompletionSource<object> tcs;

        protected override Type StyleKeyOverride => typeof(PopupDialogContainer);

        public void Close()
        {
            if (tcs == null)
            {
                throw new Exception($"还未调用{nameof(ShowDialog)}");
            }

            HideAndRemoveSelf();

            tcs.SetResult(null);
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
            Grid.SetRowSpan(this, int.MaxValue);
            Grid.SetColumnSpan(this, int.MaxValue);
            Content = dialogHost;
            tcs = new TaskCompletionSource<object>();
            await tcs.Task;
            return (T)tcs.Task.Result;
        }

        public Task ShowDialog(Grid container, DialogHost dialogHost)
        {
            return ShowDialog<object>(container, dialogHost);
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);
            bdDialog = e.NameScope.Find<Border>("PART_Dialog");
            bdBackground = e.NameScope.Find<Border>("PART_Background");
        }

        protected override void OnLoaded(RoutedEventArgs e)
        {
            base.OnLoaded(e);

            //开始打开动画
            BeginAnimation(true);

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

        private void BeginAnimation(bool show)
        {
            var parameters = show
                ? (DefaultDialogOpacity, DefaultDialogTransform, DefaultBackgroundOpacity)
                : (InitialDialogOpacity, InitialDialogTransform, InitialBackgroundOpacity);
            bdDialog.Opacity = parameters.Item1;
            bdDialog.RenderTransform = parameters.Item2;
            bdBackground.Opacity = parameters.Item3;
        }

        private async void HideAndRemoveSelf()
        {
            BeginAnimation(false);
            await Task.Delay(AnimationDuration);
            Dispatcher.UIThread.Post(() => ((Panel)Parent).Children.Remove(this));
        }
    }
}