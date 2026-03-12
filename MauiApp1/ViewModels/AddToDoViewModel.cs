using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Services;

namespace MauiApp1.ViewModels;

public partial class AddToDoViewModel : BaseViewModel
{
    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private string description = string.Empty;

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            await Shell.Current.DisplayAlert("Required", "Task name cannot be empty.", "OK");
            return;
        }

        var item = new ToDoClass
        {
            item_name = Name.Trim(),
            item_description = Description.Trim(),
            status = "pending",
            user_id = AuthService.Instance.CurrentUser!.id
        };

        await AppServices.Database.InsertToDoAsync(item);
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private async Task CancelAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
