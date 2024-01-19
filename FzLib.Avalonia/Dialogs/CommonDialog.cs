﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml.Styling;
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
        public static Task ShowOkDialogAsync(this Window window, string title, string message = null)
        {
            MessageDialog dialog = new MessageDialog(title, message, null);
            return dialog.ShowDialog(window);
        }
    }

    public abstract class CommonDialog : Window
    {
        public static string OkButtonText = "确定";
        public static string YesButtonText = "是";
        public static string NoButtonText = "否";
        public static string CancelButtonText = "取消";
        public static string CloseButtonText = "关闭";

        internal CommonDialog()
        { 
            ExtendClientAreaToDecorationsHint = true;
            ExtendClientAreaChromeHints = global::Avalonia.Platform.ExtendClientAreaChromeHints.NoChrome;
            ExtendClientAreaTitleBarHeightHint = -1;
            SizeToContent = SizeToContent.WidthAndHeight;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            ShowInTaskbar = false;
            MinHeight = 184;
            MinWidth = 320;
            MaxWidth = 800;
            MaxHeight = 800;
            Padding = new Thickness(16);
            PrimaryButtonContent = null;
        }
        Button PrimaryButton;
        Button SecondaryButton;
        Button CloseButton;
        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            PrimaryButton= e.NameScope.Find(nameof(PrimaryButton)) as Button;
            SecondaryButton = e.NameScope.Find(nameof(SecondaryButton)) as Button;
            CloseButton = e.NameScope.Find(nameof(CloseButton)) as Button;
            CloseButton.Click += (s, e) => Close();
            base.OnApplyTemplate(e);
        }

        public static readonly StyledProperty<object> PrimaryButtonContentProperty =
    AvaloniaProperty.Register<CommonDialog, object>(nameof(PrimaryButtonContent));
        public object PrimaryButtonContent
        {
            get => GetValue(PrimaryButtonContentProperty);
            set => SetValue(PrimaryButtonContentProperty, value);
        }
        public static readonly StyledProperty<object> SecondaryButtonContentProperty =
    AvaloniaProperty.Register<CommonDialog, object>(nameof(SecondaryButtonContent));
        public object SecondaryButtonContent
        {
            get => GetValue(SecondaryButtonContentProperty);
            set => SetValue(SecondaryButtonContentProperty, value);
        }
        public static readonly StyledProperty<object> CloseButtonContentProperty =
    AvaloniaProperty.Register<CommonDialog, object>(nameof(CloseButtonContent));
        public object CloseButtonContent
        {
            get => GetValue(CloseButtonContentProperty);
            set => SetValue(CloseButtonContentProperty, value);
        }

        public void CloseCommand()
        {
            Close();
        }
        //public static async Task<int?> ShowIntInputDialogAsync(string title, int? defaultValue = null)
        //{
        //    InputDialog dialog = new InputDialog(p => int.TryParse(p, out int _), "1234567890")
        //    {
        //        Title = title,
        //    };
        //    if (defaultValue.HasValue)
        //    {
        //        dialog.InputContent = defaultValue.ToString();
        //    }
        //    if (await dialog.ShowAsync() == ContentDialogResult.Primary)
        //    {
        //        return int.Parse(dialog.InputContent);
        //    }
        //    return null;
        //}

        //public static async Task<double?> ShowDoubleInputDialogAsync(string title, double? defaultValue = null)
        //{
        //    InputDialog dialog = new InputDialog(p => double.TryParse(p, out double _), "1234567890.")
        //    {
        //        Title = title,
        //    };
        //    if (defaultValue.HasValue)
        //    {
        //        dialog.InputContent = defaultValue.ToString();
        //    }
        //    if (await dialog.ShowAsync() == ContentDialogResult.Primary)
        //    {
        //        return double.Parse(dialog.InputContent);
        //    }
        //    return null;
        //}

        //public static async Task<string> ShowInputDialogAsync(string title, string defaultText = "", bool multiLines = false, int maxLines = 1)
        //{
        //    InputDialog dialog = new InputDialog()
        //    {
        //        Title = title,
        //        InputContent = defaultText,
        //        MultiLines = multiLines,
        //        MaxLines = maxLines
        //    };
        //    if (await dialog.ShowAsync() == ContentDialogResult.Primary)
        //    {
        //        return dialog.InputContent;
        //    }
        //    return null;
        //}

        //public static async Task<bool> ShowYesNoDialogAsync(string title, string message = null, string detail = null)
        //{
        //    DetailTextDialog dialog = new DetailTextDialog()
        //    {
        //        Title = title,
        //        Message = message,
        //        PrimaryButtonText = "是",
        //        IsPrimaryButtonEnabled = true,
        //        SecondaryButtonText = "否",
        //        IsSecondaryButtonEnabled = true,
        //        Detail = detail
        //    };
        //    var result = await dialog.ShowAsync();
        //    return result == ContentDialogResult.Primary;
        //}

        public static Task ShowOkDialogAsync(string title, string message, string detail)
        {
            //DetailTextDialog dialog = new DetailTextDialog()
            //{
            //    Title = title,
            //    Message = message,
            //    Detail = detail,
            //    PrimaryButtonText = "确定",
            //    IsPrimaryButtonEnabled = true,
            //};
            //return dialog.ShowAsync();
            return null;
        }

        //public static Task ShowErrorDialogAsync(string message, string detail, string title = "错误")
        //{
        //    DetailTextDialog dialog = new DetailTextDialog()
        //    {
        //        Detail = detail,
        //        PrimaryButtonText = "确定",
        //        IsPrimaryButtonEnabled = true,
        //        Icon = "\uEA39",
        //        Title = title,
        //        Message = message,
        //        IconBrush = System.Windows.Media.Brushes.Red
        //    };
        //    return dialog.ShowAsync();
        //}

        //public static Task ShowErrorDialogAsync(Exception ex, string message = null)
        //{
        //    DetailTextDialog dialog = new DetailTextDialog()
        //    {
        //        Detail = ex == null ? null : ex.ToString(),
        //        PrimaryButtonText = "确定",
        //        IsPrimaryButtonEnabled = true,
        //        Icon = "\uEA39",
        //        IconBrush = System.Windows.Media.Brushes.Red
        //    };
        //    if (ex == null)
        //    {
        //        dialog.Title = "错误";
        //        dialog.Message = message;
        //    }
        //    else
        //    {
        //        dialog.Title = message ?? "错误";
        //        dialog.Message = ex.Message;
        //    }
        //    return dialog.ShowAsync();
        //}

        //public static Task ShowErrorDialogAsync(string message)
        //{
        //    if (message == null)
        //    {
        //        throw new ArgumentNullException();
        //    }
        //    return ShowErrorDialogAsync(null, message);
        //}

        //public static async Task<int> ShowSelectItemDialogAsync(string title, IEnumerable<SelectDialogItem> items, string extraButtonText = null, Action extraButtonAction = null)
        //{
        //    SelectItemDialog dialog = new SelectItemDialog()
        //    {
        //        Title = title,
        //        Items = new List<SelectDialogItem>(items),
        //    };
        //    if (extraButtonText != null)
        //    {
        //        dialog.IsShadowEnabled = true;
        //        dialog.SecondaryButtonText = extraButtonText;
        //        dialog.SecondaryButtonClick += (p1, p2) => extraButtonAction();
        //    }
        //    await dialog.ShowAsync();
        //    return dialog.SelectedIndex;
        //}

        //public static async Task<IReadOnlyList<CheckDialogItem>> ShowCheckBoxDialogAsync(string title,
        //    IEnumerable<CheckDialogItem> items,
        //    bool needAtLeastOneCheck,
        //    string extraButtonText = null,
        //    Action extraButtonAction = null)
        //{
        //    CheckBoxDialog dialog = new CheckBoxDialog()
        //    {
        //        Title = title,
        //        Items = new List<CheckDialogItem>(items),
        //        NeedAtLeastOneCheck = needAtLeastOneCheck
        //    };
        //    if (extraButtonText != null)
        //    {
        //        dialog.IsShadowEnabled = true;
        //        dialog.SecondaryButtonText = extraButtonText;
        //        dialog.SecondaryButtonClick += (p1, p2) => extraButtonAction();
        //    }
        //    if (await dialog.ShowAsync() == ContentDialogResult.Primary)
        //    {
        //        return dialog.Items.Where(p => p.IsChecked).ToList().AsReadOnly();
        //    }
        //    return null;
        //}



    }

    public class CommonDialogButtonGridVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}