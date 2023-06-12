using Microsoft.Maui.ApplicationModel;
using TaskManager.Services;
using TaskManager.ViewModels.Models;

namespace TaskManager.ViewModels.Tests;

public class MainPageViewModelTests
{
    private readonly MainPageViewModel _sut;
    private readonly IPermissionService _permissionServiceMock;

    public MainPageViewModelTests()
    {
        _permissionServiceMock = Substitute.For<IPermissionService>();
        _sut = new MainPageViewModel(_permissionServiceMock);
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
        // Arrange
        _sut.TodoNameEntry = "test Todo";
        _permissionServiceMock.HasPermission<Permissions.StorageWrite>().Returns(true);

        // Act
        await _sut.AddTodo();

        // Assert
        Assert.Equal(string.Empty, _sut.TodoNameEntry);
    }

    [Fact]
    public async Task AddTodo_WithoutPermission_DoesNotAddTodoToCollection()
    {
        // Arrange
        _permissionServiceMock.HasPermission<Permissions.StorageWrite>().Returns(false);

        // Act
        await _sut.AddTodo();

        // Assert
        Assert.Empty(_sut.Todos);
    }
}
