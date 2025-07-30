using System;

namespace FzLib.Avalonia.Dialogs;

public class SelectDialogItem : DialogItemBase
{
    public SelectDialogItem(string title, string detail, Action selectAction) : base(title, detail)
    {
        SelectAction = selectAction;
    }

    public SelectDialogItem(string title, string detail = null) : base(title, detail)
    {
    }

    public Action SelectAction { get; set; }
}
