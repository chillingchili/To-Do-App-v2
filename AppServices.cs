using MauiApp1.Services;

namespace MauiApp1;

/// <summary>
/// Static helpers shared across the app (database singleton + navigation data slot).
/// </summary>
public static class AppServices
{
    public static ApiService Api { get; } = new ApiService();

    // Temporarily holds the item being edited so Edit pages can pick it up.
    public static ToDoClass? SelectedToDoItem { get; set; }
    public static ToDoClass? SelectedCompletedItem { get; set; }
}
