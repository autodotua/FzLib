using Avalonia.Controls;
using Avalonia.Interactivity;
using FzLib.Avalonia.Dialogs;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Styling;
using FzLib.Avalonia.Controls;
using FzLib.Samples.ViewModels;

namespace FzLib.Samples.Views;
public partial class ComboBoxDialog : DialogHost
{
    public ComboBoxDialog()
    {
        DataContext = new ComboBoxDialogViewModel();
        InitializeComponent();
    }

    protected override void OnCloseButtonClick()
    {
        Close();
    }
}
