using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.ApplicationModel.Communication;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using TaskManager.Services;
using TaskManager.ViewModels;
using TaskManager.Views;

namespace TaskManager;

public static class MauiProgram
{
    private const string APP_ACTION_ID_OVERVIEW = "overview";
    private const string APP_ACTION_ID_ABOUT = "about";

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .ConfigureEssentials(essentials => essentials.UseVersionTracking());

        RegisterDepedencies(builder);

        if (AppActions.Current.IsSupported)
        {
            builder.ConfigureEssentials(essentials =>
            {
                essentials
                    .AddAppAction(APP_ACTION_ID_OVERVIEW, "Overview", icon: "appicon")
                    .AddAppAction(APP_ACTION_ID_ABOUT, "About", icon: "appicon")
                    .OnAppAction(HandleAppActions);
            });
        }

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }

    public static void HandleAppActions(AppAction appAction)
    {
        Application.Current.Dispatcher.Dispatch(async () =>
        {
            Page page = appAction.Id switch
            {
                APP_ACTION_ID_OVERVIEW => new MainPage(new MainPageViewModel(new PermissionService(new AlertService()))),
                APP_ACTION_ID_ABOUT => new AboutPage(new AboutPageViewModel(new AlertService())),
                _ => new MainPage(new MainPageViewModel(new PermissionService(new AlertService())))
            };

            // TODO: add shell navigation?
            await Application.Current.MainPage.Navigation.PopToRootAsync();
            await Application.Current.MainPage.Navigation.PushAsync(page);
        });
    }

    public static void RegisterDepedencies(MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<IAlertService, AlertService>();
        builder.Services.AddSingleton<IPermissionService, PermissionService>();
        builder.Services.AddSingleton(Contacts.Default);

        builder.Services.AddSingleton<BaseViewModel>();

        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<MainPageViewModel>();

        builder.Services.AddSingleton<AboutPage>();
        builder.Services.AddSingleton<AboutPageViewModel>();

        builder.Services.AddSingleton<ContactPage>();
        builder.Services.AddSingleton<ContactPageViewModel>();
    }
}