using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.Primitives;

namespace FzLib.Avalonia.Controls;

public class ProgressRingOverlay : TemplatedControl
{
    private IDisposable activeSubscription;
    private int activationVersion = 0;

    public static readonly StyledProperty<double> RingSizeProperty =
        AvaloniaProperty.Register<ProgressRingOverlay, double>(
            nameof(RingSize), defaultValue: 64d);

    public static readonly StyledProperty<bool> IsActiveProperty =
        ProgressRing.IsActiveProperty.AddOwner<ProgressRingOverlay>();

    public static readonly StyledProperty<bool> IsActualActiveProperty =
        AvaloniaProperty.Register<ProgressRingOverlay, bool>(
            nameof(IsActualActive));

    public static readonly StyledProperty<TimeSpan> DelayProperty =
        AvaloniaProperty.Register<ProgressRingOverlay, TimeSpan>(
            nameof(Delay), defaultValue: TimeSpan.FromSeconds(0.3));

    public TimeSpan Delay
    {
        get => GetValue(DelayProperty);
        set => SetValue(DelayProperty, value);
    }

    public bool IsActive
    {
        get => GetValue(IsActiveProperty);
        set => SetValue(IsActiveProperty, value);
    }

    public bool IsActualActive
    {
        get => GetValue(IsActualActiveProperty);
        private set => SetValue(IsActualActiveProperty, value);
    }

    public double RingSize
    {
        get => GetValue(RingSizeProperty);
        set => SetValue(RingSizeProperty, value);
    }

    private async void OnActiveChanged(bool value)
    {
        int currentVersion = ++activationVersion;

        if (value)
        {
            if (Delay > TimeSpan.Zero)
            {
                await Task.Delay(Delay);
            }

            // 如果中途被取消，就退出
            if (currentVersion != activationVersion || !IsActive)
            {
                return;
            }

            IsActualActive = true;
        }
        else
        {
            IsActualActive = false;
        }
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        activeSubscription = this.GetObservable(IsActiveProperty).Subscribe(OnActiveChanged);
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        activeSubscription?.Dispose();
    }
}