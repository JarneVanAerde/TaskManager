using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel;
using System.Threading.Tasks;

namespace TaskManager.ViewModels;

public partial class MainPageViewModel : BaseViewModel
{
    public MainPageViewModel()
    {
        Title = "Overview";
    }

    [RelayCommand]
    private async Task AddTodo()
    {
        var storageWritePermission = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
        if (storageWritePermission != PermissionStatus.Granted)
        {
            return;
        }
    }
}