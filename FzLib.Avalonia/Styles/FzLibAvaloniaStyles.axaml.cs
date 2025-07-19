using System;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Styling;

namespace FzLib.Avalonia.Styles;

public class FzLibAvaloniaStyles : global::Avalonia.Styling.Styles
{
    public FzLibAvaloniaStyles(IServiceProvider sp = null)
    {
        AvaloniaXamlLoader.Load(sp, this);
    }
}