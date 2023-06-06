using Microsoft.Maui.Controls;
using TaskManager.ViewModels;

namespace TaskManager.Views;

public partial class MainPage : ContentPage
{
	public MainPage(MainPageViewModel mainPageViewModel)
	{
		InitializeComponent();
		BindingContext = mainPageViewModel;
	}
}