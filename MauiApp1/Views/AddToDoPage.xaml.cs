using MauiApp1.ViewModels;

namespace MauiApp1.Views;

public partial class AddToDoPage : ContentPage
{
    public AddToDoPage()
    {
        InitializeComponent();
        BindingContext = new AddToDoViewModel();
    }
}
