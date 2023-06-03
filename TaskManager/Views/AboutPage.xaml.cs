using Microsoft.Maui.Controls;
using TaskManager.ViewModels;

namespace TaskManager.Views;

public partial class AboutPage : ContentPage
{
	public AboutPage(AboutPageViewModel aboutPageViewModel)
	{
		InitializeComponent();
		BindingContext = aboutPageViewModel;
	}
}