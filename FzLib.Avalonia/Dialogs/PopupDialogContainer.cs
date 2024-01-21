using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Media;
using System;
using System.Threading.Tasks;

namespace FzLib.Avalonia.Dialogs
{
    public class PopupDialogContainer : Panel, IDialogHostContainer<Grid>
    {
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

        public async Task<T> ShowDialog<T>(Grid container, DialogHost dialogHost)
        {
            Border bdBackground = new Border()
            {
                Background = Brushes.Gray,
                Opacity = 0.5,
            };
            bdBackground[!BackgroundProperty] = new DynamicResourceExtension("SystemControlBackgroundAltHighBrush");
            Children.Add(bdBackground);

            Border bdDialog = new Border()
            {
                CornerRadius = new CornerRadius(4),
                Effect = new DropShadowEffect()
                {
                    BlurRadius = 6,
                    OffsetX = 0,
                    OffsetY = 0,
                },
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Child = dialogHost,
                Background = Brushes.Black,
                MinHeight = 120,
                MinWidth = 320,
                MaxWidth = 800,
                MaxHeight = 800
            };
            (bdDialog.Effect as DropShadowEffect)[!DropShadowEffectBase.ColorProperty] = new DynamicResourceExtension("SystemControlBackgroundChromeMediumLowBrush");
            bdDialog[!BackgroundProperty] = new DynamicResourceExtension("SystemControlBackgroundChromeMediumLowBrush");
            Children.Add(bdDialog);

            Grid.SetColumnSpan(this, int.MaxValue);
            Grid.SetRowSpan(this, int.MaxValue);
            container.Margin.Deconstruct(out double left, out double top, out double right, out double bottom);
            Margin = new Thickness(-left, -top, -right, -bottom);
            HorizontalAlignment = HorizontalAlignment.Stretch;
            VerticalAlignment = VerticalAlignment.Stretch;

            container.Children.Add(this);
            tcs = new TaskCompletionSource<object>();
            await tcs.Task;
            return (T)tcs.Task.Result;
        }

        public Task ShowDialog(Grid container, DialogHost dialogHost)
        {
            return ShowDialog<object>(container, dialogHost);
        }
    }
}