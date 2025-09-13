using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace FzLib.Samples.ViewModels;

public partial class ComboBoxDialogViewModel : ObservableObject
{
    [ObservableProperty]
    private string message;

    [ObservableProperty]
    private int selectedIndex;

    [RelayCommand]
    private void PrimaryButtonClick()
    {
        Message = $"点击了主要按钮；选择了第{SelectedIndex}项";
    }

    [RelayCommand]
    private void SecondaryButtonClick()
    {
        Message = $"点击了次要按钮；选择了第{SelectedIndex}项";
    }
}