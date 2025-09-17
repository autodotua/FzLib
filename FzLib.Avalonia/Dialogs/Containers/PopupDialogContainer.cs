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
using Avalonia.Media.Transformation;
using Avalonia.Threading;
using Avalonia.Xaml.Interactivity;
using FzLib.Avalonia.Behaviors;

namespace FzLib.Avalonia.Dialogs
{
    public class PopupDialogContainer : ContentControl, IDialogHostContainer<Grid>
    {
        public PopupDialogContainer()
        {
        }

        TaskCompletionSource<object> tcs;

        public void Close()
        {
            if (tcs == null)
            {
                throw new Exception($"还未调用{nameof(ShowDialog)}");
            }

            (Parent as Grid).Children.Remove(this);
            tcs.SetResult(null);
        }

        public void Close(object result)
        {
            if (tcs == null)
            {
                throw new Exception($"还未调用{nameof(ShowDialog)}");
            }

            (Parent as Grid).Children.Remove(this);
            tcs.SetResult(result);
        }

        private Border bdDialog;

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
            var thumb = this.FindThumb();
            if (thumb is null)
            {
                return;
            }

            var bd = (Border)this.GetVisualDescendants().First(p => p.Name == "PART_Dialog");


            bd.Opacity = 1;
            bd.RenderTransform = TransformOperations.Parse("scale(1)");

            var bg = this.GetVisualDescendants().First(p => p.Name == "PART_Background");
            bg.Opacity = 0.5;
            var behavior = new VisualDragBehavior
            {
                Target = bd
            };
            //下面这行，放在上面几行的上面就不行，很神奇
            Interaction.GetBehaviors(thumb).Add(behavior);
        }

        protected override Type StyleKeyOverride => typeof(PopupDialogContainer);

        public Task ShowDialog(Grid container, DialogHost dialogHost)
        {
            return ShowDialog<object>(container, dialogHost);
        }
    }
}