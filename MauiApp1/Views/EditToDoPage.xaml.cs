using MauiApp1.ViewModels;

namespace MauiApp1.Views;

public partial class EditToDoPage : ContentPage
{
    private readonly EditToDoViewModel _vm;

    public EditToDoPage()
    {
        InitializeComponent();
        _vm = new EditToDoViewModel();
        BindingContext = _vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _vm.Initialize();
    }
}
