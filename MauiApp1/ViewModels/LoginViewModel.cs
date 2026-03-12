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
            var user = await AppServices.Database.GetUserAsync(Email.Trim().ToLower());
            if (user is null || user.password_hash != HashHelper.Hash(Password))
            {
                await ShowAlert("Login Failed", "Invalid email or password. Please try again.");
                return;
            }

            AuthService.Instance.SetUser(user);
            Application.Current!.Windows[0].Page = new AppShell();
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
