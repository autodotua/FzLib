using System;

namespace FzLib.Avalonia.Messages;

public class InputDialogMessage : DialogHostMessage
{
    public enum InputDialogType
    {
        Text,
        Integer,
        Float,
        Password,
        MultipleLinesText
    }
    public object DefaultValue { get; set; }
    public string Message { get; init; }
    public string Title { get; init; }
    public InputDialogType Type { get; set; }
    public string Watermark { get; set; }
    public Action<string> Validation { get; set; }
}