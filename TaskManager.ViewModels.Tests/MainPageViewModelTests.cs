using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Networking;
using System;
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

    [Theory]
    [InlineData("Test Todo")]
    [InlineData("Test Todo     ")]
    [InlineData("    Test Todo")]
    public async Task AddTodo_WithPermission_AddsTodoToCollection(string todoTitle)
    {
        _sut.TodoTitleEntry = todoTitle;
        _permissionServiceMock.HasPermission<Permissions.StorageWrite>().Returns(true);

        await _sut.AddTodo();

        Assert.Single(_sut.Todos);
        Assert.Equal("Test Todo", _sut.Todos[0].Title);
    }

    [Fact]
    public async Task AddTodo_WithPermission_ClearsTodoNameEntry()
    {
        _sut.TodoTitleEntry = "test Todo";
        _permissionServiceMock.HasPermission<Permissions.StorageWrite>().Returns(true);

        await _sut.AddTodo();

        Assert.Equal(string.Empty, _sut.TodoTitleEntry);
    }

    [Fact]
    public async Task AddTodo_WithoutPermission_DoesNotAddTodoToCollection()
    {
        _permissionServiceMock.HasPermission<Permissions.StorageWrite>().Returns(false);

        await _sut.AddTodo();

        Assert.Empty(_sut.Todos);
    }

    [Fact]
    public void IsTodoTitleEntryEnabled_TodoTitleEntryNotEmpty_IsTodoTitleEntryEnabledReturnsTrue()
    {
        _sut.IsTodoTitleEntryEnabled = false;

        _sut.TodoTitleEntry = "Something filled in";

        Assert.True(_sut.IsTodoTitleEntryEnabled);
    }

    [Fact]
    public void IsTodoTitleEntryEnabled_TodoTitleEntryEmpty_IsTodoTitleEntryEnabledReturnsFalse()
    {
        _sut.IsTodoTitleEntryEnabled = true;

        _sut.TodoTitleEntry = string.Empty;

        Assert.False(_sut.IsTodoTitleEntryEnabled);
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