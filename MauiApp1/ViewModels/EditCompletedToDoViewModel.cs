using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MauiApp1.ViewModels;

public partial class EditCompletedToDoViewModel : BaseViewModel
{
    private ToDoClass? _currentItem;

    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private string description = string.Empty;

    public void Initialize()
    {
        _currentItem = AppServices.SelectedCompletedItem;
        if (_currentItem is not null)
        {
            Name = _currentItem.item_name ?? string.Empty;
            Description = _currentItem.item_description ?? string.Empty;
        }
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (_currentItem is null) return;

        if (string.IsNullOrWhiteSpace(Name))
        {
            await Shell.Current.DisplayAlert("Required", "Task name cannot be empty.", "OK");
            return;
        }

        _currentItem.item_name = Name.Trim();
        _currentItem.item_description = Description.Trim();
        await AppServices.Database.UpdateToDoAsync(_currentItem);
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private async Task MoveToPendingAsync()
    {
        if (_currentItem is null) return;

        _currentItem.status = "pending";
        await AppServices.Database.UpdateToDoAsync(_currentItem);
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        if (_currentItem is null) return;

        bool confirmed = await Shell.Current.DisplayAlert(
            "Delete Task", "Are you sure you want to delete this task?", "Delete", "Cancel");
        if (!confirmed) return;

        await AppServices.Database.DeleteToDoAsync(_currentItem);
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private async Task CancelAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
