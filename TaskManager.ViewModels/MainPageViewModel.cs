using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TaskManager.Services;
using Microsoft.Maui.Networking;
using TaskManager.Models;

namespace TaskManager.ViewModels;

public partial class MainPageViewModel : BaseViewModel
{
    public ObservableCollection<Todo> Todos { get; }

    private readonly IPermissionService _permissionService;
    private readonly IConnectivity _connectivity;
    private readonly IAlertService _alertService;
    private readonly ITodoClient _todoClient;

    [ObservableProperty]
    private string todoTitleEntry;

    [ObservableProperty]
    private bool isTodoTitleEntryEnabled;

    public MainPageViewModel(IPermissionService permissionService, IConnectivity connectivity, IAlertService alertService, ITodoClient todoClient)
    {
        Title = "Overview";
        Todos = new ObservableCollection<Todo>();

        _permissionService = permissionService;
        _connectivity = connectivity;
        _alertService = alertService;
        _todoClient = todoClient;
    }

    partial void OnTodoTitleEntryChanged(string oldValue, string newValue)
    {
        IsTodoTitleEntryEnabled = !string.IsNullOrWhiteSpace(newValue);
    }

    [RelayCommand]
    public async Task AddTodo()
    {
        var hasMissingPermissions = !await _permissionService.HasPermission<Permissions.StorageWrite>();
        if (hasMissingPermissions) return;

        Todos.Insert(0, new Todo
        {
            Title = TodoTitleEntry.Trim()
        });

        TodoTitleEntry = string.Empty;

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

        var todosFromClient = await _todoClient.GetTodos();
        foreach (var todo in todosFromClient)
        {
            Todos.Insert(0, todo);
        }

        IsBusy = false;
    }
}