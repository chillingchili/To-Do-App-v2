using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Services;
using MauiApp1.Views;

namespace MauiApp1.ViewModels;

public partial class LoginViewModel : BaseViewModel
{
    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;

    [RelayCommand]
    private async Task LoginAsync()
    {
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            await ShowAlert("Missing Information", "Please enter your email and password.");
            return;
        }

        IsBusy = true;
        try
        {
            System.Diagnostics.Debug.WriteLine("Making API call...");
            var response = await AppServices.Api.SignInAsync(Email.Trim(), Password);
            System.Diagnostics.Debug.WriteLine($"Response: {response?.status}");
            
            if (response?.status != 200 || response?.data is null)
            {
                MainThread.BeginInvokeOnMainThread(async () => {
                    await ShowAlert("Login Failed", response?.message ?? "Invalid email or password.");
                });
                return;
            }

            AuthService.Instance.SetUser(response.data.id, response.data.fname ?? "", response.data.lname ?? "", response.data.email ?? "");
            System.Diagnostics.Debug.WriteLine("User set, navigating...");
            
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                Application.Current!.Windows[0].Page = new AppShell();
            });
            System.Diagnostics.Debug.WriteLine("Navigated.");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"ERROR: {ex}");
            MainThread.BeginInvokeOnMainThread(async () => {
                await ShowAlert("Error", ex.Message);
            });
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task GoToRegisterAsync()
    {
        await Application.Current!.Windows[0].Page!.Navigation.PushAsync(new RegisterPage());
    }

    private static Task ShowAlert(string title, string message)
        => Application.Current!.Windows[0].Page!.DisplayAlert(title, message, "OK");
}
