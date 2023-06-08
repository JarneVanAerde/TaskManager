using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.ViewModels;

public partial class MainPageViewModel : BaseViewModel
{
    public ObservableCollection<Todo> Todos { get; }

    private readonly IPermissionService _permissionService;

    [ObservableProperty]
    private string todoNameEntry;

    public MainPageViewModel(IPermissionService permissionService)
    {
        Title = "Overview";
        Todos = new ObservableCollection<Todo>();
        _permissionService = permissionService;
    }

    [RelayCommand]
    private async Task AddTodo()
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
}