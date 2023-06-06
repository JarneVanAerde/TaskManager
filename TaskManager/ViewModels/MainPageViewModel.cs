using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using System.Threading.Tasks;

namespace TaskManager.ViewModels;

public partial class MainPageViewModel : BaseViewModel
{
    private const string PlayStoreURL = "https://play.google.com/store/apps/details?id=com.google.android.apps.tasks&hl=nl&gl=US";

    public MainPageViewModel()
    {
        Title = "Overview";
    }

    [RelayCommand]
    public async Task GoToPlayStore()
    {
        try
        {
            var uri = new Uri(PlayStoreURL);
            await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        }
        catch
        {
            await Shell.Current.DisplayAlert("Error!", "Unable to open the browser", "OK");
        }
    }
}