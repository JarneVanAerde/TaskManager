using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices.Sensors;
using System.Threading.Tasks;

namespace TaskManager.ViewModels;

public partial class AboutPageViewModel : BaseViewModel
{
    [ObservableProperty]
    private string name = AppInfo.Current.Name;

    [ObservableProperty]
    private string package = AppInfo.Current.PackageName;

    [ObservableProperty]
    private string version = AppInfo.Current.VersionString;

    [ObservableProperty]
    private string build = AppInfo.Current.BuildString;

    public AboutPageViewModel()
    {
        Title = "hello from about!";
    }

    [RelayCommand]   
    private void ShowAppSettings()
    {
        AppInfo.Current.ShowSettingsUI();
    }

    [RelayCommand]
    private async Task GoToMaps()
    {
        var location = new Location(51.3321142, 4.38072190733679);
        var options = new MapLaunchOptions { Name = "Home base" };

        try
        {
            await Map.Default.OpenAsync(location, options);
        }
        catch
        {
            await Shell.Current.DisplayAlert("Error!", "Unable to open the maps app", "OK");
        }
    }
}

