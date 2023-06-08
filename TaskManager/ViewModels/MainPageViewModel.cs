using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Devices;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.ViewModels;

public partial class MainPageViewModel : BaseViewModel
{
    public ObservableCollection<Todo> Todos { get; }

    private readonly IAlertService _alertService;

    [ObservableProperty]
    private string todoNameEntry;

    public MainPageViewModel(IAlertService alertService)
    {
        Title = "Overview";
        Todos = new ObservableCollection<Todo>();
        _alertService = alertService;
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
            await _alertService.DisplayInfo("Important!", "It is important that we get these permission, otherwise you will not be able to add a TODO.");
        }

        storageWritePermission = await Permissions.RequestAsync<Permissions.StorageWrite>();
        if (storageWritePermission != PermissionStatus.Granted)
        {
            await _alertService.DisplayInfo("Oops", "Unable to add todo due to missing permissions");
        }

        Todos.Add(new Todo
        {
            Name = TodoNameEntry
        });

        TodoNameEntry = string.Empty;

        // TODO: write the todo to the storage of the device.
    }
}