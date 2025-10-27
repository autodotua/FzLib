using CommunityToolkit.Mvvm.ComponentModel;
using FzLib.Application;

namespace FzLib.Samples.ViewModels;

public partial class IdentityViewModel : ObservableObject
{
    [ObservableProperty]
    private string userId = IdentityProvider.GetUserId();

    [ObservableProperty]
    private string machineId = IdentityProvider.GetMachineId();

    [ObservableProperty]
    private string combinedId = IdentityProvider.GetCombinedId();
}
