using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel;

namespace TaskManager.ViewModels;

public partial class AboutPageViewModel : ObservableObject
{
    [ObservableProperty]
    private string name = AppInfo.Current.Name;

    [ObservableProperty]
    private string package = AppInfo.Current.PackageName;

    [ObservableProperty]
    private string version = AppInfo.Current.VersionString;

    [ObservableProperty]
    private string build = AppInfo.Current.BuildString;

    [RelayCommand]   
    private static void ShowAppSettings()
    {
        AppInfo.Current.ShowSettingsUI();
    }
}

