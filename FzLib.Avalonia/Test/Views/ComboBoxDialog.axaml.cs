using Avalonia.Controls;
using Avalonia.Interactivity;
using FzLib.Avalonia.Dialogs;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Styling;
using FzLib.Avalonia.Controls;

namespace FzLib.Avalonia.Test.Views;
public partial class ComboBoxDialog : DialogHost
{
    public ComboBoxDialog()
    {
        InitializeComponent();
    }

    protected override void OnCloseButtonClick()
    {
        base.OnCloseButtonClick();
        Close();
    }
}
