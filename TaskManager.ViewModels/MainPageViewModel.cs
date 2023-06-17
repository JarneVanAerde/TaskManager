using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TaskManager.ViewModels.Models;
using TaskManager.Services;
using Microsoft.Maui.Networking;

namespace TaskManager.ViewModels;

public partial class MainPageViewModel : BaseViewModel
{
    public ObservableCollection<Todo> Todos { get; }

    private readonly IPermissionService _permissionService;
    private readonly IConnectivity _connectivity;
    private readonly IAlertService _alertService;

    [ObservableProperty]
    private string todoNameEntry;

    public MainPageViewModel(IPermissionService permissionService, IConnectivity connectivity, IAlertService alertService)
    {
        Title = "Overview";
        Todos = new ObservableCollection<Todo>();

        _permissionService = permissionService;
        _connectivity = connectivity;
        _alertService = alertService;
    }

    [RelayCommand]
    public async Task AddTodo()
    {
        var hasMissingPermissions = !await _permissionService.HasPermission<Permissions.StorageWrite>();
        if (hasMissingPermissions) return;

        Todos.Add(new Todo
        {
            Name = TodoNameEntry
        });

        TodoNameEntry = string.Empty;

        // TODO: write the todo to the storage of the device.
    }

    [RelayCommand]
    public async Task LoadTodos()
    {
        if (_connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            await _alertService.DisplayError("You need internet access for this feature");
            return;
        }

        IsBusy = true;
        // TODO: if we leave the page, we need to stop loading the todo's.
    }
}