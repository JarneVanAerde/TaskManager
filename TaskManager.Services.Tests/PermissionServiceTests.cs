using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Devices;

namespace TaskManager.Services.Tests;

public class PermissionServiceTests
{
    private IPermissionService _permissionService;
    private IAlertService _alertServiceMock;

    public PermissionServiceTests()
    {
        _alertServiceMock = Substitute.For<IAlertService>();
        _permissionService = new PermissionService(_alertServiceMock);
    }

    [Fact]
    public async Task HasPermission_WithPermissionGranted_ReturnsTrue()
    {
        await _alertServiceMock.DidNotReceiveWithAnyArgs().DisplayInfo(null, null);

        var hasPermission = await _permissionService.HasPermission<Permissions.Battery>();

        Assert.True(hasPermission);
    }

    //[Fact]
    //public async Task HasPermission_WithPermissionDenied_AndroidPlatform_ShouldShowExplanation_DisplaysAlert()
    //{
    //    // Arrange
    //    _alertServiceMock.DidNotReceiveWithAnyArgs().DisplayInfo(null, null);

    //    // Act
    //    var hasPermission = await _permissionService.HasPermission<Permissions.StorageWrite>();

    //    // Assert
    //    Assert.False(hasPermission);
    //    _alertServiceMock.Received(1).DisplayInfo("Important!", "You need permissions in order to complete this action");
    //}

    //[Fact]
    //public async Task HasPermission_WithPermissionDenied_NotAndroidPlatform_ShouldNotShowExplanation_DoesNotDisplayAlert()
    //{
    //    // Arrange
    //    DeviceInfo.Platform = DevicePlatform.iOS;
    //    _alertServiceMock.DidNotReceiveWithAnyArgs().DisplayInfo(null, null);

    //    // Act
    //    var hasPermission = await _permissionService.HasPermission<Permissions.StorageWrite>();

    //    // Assert
    //    Assert.False(hasPermission);
    //    _alertServiceMock.DidNotReceiveWithAnyArgs().DisplayInfo(null, null);
    //}

    //[Fact]
    //public async Task HasPermission_WithPermissionDenied_AndroidPlatform_ShouldNotShowExplanation_DoesNotDisplayAlert()
    //{
    //    // Arrange
    //    DeviceInfo.Platform = DevicePlatform.Android;
    //    _alertServiceMock.DidNotReceiveWithAnyArgs().DisplayInfo(null, null);

    //    // Act
    //    var hasPermission = await _permissionService.HasPermission<Permissions.Location>();

    //    // Assert
    //    Assert.False(hasPermission);
    //    _alertServiceMock.DidNotReceiveWithAnyArgs().DisplayInfo(null, null);
    //}
}
