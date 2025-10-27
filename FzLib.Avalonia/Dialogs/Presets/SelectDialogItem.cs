using System;
using System.Diagnostics.CodeAnalysis;

namespace FzLib.Avalonia.Dialogs;

public class SelectDialogItem : DialogItemBase
{
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(SelectDialogItem))]
    public SelectDialogItem(string title, string detail, Action selectAction) : base(title, detail)
    {
        SelectAction = selectAction;
    }

    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(SelectDialogItem))]
    public SelectDialogItem(string title, string detail = null) : base(title, detail)
    {
    }

    public Action SelectAction { get; set; }
}
