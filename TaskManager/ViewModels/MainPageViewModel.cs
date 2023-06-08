using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TaskManager.Models;

namespace TaskManager.ViewModels;

public partial class MainPageViewModel : BaseViewModel
{
    [ObservableProperty]
    private string todoNameEntry;
    public ObservableCollection<Todo> Todos { get; }

    public MainPageViewModel()
    {
        Title = "Overview";
        Todos = new ObservableCollection<Todo>();
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
        if (storageWritePermission != PermissionStatus.Granted)
        {
            await Shell.Current.DisplayAlert("Oops", "Unable to add todo due to missing permissions", "OK");
        }

        Todos.Add(new Todo
        {
            Name = TodoNameEntry
        });

        TodoNameEntry = string.Empty;

        // TODO: write the todo to the storage of the device.
    }
}