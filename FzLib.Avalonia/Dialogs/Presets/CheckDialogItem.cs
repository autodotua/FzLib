using System;

namespace FzLib.Avalonia.Dialogs;

public class CheckDialogItem : DialogItemBase
{
    private bool isChecked = false;

    public CheckDialogItem(string title, string detail = null) : base(title, detail)
    {
    }

    public CheckDialogItem(string title, string detail, bool isEnabled, bool isChecked) : base(title, detail)
    {
        IsEnabled = isEnabled;
        IsChecked = isChecked;
    }

    public event EventHandler IsCheckedChanged;

    public bool IsChecked
    {
        get => isChecked;
        set
        {
            if (value != isChecked)
            {
                isChecked = value;
                IsCheckedChanged?.Invoke(this,EventArgs.Empty);
            }
        }
    }
    public bool IsEnabled { get; set; } = true;
}