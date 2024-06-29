using System;

namespace FzLib.Avalonia.Messages
{
    public class CommonDialogMessage : DialogHostMessage
    {
        public enum CommonDialogType
        {
            Unknown,
            Ok,
            Error,
            YesNo
        }

        public string Detail { get; init; }
        public Exception Exception { get; init; }
        public string Message { get; init; }
        public string Title { get; init; }
        public CommonDialogType Type { get; init; }
    }
}
