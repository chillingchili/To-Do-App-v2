using MauiApp1.Views;

namespace MauiApp1;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        // Always start at the Login page; shell is loaded after successful login.
        return new Window(new NavigationPage(new LoginPage()));
    }
}