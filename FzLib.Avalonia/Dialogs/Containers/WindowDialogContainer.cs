using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.VisualTree;
using FzLib.Avalonia.Controls;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Avalonia.Xaml.Interactivity;
using FzLib.Avalonia.Behaviors;

namespace FzLib.Avalonia.Dialogs
{
    public class WindowDialogContainer : Window, IDialogHostContainer<Window>
    {
        internal WindowDialogContainer()
        {
            ExtendClientAreaToDecorationsHint = true;
            ExtendClientAreaChromeHints = global::Avalonia.Platform.ExtendClientAreaChromeHints.NoChrome;
            ExtendClientAreaTitleBarHeightHint = -1;
            SystemDecorations = SystemDecorations.BorderOnly; //避免在Linux上显示边框
            SizeToContent = SizeToContent.WidthAndHeight;
            CanResize = false;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            ShowInTaskbar = false;
            MinHeight = 120;
            MinWidth = 320;
            MaxWidth = 800;
            MaxHeight = 800;
            Padding = new Thickness(16);
        }

        protected override Type StyleKeyOverride => typeof(WindowDialogContainer);

        public Task ShowDialog(Window window, DialogHost dialogHost)
        {
            Content = dialogHost;
            return ShowDialog(window);
        }

        public Task ShowDialog(DialogHost dialogHost)
        {
            Content = dialogHost;
            TaskCompletionSource tcs = new TaskCompletionSource();
            Closed += WindowClosed;
            Show();
            return tcs.Task;

            void WindowClosed(object sender, EventArgs e)
            {
                Closed -= WindowClosed;
                tcs.SetResult();
            }
        }

        public Task<T> ShowDialog<T>(Window window, DialogHost dialogHost)
        {
            Content = dialogHost;
            return ShowDialog<T>(window);
        }

        protected override void OnLoaded(RoutedEventArgs e)
        {
            if (Content is not DialogHost dw)
            {
                throw new Exception($"{nameof(WindowDialogContainer)}的{nameof(Content)}必须为{nameof(DialogHost)}");
            }
            
            var thumb = this.FindThumb();
            if (thumb is null)
            {
                return;
            }

            var behavior = new WindowDragBehavior();

            Interaction.GetBehaviors(thumb).Add(behavior);
        }
    }
}