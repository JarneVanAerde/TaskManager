using Microsoft.Maui.Controls;
using System.Threading.Tasks;

namespace TaskManager.Services;

public interface IAlertService
{
    Task DisplayInfo(string title, string message);
    Task DisplayError(string message);
}

// TODO: abstract away the shell in a shell service.
public class AlertService : IAlertService
{
    public async Task DisplayInfo(string title, string message)
    {
        await Shell.Current.DisplayAlert(title, message, "Ok");
    }

    public async Task DisplayError(string message)
    {
        await Shell.Current.DisplayAlert("Error!", message, "Ok");
    }
}