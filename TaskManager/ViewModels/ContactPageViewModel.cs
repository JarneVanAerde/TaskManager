using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.ApplicationModel.Communication;
using System.Threading.Tasks;
using TaskManager.Services;

namespace TaskManager.ViewModels;

public partial class ContactPageViewModel : BaseViewModel
{
    private readonly IContacts _contacts;
    private readonly IPermissionService _permissionService;
    private readonly IAlertService _alertService;

    public ContactPageViewModel(IContacts contacts, IPermissionService permissionService, IAlertService alertService)
    {
        Title = "Contact";
        _contacts = contacts;
        _permissionService = permissionService;
        _alertService = alertService;
    }

    [RelayCommand]
    private async Task PickContact()
    {
        var hasMissingPermissions = !await _permissionService.HasPermission<Permissions.ContactsRead>();
        if (hasMissingPermissions) return;

        var contact = await _contacts.PickContactAsync();
        if (contact == null) return;

        var displayName = contact.DisplayName;
        await _alertService.DisplayInfo("Contact info", $"Hello there {displayName}!");
    }
}
