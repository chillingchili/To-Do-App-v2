using MauiApp1.ViewModels;

namespace MauiApp1.Views;

public partial class ProfilePage : ContentPage
{
    private readonly ProfileViewModel _vm;

    public ProfilePage()
    {
        InitializeComponent();
        _vm = new ProfileViewModel();
        BindingContext = _vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _vm.Initialize();
    }
}
