using MauiApp1.ViewModels;

namespace MauiApp1.Views;

public partial class EditCompletedToDoPage : ContentPage
{
    private readonly EditCompletedToDoViewModel _vm;

    public EditCompletedToDoPage()
    {
        InitializeComponent();
        _vm = new EditCompletedToDoViewModel();
        BindingContext = _vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _vm.Initialize();
    }
}
