using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FzLib.Avalonia.Dialogs
{
    public static class CommonDialogExtension
    {
        public static Task ShowOkDialogAsync(this Window window, string title, string message = null, string detail = null)
        {
            MessageDialog dialog = new MessageDialog(new MessageDialogViewModel()
            {
                Title = title,
                Message = message,
                Detail = detail,
                //Icon = MessageDialog.InfoIcon,
                //IconBrush = window.Foreground
            });
            return dialog.ShowDialog(window);
        }

        public static Task ShowWarningDialogAsync(this Window window, string title, string message = null, string detail = null)
        {
            MessageDialog dialog = new MessageDialog(new MessageDialogViewModel()
            {
                Title = title,
                Message = message,
                Detail = detail,
                Icon = MessageDialog.WarningIcon,
                IconBrush = SolidColorBrush.Parse("#ffb900")
            }); 
            return dialog.ShowDialog(window);
        }


        public static Task ShowErrorDialogAsync(this Window window, string title, string message = null, string detail = null)
        {
            MessageDialog dialog = new MessageDialog(new MessageDialogViewModel()
            {
                Title = title,
                Message = message,
                Detail = detail,
                Icon = MessageDialog.ErrorIcon,
                IconBrush = Brushes.Red
            });
            return dialog.ShowDialog(window);
        }

        public static Task ShowErrorDialogAsync(this Window window, string title, Exception ex)
        {
            MessageDialog dialog = new MessageDialog(new MessageDialogViewModel()
            {
                Title = title,
                Message = ex.Message,
                Detail = ex.ToString(),
                Icon = MessageDialog.ErrorIcon,
                IconBrush = Brushes.Red
            });
            return dialog.ShowDialog(window);
        }
    }
}