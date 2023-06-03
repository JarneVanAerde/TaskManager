using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.ApplicationModel;

namespace TaskManager.ViewModels;

public partial class AboutPageViewModel : ObservableObject
{
    [ObservableProperty]
    private string name;

    [ObservableProperty]
    private string package = AppInfo.Current.PackageName;

    [ObservableProperty]
    private string version = AppInfo.Current.VersionString;

    [ObservableProperty]
    private string build = AppInfo.Current.BuildString;

    public AboutPageViewModel()
    {
        Name = AppInfo.Current.Name;
    }
}

