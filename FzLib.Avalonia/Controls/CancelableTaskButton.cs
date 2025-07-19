using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Data.Converters;
using CommunityToolkit.Mvvm.Input;

namespace FzLib.Avalonia.Controls;

public class CancelableTaskButton : Button
{
    protected override Type StyleKeyOverride { get; } = typeof(Button);

    public static readonly StyledProperty<object> CancelContentProperty =
        AvaloniaProperty.Register<CancelableTaskButton, object>(nameof(CancelContent), "取消");

    public static readonly StyledProperty<object> ExecuteContentProperty =
        AvaloniaProperty.Register<CancelableTaskButton, object>(nameof(ExecuteContent), "执行");

    public static readonly StyledProperty<ICommand> CancelCommandProperty =
        AvaloniaProperty.Register<CancelableTaskButton, ICommand>(nameof(CancelCommand));

    public static readonly StyledProperty<IAsyncRelayCommand> TaskCommandProperty =
        AvaloniaProperty.Register<CancelableTaskButton, IAsyncRelayCommand>(nameof(TaskCommand));

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == TaskCommandProperty)
        {
            if (change.NewValue is IAsyncRelayCommand cmd)
            {
                Command = cmd;
                cmd.PropertyChanged += TaskCommandOnPropertyChanged;
            }

            if (change.OldValue is IAsyncRelayCommand old)
            {
                old.PropertyChanged -= TaskCommandOnPropertyChanged;
            }
        }
    }

    private void TaskCommandOnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(IAsyncRelayCommand.IsRunning))
        {
            return;
        }

        if (sender is not IAsyncRelayCommand cmd)
        {
            throw new ArgumentNullException(nameof(cmd));
        }
        if (cmd.IsRunning)
        {
            Command = CancelCommand;
            Content = CancelContent;
        }
        else
        {
            Command = TaskCommand;
            Content = ExecuteContent;
        }
    }

    public CancelableTaskButton()
    {
        Content = ExecuteContent;
    }

    public object CancelContent
    {
        get => GetValue(CancelContentProperty);
        set => SetValue(CancelContentProperty, value);
    }

    public object ExecuteContent
    {
        get => GetValue(ExecuteContentProperty);
        set => SetValue(ExecuteContentProperty, value);
    }

    public ICommand CancelCommand
    {
        get => GetValue(CancelCommandProperty);
        set => SetValue(CancelCommandProperty, value);
    }

    public IAsyncRelayCommand TaskCommand
    {
        get => GetValue(TaskCommandProperty);
        set => SetValue(TaskCommandProperty, value);
    }
}