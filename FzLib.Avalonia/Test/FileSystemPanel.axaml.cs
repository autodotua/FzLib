using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Linq;
using System;
using FzLib.Avalonia.Test;
using FzLib.Avalonia.Dialogs;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace FzLib.Avalonia.Test;

public partial class FileSystemPanel : UserControl
{
    public FileSystemPanel()
    {
        InitializeComponent();
    }
}