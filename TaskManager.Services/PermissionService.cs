using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Devices;
using System.Threading.Tasks;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace TaskManager.Services;

public interface IPermissionService
{
    Task<bool> HasPermission<TPermission>() where TPermission : BasePermission, new();
}

public class PermissionService : IPermissionService
{
    private readonly IAlertService _alertService;

    public PermissionService(IAlertService alertService)
    {
        _alertService = alertService;
    }

    public async Task<bool> HasPermission<TPermission>() where TPermission : BasePermission, new()
    {
        var storageWritePermission = await CheckStatusAsync<TPermission>();

        var shouldShowExplanation =
            storageWritePermission == PermissionStatus.Denied &&
            DeviceInfo.Platform == DevicePlatform.Android &&
            ShouldShowRationale<TPermission>();
        if (shouldShowExplanation)
        {
            await _alertService.DisplayInfo("Important!", "You need permissions in order to complete this action");
        }

        storageWritePermission = await RequestAsync<TPermission>();
        if (storageWritePermission != PermissionStatus.Granted)
        {
            await _alertService.DisplayInfo("Oops", "Unable to complete action due to missing permissions");
            return false;
        }

        return true;
    }
}
