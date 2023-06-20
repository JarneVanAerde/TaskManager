using TaskManager.Models;

namespace TaskManager.Services.Tests;

public class TodoClientTests
{
    private readonly ITaskManagerHttpClient _httpClientMock;
    private readonly IAlertService _alertServiceMock;

    private readonly TodoClient _sut;

    public TodoClientTests()
    {
        _httpClientMock = Substitute.For<ITaskManagerHttpClient>();
        _alertServiceMock = Substitute.For<IAlertService>();

        _sut = new TodoClient(_httpClientMock, _alertServiceMock);
    }

    [Fact]
    public async Task GetTodos_HttpClientResponseIsNotOk_ReturnsEmptyArray()
    {
        var response = new ResponseMessage<Todo[]>
        {
            Ok = false
        };
        _httpClientMock.Get<Todo[]>(Arg.Any<string>()).Returns(Task.FromResult(response));

        var todos = await _sut.GetTodos();

        Assert.Empty(todos);
        await _alertServiceMock.Received(1).DisplayError("Something went wrong while fetching the todos");
    }

    [Fact]
    public async Task GetTodos_HttpClientResponseIsOk_ReturnsLimitedArray()
    {
        var response = new ResponseMessage<Todo[]>
        {
            Ok = true,
            Data = new[]
            {
                new Todo { Title = "Todo 1" },
                new Todo { Title = "Todo 2" },
                new Todo { Title = "Todo 3" },
                new Todo { Title = "Todo 4" },
                new Todo { Title = "Todo 5" },
                new Todo { Title = "Todo 6" }
            }
        };
        _httpClientMock.Get<Todo[]>(Arg.Any<string>()).Returns(Task.FromResult(response));

        var todos = await _sut.GetTodos();

        Assert.Equal(5, todos.Length);
        Assert.Equal("Todo 1", todos[0].Title);
        Assert.Equal("Todo 2", todos[1].Title);
        Assert.Equal("Todo 3", todos[2].Title);
        Assert.Equal("Todo 4", todos[3].Title);
        Assert.Equal("Todo 5", todos[4].Title);
    }
}
