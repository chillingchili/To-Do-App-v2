using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Models;

namespace MauiApp1.ViewModels;

public partial class RegisterViewModel : BaseViewModel
{
    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;

    [ObservableProperty]
    private string confirmPassword = string.Empty;

    [RelayCommand]
    private async Task RegisterAsync()
    {
        if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Email) ||
            string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(ConfirmPassword))
        {
            await ShowAlert("Missing Information", "Please fill in all fields.");
            return;
        }

        if (Password != ConfirmPassword)
        {
            await ShowAlert("Password Mismatch", "Passwords do not match. Please try again.");
            return;
        }

        if (Password.Length < 6)
        {
            await ShowAlert("Weak Password", "Password must be at least 6 characters long.");
            return;
        }

        IsBusy = true;
        try
        {
            var existing = await AppServices.Database.GetUserAsync(Email.Trim().ToLower());
            if (existing is not null)
            {
                await ShowAlert("Email Taken", "An account with this email already exists.");
                return;
            }

            var user = new UserClass
            {
                name = Name.Trim(),
                email = Email.Trim().ToLower(),
                password_hash = HashHelper.Hash(Password)
            };

            await AppServices.Database.CreateUserAsync(user);
            await ShowAlert("Account Created", "Your account has been created! Please sign in.");
            await Application.Current!.Windows[0].Page!.Navigation.PopAsync();
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task GoBackAsync()
    {
        await Application.Current!.Windows[0].Page!.Navigation.PopAsync();
    }

    private static Task ShowAlert(string title, string message)
        => Application.Current!.Windows[0].Page!.DisplayAlert(title, message, "OK");
}
