﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.Graphics;
using System.Threading.Tasks;
using TaskManager.Services;

namespace TaskManager.ViewModels;

public partial class AboutPageViewModel : BaseViewModel
{
    private const string PlayStoreURL = "https://play.google.com/store/apps/details?id=com.google.android.apps.tasks&hl=nl&gl=US";

    private readonly IAlertService _alertService;

    [ObservableProperty]
    private string name = AppInfo.Current.Name;

    [ObservableProperty]
    private string package = AppInfo.Current.PackageName;

    [ObservableProperty]
    private string version = AppInfo.Current.VersionString;

    [ObservableProperty]
    private string build = AppInfo.Current.BuildString;

    [ObservableProperty]
    private string firstLaunch = VersionTracking.IsFirstLaunchEver ? "YES!" : "New...";

    public AboutPageViewModel(IAlertService alertService)
    {
        Title = "About";
        _alertService = alertService;
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
            await _alertService.DisplayError("Unable to open the maps app");
        }
    }

    [RelayCommand]
    private async Task GoToPlayStore()
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
            await _alertService.DisplayError("Unable to open the browser");
        }
    }
}