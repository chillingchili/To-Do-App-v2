using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Services;

namespace MauiApp1.ViewModels;

public partial class RegisterViewModel : BaseViewModel
{
    [ObservableProperty]
    private string firstName = string.Empty;

    [ObservableProperty]
    private string lastName = string.Empty;

    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;

    [ObservableProperty]
    private string confirmPassword = string.Empty;

    [RelayCommand]
    private async Task RegisterAsync()
    {
        if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName) ||
            string.IsNullOrWhiteSpace(Email) ||
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
            System.Diagnostics.Debug.WriteLine($"Attempting registration: {FirstName} {LastName} ({Email})");
            
            var response = await AppServices.Api.SignUpAsync(FirstName.Trim(), LastName.Trim(), Email.Trim(), Password);
            
            if (response?.status != 200)
            {
                MainThread.BeginInvokeOnMainThread(async () => {
                    await ShowAlert("Registration Failed", response?.message ?? "Could not create account.");
                });
                return;
            }

            MainThread.BeginInvokeOnMainThread(async () => {
                await ShowAlert("Account Created", "Your account has been created! Please sign in.");
                await Application.Current!.Windows[0].Page!.Navigation.PopAsync();
            });
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"REGISTRATION VIEWMODEL ERROR: {ex}");
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
    private async Task GoBackAsync()
    {
        await Application.Current!.Windows[0].Page!.Navigation.PopAsync();
    }

    private static Task ShowAlert(string title, string message)
        => Application.Current!.Windows[0].Page!.DisplayAlert(title, message, "OK");
}
