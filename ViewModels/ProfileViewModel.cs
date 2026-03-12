using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Services;
using MauiApp1.Views;

namespace MauiApp1.ViewModels;

public partial class ProfileViewModel : BaseViewModel
{
    [ObservableProperty]
    private string userName = string.Empty;

    [ObservableProperty]
    private string userEmail = string.Empty;

    public void Initialize()
    {
        var user = AuthService.Instance.CurrentUser;
        if (user is not null)
        {
            UserName = user.name;
            UserEmail = user.email;
        }
    }

    [RelayCommand]
    private void Logout()
    {
        AuthService.Instance.Logout();
        Application.Current!.Windows[0].Page = new NavigationPage(new LoginPage());
    }
}
