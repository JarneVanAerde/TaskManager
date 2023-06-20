using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TaskManager.Models;

namespace TaskManager.Services;

public interface ITodoClient
{
    Task<List<Todo>> GetTodos();
}

public class TodoClient : ITodoClient
{
    private const string URL = "https://jsonplaceholder.typicode.com/todos";
    private readonly HttpClient _httpClient;

    public TodoClient()
    {
        _httpClient = new HttpClient();
    }

    List<Todo> todoList = new();

    public async Task<List<Todo>> GetTodos()
    {
        // TODO: keep refreshing the page
        if (todoList.Count > 0)
            return todoList;

        var response = await _httpClient.GetAsync(URL);
        if (response.IsSuccessStatusCode)
        {
            todoList = await response.Content.ReadFromJsonAsync<List<Todo>>();
            todoList = todoList.Take(5).ToList();
        }

        return todoList;
    }
}