using Microsoft.Maui.Controls;
using TaskManager.Views;

namespace TaskManager;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
		Routing.RegisterRoute(nameof(AboutPage), typeof(AboutPage));
    }
}
