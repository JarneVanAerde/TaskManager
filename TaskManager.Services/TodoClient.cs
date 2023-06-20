using System;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Models;

namespace TaskManager.Services;

public interface ITodoClient
{
    Task<Todo[]> GetTodos();
}

public class TodoClient : ITodoClient
{
    private const string URL = "https://jsonplaceholder.typicode.com/todos";

    private readonly ITaskManagerHttpClient _httpClient;
    private readonly IAlertService _alertService;

    public TodoClient(ITaskManagerHttpClient httpClient, IAlertService alertService)
    {
        _httpClient = httpClient;
        _alertService = alertService;
    }

    public async Task<Todo[]> GetTodos()
    {
        var response = await _httpClient.Get<Todo[]>(URL);

        if (!response.Ok)
        {
            await _alertService.DisplayError("Something went wrong while fetching the todos");
            return Array.Empty<Todo>();
        }

        return response.Data.Take(5).ToArray();
    }
}