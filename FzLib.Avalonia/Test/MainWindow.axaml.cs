﻿using Avalonia.Controls;
using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.ComponentModel;
using FzLib.Avalonia.Dialogs;
using System;

namespace FzLib.Avalonia.Test;


public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private string message;
}
public partial class MainWindow : Window
{
    private MainViewModel VM { get; }
    public MainWindow()
    {
        DataContext = VM = new MainViewModel();
        InitializeComponent();
    }
    private async void DialogButton_Click(object sender, RoutedEventArgs e)
    {
        switch ((sender as Button).Tag as string)
        {
            case "1":
                await this.GetWindow().ShowOkDialogAsync("标题", "信息正文");
                break;
            case "2":
                await this.GetWindow().ShowOkDialogAsync("标题", "信息正文", "带很长很长很长很长很长很长很长很长很长很长内容的详情带很长很长内容的详情带很长很长内容的详情带很长很长内容的详情带很长很长内容的详情\r\n带很长很长内容的详情带很长很长内容的详情带很长很长内容的详情");
                break;
            case "3":
                await this.GetWindow().ShowWarningDialogAsync("标题", "警告正文");
                break;
            case "4":
                await this.GetWindow().ShowErrorDialogAsync("标题", "错误正文");
                break;
            case "5":
                try
                {
                    _ = 1 / Array.Empty<int>().Length;
                }
                catch (Exception ex)
                {
                    while (await this.GetWindow().ShowErrorDialogAsync("错误信息", ex, true))
                    {

                    }
                }
                break;
            case "6":
                VM.Message = (await this.GetWindow().ShowYesNoDialogAsync("标题", "询问内容")).Value ? "单击“是”" : "单击“否”";
                break;
            case "7":
               switch (await this.GetWindow().ShowYesNoDialogAsync("标题", "询问内容", cancelButon: true))
                {
                    case true:
                        VM.Message = "单击“是”";
                        break;
                    case false:
                        VM.Message = "单击“否”";
                        break;
                    case null:
                        VM.Message = "单击“取消”";
                        break;
                }
                break;

        }

    }

}