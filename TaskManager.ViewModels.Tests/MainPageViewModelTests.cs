using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Networking;
using TaskManager.Services;

namespace TaskManager.ViewModels.Tests;

public class MainPageViewModelTests
{
    private readonly MainPageViewModel _sut;

    private readonly IPermissionService _permissionServiceMock;
    private readonly IConnectivity _connectivityMock;
    private readonly IAlertService _alertServiceMock;

    public MainPageViewModelTests()
    {
        _permissionServiceMock = Substitute.For<IPermissionService>();
        _connectivityMock = Substitute.For<IConnectivity>();
        _alertServiceMock = Substitute.For<IAlertService>();

        _sut = new MainPageViewModel(_permissionServiceMock, _connectivityMock, _alertServiceMock);
    }

    [Fact]
    public async Task AddTodo_WithPermission_AddsTodoToCollection()
    {
        var todoName = "Test Todo";
        _sut.TodoNameEntry = todoName;
        _permissionServiceMock.HasPermission<Permissions.StorageWrite>().Returns(true);

        await _sut.AddTodo();

        Assert.Single(_sut.Todos);
        Assert.Equal(todoName, _sut.Todos[0].Name);
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
    public async Task LoadTodos_WithInternetAccess_SetsIsBusyToTrue()
    {
        _connectivityMock.NetworkAccess.Returns(NetworkAccess.Internet);

        await _sut.LoadTodos();

        // Assert
        Assert.True(_sut.IsBusy);
    }

    [Fact]
    public async Task LoadTodos_WithoutInternetAccess_DisplaysError()
    {
        _connectivityMock.NetworkAccess.Returns(NetworkAccess.None);

        await _sut.LoadTodos();

        await _alertServiceMock.Received(1).DisplayError("You need internet access for this feature");
    }
}