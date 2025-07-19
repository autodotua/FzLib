using System.ComponentModel;

namespace FzLib.Avalonia.Dialogs
{
    public enum DialogContainerType
    {
        [Description("内嵌")]
        Popup,
        [Description("窗口")]
        Window,
        [Description("内嵌优先")]
        PopupPreferred,
        [Description("窗口优先")]
        WindowPreferred,
        [Description("非模态窗口")]
        ModelessWindow
    }
}