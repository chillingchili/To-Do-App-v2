

namespace MauiApp1.Services;

public class AuthService
{
    private static AuthService? _instance;
    public static AuthService Instance => _instance ??= new AuthService();

    public int CurrentUserId { get; private set; }
    public string? CurrentUserFirstName { get; private set; }
    public string? CurrentUserLastName { get; private set; }
    public string? CurrentUserEmail { get; private set; }
    public bool IsLoggedIn => CurrentUserId > 0;

    public void SetUser(int id, string firstName, string lastName, string email)
    {
        CurrentUserId = id;
        CurrentUserFirstName = firstName;
        CurrentUserLastName = lastName;
        CurrentUserEmail = email;
    }

    public void Logout()
    {
        CurrentUserId = 0;
        CurrentUserFirstName = null;
        CurrentUserLastName = null;
        CurrentUserEmail = null;
    }
}
