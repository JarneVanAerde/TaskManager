using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.Graphics;
using System.Threading.Tasks;
using TaskManager.Services;

namespace TaskManager.ViewModels;

public partial class AboutPageViewModel : BaseViewModel
{
    internal const string PlayStoreURL = "https://play.google.com/store/apps/details?id=com.google.android.apps.tasks&hl=nl&gl=US";

    private readonly IAlertService _alertService;
    private readonly IAppInfo _appInfo;
    private readonly IMap _map;
    private readonly IBrowser _browser;

    [ObservableProperty]
    private string name;

    [ObservableProperty]
    private string package;

    [ObservableProperty]
    private string version;

    [ObservableProperty]
    private string build;

    [ObservableProperty]
    private string firstLaunch;

    public AboutPageViewModel(
        IAlertService alertService,
        IAppInfo appInfo,
        IVersionTracking versionTracking,
        IMap map,
        IBrowser browser)
    {
        Title = "About";
        Name = appInfo.Name;
        Package = appInfo.PackageName;
        Version = appInfo.VersionString;
        Build = appInfo.BuildString;

        FirstLaunch = versionTracking.IsFirstLaunchEver ? "YES!" : "New...";

        _alertService = alertService;
        _appInfo = appInfo;
        _map = map;
        _browser = browser;
    }

    [RelayCommand]   
    public void ShowAppSettings()
    {
        _appInfo.ShowSettingsUI();
    }

    [RelayCommand]
    public async Task GoToMaps()
    {
        var location = new Location(51.3321142, 4.38072190733679);
        var options = new MapLaunchOptions { Name = "Home base" };

        try
        {
            await _map.OpenAsync(location, options);
        }
        catch
        {
            await _alertService.DisplayError("Unable to open the maps app");
        }
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

            await _browser.OpenAsync(uri, options);
        }
        catch
        {
            await _alertService.DisplayError("Unable to open the browser");
        }
    }
}