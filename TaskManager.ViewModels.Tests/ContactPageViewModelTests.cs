using Microsoft.Maui.ApplicationModel.Communication;
using Microsoft.Maui.ApplicationModel;
using TaskManager.Services;

namespace TaskManager.ViewModels.Tests;

public class ContactPageViewModelTests
{
    private readonly ContactPageViewModel _sut;
    private readonly IContacts _contactsMock;
    private readonly IEmail _emailMock;
    private readonly IPermissionService _permissionServiceMock;
    private readonly IAlertService _alertServiceMock;

    public ContactPageViewModelTests()
    {
        _contactsMock = Substitute.For<IContacts>();
        _emailMock = Substitute.For<IEmail>();
        _permissionServiceMock = Substitute.For<IPermissionService>();
        _alertServiceMock = Substitute.For<IAlertService>();

        _sut = new ContactPageViewModel(
            _contactsMock,
            _emailMock,
            _permissionServiceMock,
            _alertServiceMock);
    }

    [Fact]
    public async Task PickContact_WithMissingPermissions_ShouldNotPickContact()
    {
        _permissionServiceMock.HasPermission<Permissions.ContactsRead>().Returns(false);

        await _sut.PickContact();

        await _contactsMock.DidNotReceive().PickContactAsync();
        await _alertServiceMock.DidNotReceive().DisplayInfo(Arg.Any<string>(), Arg.Any<string>());
    }

    [Fact]
    public async Task PickContact_WithValidPermissions_ShouldDisplayInfo()
    {
        _permissionServiceMock.HasPermission<Permissions.ContactsRead>().Returns(true);

        var contact = new Contact
        {
            GivenName = "John Doe"
        };
        _contactsMock.PickContactAsync().Returns(contact);

        await _sut.PickContact();

        await _alertServiceMock.Received(1).DisplayInfo("Contact info", "Hello there John Doe!");
    }

    [Fact]
    public async Task PickContact_WithValidPermissionsButNullContact_ShouldNotDisplayInfo()
    {
        _permissionServiceMock.HasPermission<Permissions.ContactsRead>().Returns(true);
        _contactsMock.PickContactAsync().Returns((Contact)null);

        await _sut.PickContact();

        await _alertServiceMock.DidNotReceive().DisplayInfo(Arg.Any<string>(), Arg.Any<string>());
    }

    [Fact]
    public async Task GoToEmailClient_WithComposeSupported_ShouldComposeEmail()
    {
        _emailMock.IsComposeSupported.Returns(true);

        await _sut.GoToEmailClient();

        await _emailMock.Received(1).ComposeAsync(Arg.Is<EmailMessage>(m =>
            m.Subject == "Contact" &&
            m.BodyFormat == EmailBodyFormat.PlainText &&
            m.To != null &&
            m.To.Contains("jarne.vanaerde@hotmail.com")));
    }

    [Fact]
    public async Task GoToEmailClient_WithComposeNotSupported_ShouldDisplayError()
    {
        _emailMock.IsComposeSupported.Returns(false);

        await _sut.GoToEmailClient();

        await _alertServiceMock.Received(1).DisplayError("Email function is not supported on this device");
    }
}
