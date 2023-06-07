using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
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

            var options = new BrowserLaunchOptions
            {
                LaunchMode = BrowserLaunchMode.SystemPreferred,
                TitleMode = BrowserTitleMode.Show,
                PreferredToolbarColor = Colors.Orange
            };

            await Browser.Default.OpenAsync(uri, options);
        }
        catch
        {
            // TODO: abstract away the shell (search on the internet)
            await Shell.Current.DisplayAlert("Error!", "Unable to open the browser", "OK");
        }
    }
}