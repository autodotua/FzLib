using System.ComponentModel;

namespace FzLib.Avalonia.Controls;

public enum ItemsControlLayout
{
    [Description("垂直堆叠")]
    VerticalStack,

    [Description("水平堆叠")]
    HorizontalStack,

    [Description("垂直换行")]
    VerticalWrap,

    [Description("水平换行")]
    HorizontalWrap,

    [Description("均匀网格")]
    UniformGrid
}