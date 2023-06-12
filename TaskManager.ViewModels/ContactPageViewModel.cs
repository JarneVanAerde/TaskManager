using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.ApplicationModel.Communication;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Services;

namespace TaskManager.ViewModels;

public partial class ContactPageViewModel : BaseViewModel
{
    private readonly IContacts _contacts;
    private readonly IEmail _email;
    private readonly IPermissionService _permissionService;
    private readonly IAlertService _alertService;

    public ContactPageViewModel(
        IContacts contacts,
        IEmail email,
        IPermissionService permissionService,
        IAlertService alertService)
    {
        Title = "Contact";
        _contacts = contacts;
        _email = email;
        _permissionService = permissionService;
        _alertService = alertService;
    }

    [RelayCommand]
    public async Task PickContact()
    {
        var hasMissingPermissions = !await _permissionService.HasPermission<Permissions.ContactsRead>();
        if (hasMissingPermissions) return;

        var contact = await _contacts.PickContactAsync();
        if (contact == null) return;

        var displayName = contact.DisplayName;
        await _alertService.DisplayInfo("Contact info", $"Hello there {displayName}!");
    }


    [RelayCommand]
    public async Task GoToEmailClient()
    {
        if (!_email.IsComposeSupported)
        {
            await _alertService.DisplayError("Email function is not supported on this device");
        }

        string subject = "Contact";
        string[] recipients = new[] { "jarne.vanaerde@hotmail.com" };

        var message = new EmailMessage
        {
            Subject = subject,
            BodyFormat = EmailBodyFormat.PlainText,
            To = new List<string>(recipients)
        };

        await _email.ComposeAsync(message);
    }
}
