using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel;

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
    private static void ShowAppSettings()
    {
        AppInfo.Current.ShowSettingsUI();
    }
}

