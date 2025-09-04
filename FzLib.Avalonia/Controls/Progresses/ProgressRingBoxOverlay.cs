using System.Windows.Input;
using Avalonia;

namespace FzLib.Avalonia.Controls;

public class ProgressRingBoxOverlay : ProgressRingOverlay
{
    public static readonly StyledProperty<double> BoxHeightProperty =
        AvaloniaProperty.Register<ProgressRingBoxOverlay, double>(nameof(BoxHeight), double.NaN);

    public static readonly StyledProperty<double> BoxWidthProperty =
        AvaloniaProperty.Register<ProgressRingBoxOverlay, double>(nameof(BoxWidth), 240d);

    public static readonly StyledProperty<bool> CanCancelProperty =
        AvaloniaProperty.Register<ProgressRingBoxOverlay, bool>(nameof(CanCancel), false);

    public static readonly StyledProperty<ICommand> CancelCommandProperty =
        AvaloniaProperty.Register<ProgressRingBoxOverlay, ICommand>(nameof(CancelCommand));

    public static readonly StyledProperty<object> CancelContentProperty =
        AvaloniaProperty.Register<ProgressRingBoxOverlay, object>(nameof(CancelContent), "取消");

    public static readonly StyledProperty<string> MessageProperty =
        AvaloniaProperty.Register<ProgressRingBoxOverlay, string>(nameof(Message), "正在处理");

    public static readonly StyledProperty<string> TitleProperty =
                                AvaloniaProperty.Register<ProgressRingBoxOverlay, string>(nameof(Title), "请稍等");

    public double BoxHeight
    {
        get => GetValue(BoxHeightProperty);
        set => SetValue(BoxHeightProperty, value);
    }

    public double BoxWidth
    {
        get => GetValue(BoxWidthProperty);
        set => SetValue(BoxWidthProperty, value);
    }

    public bool CanCancel
    {
        get => GetValue(CanCancelProperty);
        set => SetValue(CanCancelProperty, value);
    }

    public ICommand CancelCommand
    {
        get => GetValue(CancelCommandProperty);
        set => SetValue(CancelCommandProperty, value);
    }

    public object CancelContent
    {
        get => GetValue(CancelContentProperty);
        set => SetValue(CancelContentProperty, value);
    }

    public string Message
    {
        get => GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }

    public string Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
}