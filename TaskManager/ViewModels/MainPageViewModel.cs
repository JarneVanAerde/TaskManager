using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices;
using System.Threading.Tasks;

namespace TaskManager.ViewModels;

public partial class MainPageViewModel : BaseViewModel
{
    public MainPageViewModel()
    {
        Title = "Overview";
    }

    // TODO: Extract to permission service
    [RelayCommand]
    private async Task AddTodo()
    {
        var storageWritePermission = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
  
        var shouldShowExplanation =
            storageWritePermission == PermissionStatus.Denied &&
            DeviceInfo.Platform == DevicePlatform.Android &&
            Permissions.ShouldShowRationale<Permissions.StorageWrite>();
        if (shouldShowExplanation)
        {
            await Shell.Current.DisplayAlert("Important!", "It is important that we get these permission, otherwise you will not be able to add a TODO.", "OK");
        }

        storageWritePermission = await Permissions.RequestAsync<Permissions.StorageWrite>();

        if (storageWritePermission == PermissionStatus.Granted)
        {
            await Shell.Current.DisplayAlert("GRANTED!", "Permission granted!", "OK");
        }
        else
        {
            throw new Exception("You did not do what we asked...");
        }
    }
}