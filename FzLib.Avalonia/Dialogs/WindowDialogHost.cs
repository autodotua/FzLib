using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Styling;
using System;
using System.Reflection;

namespace FzLib.Avalonia.Dialogs
{
    public class WindowDialogHost : Window
    {
        internal WindowDialogHost()
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
            Loaded += WindowDialogHost_Loaded;
        }

        private void WindowDialogHost_Loaded(object sender, global::Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (Content is DialogHost dw)
            {
                //PrimaryButton = dw.PrimaryButton;
                //SecondaryButton = dw.SecondaryButton;
                //CloseButton = dw.CloseButton;
            }
            else
            {
                throw new Exception($"{nameof(WindowDialogHost)}的{nameof(Content)}必须为{nameof(DialogHost)}");
            }
            //PrimaryButton.Click += Button_Click;
            //SecondaryButton.Click += Button_Click;
            //CloseButton.Click += Button_Click;
        }

        //private void Button_Click(object sender, global::Avalonia.Interactivity.RoutedEventArgs e)
        //{

        //    if (sender == PrimaryButton)
        //    {
        //        OnPrimaryButtonClick();
        //    }
        //    else if (sender == SecondaryButton)
        //    {
        //        OnSecondaryButtonClick();
        //    }
        //    else if (sender == CloseButton)
        //    {
        //        OnCloseButtonClick();
        //    }
        //}

        //protected virtual void OnPrimaryButtonClick() { }
        //protected virtual void OnSecondaryButtonClick() { }
        //protected virtual void OnCloseButtonClick() { }


        //private Button PrimaryButton;
        //private Button SecondaryButton;
        //private Button CloseButton;
        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);
        }
    }
}