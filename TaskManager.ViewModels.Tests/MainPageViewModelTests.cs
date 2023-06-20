using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Networking;
using System;
using System.Linq;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.ViewModels.Tests;

public class MainPageViewModelTests
{
    private readonly MainPageViewModel _sut;

    private readonly IPermissionService _permissionServiceMock;
    private readonly IConnectivity _connectivityMock;
    private readonly IAlertService _alertServiceMock;
    private readonly ITodoClient _todoClientMock;

    public MainPageViewModelTests()
    {
        _permissionServiceMock = Substitute.For<IPermissionService>();
        _connectivityMock = Substitute.For<IConnectivity>();
        _alertServiceMock = Substitute.For<IAlertService>();
        _todoClientMock = Substitute.For<ITodoClient>();

        _sut = new MainPageViewModel(_permissionServiceMock, _connectivityMock, _alertServiceMock, _todoClientMock);
    }

    [Fact]
    public async Task AddTodo_WithPermission_AddsTodoToCollection()
    {
        var todoTitle = "Test Todo";
        _sut.TodoNameEntry = todoTitle;
        _permissionServiceMock.HasPermission<Permissions.StorageWrite>().Returns(true);

        await _sut.AddTodo();

        Assert.Single(_sut.Todos);
        Assert.Equal(todoTitle, _sut.Todos[0].Title);
    }

    [Fact]
    public async Task AddTodo_WithPermission_ClearsTodoNameEntry()
    {
        _sut.TodoNameEntry = "test Todo";
        _permissionServiceMock.HasPermission<Permissions.StorageWrite>().Returns(true);

        await _sut.AddTodo();

        Assert.Equal(string.Empty, _sut.TodoNameEntry);
    }

    [Fact]
    public async Task AddTodo_WithoutPermission_DoesNotAddTodoToCollection()
    {
        _permissionServiceMock.HasPermission<Permissions.StorageWrite>().Returns(false);

        await _sut.AddTodo();

        Assert.Empty(_sut.Todos);
    }

    [Fact]
    public async Task LoadTodos_WithInternetAccess_CallsTodoClient()
    {
        _connectivityMock.NetworkAccess.Returns(NetworkAccess.Internet);
        _todoClientMock.GetTodos().Returns(Array.Empty<Todo>());

        await _sut.LoadTodos();

        await _todoClientMock.Received(1).GetTodos();
    }

    [Fact]
    public async Task LoadTodos_WithInternetAccess_SetsIsBusyBackToFalse()
    {
        _connectivityMock.NetworkAccess.Returns(NetworkAccess.Internet);
        _todoClientMock.GetTodos().Returns(Array.Empty<Todo>());

        await _sut.LoadTodos();

        Assert.False(_sut.IsBusy);
    }

    [Fact]
    public async Task LoadTodos_WithoutInternetAccess_DisplaysError()
    {
        _connectivityMock.NetworkAccess.Returns(NetworkAccess.None);

        await _sut.LoadTodos();

        await _alertServiceMock.Received(1).DisplayError("You need internet access for this feature");
    }
}