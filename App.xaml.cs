using MauiApp1.Views;
using System.Diagnostics;

namespace MauiApp1;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        AppDomain.CurrentDomain.UnhandledException += (s, e) =>
        {
            Debug.WriteLine($"CRASH: {e.ExceptionObject}");
        };
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        try
        {
            return new Window(new NavigationPage(new LoginPage()));
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"WINDOW CRASH: {ex}");
            throw;
        }
    }
}