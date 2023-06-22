using Microsoft.Maui.Controls;
using TaskManager.ViewModels;

namespace TaskManager.Views;

public partial class MainPage : ContentPage
{
	private readonly MainPageViewModel _viewModel;

	public MainPage(MainPageViewModel mainPageViewModel)
	{
		InitializeComponent();

		BindingContext = mainPageViewModel;
		_viewModel = mainPageViewModel;
	}

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

		_viewModel.IsBusy = false;
    }
}