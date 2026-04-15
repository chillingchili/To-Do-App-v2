using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Services;

namespace MauiApp1.ViewModels;

public partial class EditToDoViewModel : BaseViewModel
{
    private ToDoClass? _currentItem;

    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private string description = string.Empty;

    public void Initialize()
    {
        _currentItem = AppServices.SelectedToDoItem;
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

        IsBusy = true;
        try
        {
            var response = await AppServices.Api.UpdateToDoAsync(
                Name.Trim(),
                Description.Trim(),
                _currentItem.id);

            if (response?.status != 200)
            {
                await Shell.Current.DisplayAlert("Error", response?.message ?? "Could not update task.", "OK");
                return;
            }

            await Shell.Current.GoToAsync("..");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        if (_currentItem is null) return;

        bool confirmed = await Shell.Current.DisplayAlert(
            "Delete Task", "Are you sure you want to delete this task?", "Delete", "Cancel");
        if (!confirmed) return;

        IsBusy = true;
        try
        {
            var response = await AppServices.Api.DeleteToDoAsync(_currentItem.id);
            if (response?.status != 200)
            {
                await Shell.Current.DisplayAlert("Error", response?.message ?? "Could not delete task.", "OK");
                return;
            }

            await Shell.Current.GoToAsync("..");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task MarkCompleteAsync()
    {
        if (_currentItem is null) return;

        IsBusy = true;
        try
        {
            // API uses 'inactive' for completed, 'active' for pending
            var newStatus = _currentItem.status == "inactive" ? "active" : "inactive";
            var response = await AppServices.Api.ChangeToDoStatusAsync(newStatus, _currentItem.id);
            if (response?.status != 200)
            {
                await Shell.Current.DisplayAlert("Error", response?.message ?? "Could not change status.", "OK");
                return;
            }

            await Shell.Current.GoToAsync("..");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task CancelAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
