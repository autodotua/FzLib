using System.Diagnostics.CodeAnalysis;

namespace FzLib.Avalonia.Dialogs;

public abstract class DialogItemBase
{
    public DialogItemBase(string title, string detail = null)
    {
        Title = title;
        Detail = detail;
    }

    public DialogItemBase()
    {
    }

    public string Detail { get; set; }
    public object Tag { get; set; }
    public string Title { get; set; }
}
