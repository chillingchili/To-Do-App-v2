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

        IsBusy = true;
        try
        {
            var response = await AppServices.Api.AddToDoAsync(
                Name.Trim(),
                Description.Trim(),
                AuthService.Instance.CurrentUserId);

            if (response?.status != 200)
            {
                await Shell.Current.DisplayAlert("Error", response?.message ?? "Could not add task.", "OK");
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
