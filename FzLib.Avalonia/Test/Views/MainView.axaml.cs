using Avalonia.Controls;
using Avalonia.Interactivity;
using FzLib.Avalonia.Dialogs;
using FzLib.Avalonia;
using System;

namespace FzLib.Avalonia.Test.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    private void DialogButton_Click(object sender, RoutedEventArgs e)
    {
        switch ((sender as Button).Tag as string)
        {
            case "1":
                this.GetWindow().ShowOkDialogAsync("标题", "信息正文");
                break;
            case "2":
                this.GetWindow().ShowOkDialogAsync("标题", "信息正文", "带很长很长很长很长很长很长很长很长很长很长内容的详情带很长很长内容的详情带很长很长内容的详情带很长很长内容的详情带很长很长内容的详情\r\n带很长很长内容的详情带很长很长内容的详情带很长很长内容的详情");
                break;
            case "3":
                this.GetWindow().ShowWarningDialogAsync("标题", "警告正文");
                break;   
            case "4":
                this.GetWindow().ShowErrorDialogAsync("标题", "错误正文");
                break;
            case "5":
                try
                {
                    _ = 1 / Array.Empty<int>().Length;
                }
                catch (Exception ex)
                {
                    this.GetWindow().ShowErrorDialogAsync("错误信息", ex);
                }

                break;
        }

    }
}
