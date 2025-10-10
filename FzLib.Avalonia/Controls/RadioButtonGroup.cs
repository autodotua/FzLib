using System;
using Avalonia.Controls;
using Avalonia.Input;

namespace FzLib.Avalonia.Controls;

public class RadioButtonGroup : ListBox
{
    protected override Type StyleKeyOverride { get; } = typeof(RadioButtonGroup);

    public RadioButtonGroup()
    {
        SelectionMode = SelectionMode.Single | SelectionMode.AlwaysSelected;
    }
}