using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
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
            });

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

        RegisterDepedencies(builder);

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
                APP_ACTION_ID_OVERVIEW => new MainPage(new MainPageViewModel()),
                APP_ACTION_ID_ABOUT => new AboutPage(new AboutPageViewModel()),
                _ => new MainPage(new MainPageViewModel())
            };

            await Application.Current.MainPage.Navigation.PopToRootAsync();
            await Application.Current.MainPage.Navigation.PushAsync(page);
        });
    }

    public static void RegisterDepedencies(MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<BaseViewModel>();

        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<MainPageViewModel>();

        builder.Services.AddSingleton<AboutPage>();
        builder.Services.AddSingleton<AboutPageViewModel>();
    }
}