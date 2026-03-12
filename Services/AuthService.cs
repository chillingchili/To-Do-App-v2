using MauiApp1.Models;

namespace MauiApp1.Services;

public class AuthService
{
    private static AuthService? _instance;
    public static AuthService Instance => _instance ??= new AuthService();

    public UserClass? CurrentUser { get; private set; }
    public bool IsLoggedIn => CurrentUser is not null;

    public void SetUser(UserClass user) => CurrentUser = user;
    public void Logout() => CurrentUser = null;
}
