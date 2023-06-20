using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace TaskManager.Services;

public interface ITaskManagerHttpClient
{
    Task<ResponseMessage<T>> Get<T>(string url);
}

public class TaskManagerHttpClient : ITaskManagerHttpClient
{
    private readonly HttpClient _httpClient;

    public TaskManagerHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ResponseMessage<T>> Get<T>(string url)
    {
        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            return new ResponseMessage<T>
            {
                Ok = false
            };
        }

        var data = await response.Content.ReadFromJsonAsync<T>();
        return new ResponseMessage<T>
        {
            Ok = true,
            Data = data
        };
    }
}

public class ResponseMessage<T>
{
    public bool Ok { get; set; }
    public T Data { get; set; }
}
