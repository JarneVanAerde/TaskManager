using Microsoft.Maui.Controls;
using TaskManager.ViewModels;

namespace TaskManager.Views;

public partial class ContactPage : ContentPage
{
	public ContactPage(ContactPageViewModel contactPageViewModel)
	{
		InitializeComponent();
		BindingContext = contactPageViewModel;
	}
}