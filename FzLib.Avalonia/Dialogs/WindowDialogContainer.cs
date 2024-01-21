using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Media;
using Avalonia.Styling;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace FzLib.Avalonia.Dialogs
{
    public class WindowDialogContainer : Window, IDialogHostContainer<Window>
    {
        internal WindowDialogContainer()
        {
            ResourceDictionary rd = new ResourceDictionary();
            string assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            var uri = new Uri($"avares://{assemblyName}/Dialogs/DialogHostStyles.axaml");
            rd.MergedDictionaries.Add(new ResourceInclude((Uri)null) { Source = uri });
            Resources = rd;
            Theme = this.FindResource("DialogWindowTheme") as ControlTheme;

            ExtendClientAreaToDecorationsHint = true;
            ExtendClientAreaChromeHints = global::Avalonia.Platform.ExtendClientAreaChromeHints.NoChrome;
            ExtendClientAreaTitleBarHeightHint = -1;
            SizeToContent = SizeToContent.WidthAndHeight;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            ShowInTaskbar = false;
            MinHeight = 120;
            MinWidth = 320;
            MaxWidth = 800;
            MaxHeight = 800;
            Padding = new Thickness(16);
            Loaded += WindowDialogContainer_Loaded;
        }

        private void WindowDialogContainer_Loaded(object sender, global::Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (Content is DialogHost dw)
            {
            }
            else
            {
                throw new Exception($"{nameof(WindowDialogContainer)}的{nameof(Content)}必须为{nameof(DialogHost)}");
            }
        }

        public Task ShowDialog(Window window, DialogHost dialogHost)
        {
            Content = dialogHost;
            return ShowDialog(window);
        }

        public Task<T> ShowDialog<T>(Window window, DialogHost dialogHost)
        {
            Content = dialogHost;
            return ShowDialog<T>(window);
        }

    }
}