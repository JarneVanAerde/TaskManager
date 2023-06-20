using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Networking;
using TaskManager.Services;
using TaskManager.ViewModels;
using TaskManager.Views;
using Communication = Microsoft.Maui.ApplicationModel.Communication;

namespace TaskManager;

public static class MauiProgram
{
    //private const string APP_ACTION_ID_OVERVIEW = "overview";
    //private const string APP_ACTION_ID_ABOUT = "about";

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

        // TODO: redo app actions
        //if (AppActions.Current.IsSupported)
        //{
        //    builder.ConfigureEssentials(essentials =>
        //    {
        //        essentials
        //            .AddAppAction(APP_ACTION_ID_OVERVIEW, "Overview", icon: "appicon")
        //            .AddAppAction(APP_ACTION_ID_ABOUT, "About", icon: "appicon")
        //            .OnAppAction(HandleAppActions);
        //    });
        //}

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }

    //public static void HandleAppActions(AppAction appAction)
    //{
    //    Application.Current.Dispatcher.Dispatch(async () =>
    //    {
    //        Page page = appAction.Id switch
    //        {
    //            APP_ACTION_ID_OVERVIEW => new MainPage(
    //                new MainPageViewModel(
    //                    new PermissionService(new AlertService()),
    //                    Connectivity.Current,
    //                    new AlertService())),
    //            APP_ACTION_ID_ABOUT => new AboutPage(
    //                new AboutPageViewModel(
    //                    new AlertService(),
    //                    AppInfo.Current,
    //                    VersionTracking.Default,
    //                    Map.Default,
    //                    Browser.Default)),
    //            _ => new MainPage(
    //                new MainPageViewModel(
    //                    new PermissionService(new AlertService()),
    //                    Connectivity.Current,
    //                    new AlertService()))
    //        };

    //        // TODO: add shell navigation?
    //        await Application.Current.MainPage.Navigation.PopToRootAsync();
    //        await Application.Current.MainPage.Navigation.PushAsync(page);
    //    });
    // }

    public static void RegisterDepedencies(MauiAppBuilder builder)
    {
        builder.Services.AddSingleton(Map.Default);
        builder.Services.AddSingleton(Browser.Default);
        builder.Services.AddSingleton(AppInfo.Current);
        builder.Services.AddSingleton(VersionTracking.Default);
        builder.Services.AddSingleton(Connectivity.Current);
        builder.Services.AddSingleton(Communication.Contacts.Default);
        builder.Services.AddSingleton(Communication.Email.Default);
        builder.Services.AddSingleton(Communication.PhoneDialer.Default);

        builder.Services.AddSingleton<IAlertService, AlertService>();
        builder.Services.AddSingleton<IPermissionService, PermissionService>();
        builder.Services.AddSingleton<ITaskManagerHttpClient, TaskManagerHttpClient>();
        builder.Services.AddSingleton<ITodoClient, TodoClient>();

        builder.Services.AddSingleton<BaseViewModel>();

        builder.Services.AddTransient<MainPage>();
        builder.Services.AddSingleton<MainPageViewModel>();

        builder.Services.AddTransient<AboutPage>();
        builder.Services.AddSingleton<AboutPageViewModel>();

        builder.Services.AddTransient<ContactPage>();
        builder.Services.AddSingleton<ContactPageViewModel>();
    }
}