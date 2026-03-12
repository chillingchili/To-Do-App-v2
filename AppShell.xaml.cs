using MauiApp1.Views;

namespace MauiApp1;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Register modal/pushed routes (not top-level flyout items)
        Routing.RegisterRoute("AddToDoPage", typeof(AddToDoPage));
        Routing.RegisterRoute("EditToDoPage", typeof(EditToDoPage));
        Routing.RegisterRoute("EditCompletedToDoPage", typeof(EditCompletedToDoPage));
    }
}
