using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Services;
using System.Collections.ObjectModel;

namespace MauiApp1.ViewModels;

public partial class ToDoListViewModel : BaseViewModel
{
    [ObservableProperty]
    private ObservableCollection<ToDoClass> items = [];

    public async Task LoadItemsAsync()
    {
        IsBusy = true;
        try
        {
            var response = await AppServices.Api.GetToDoItemsAsync("active", AuthService.Instance.CurrentUserId);
            Items.Clear();
            if (response != null && response.status == 200)
            {
                var apiItems = response.GetItems();
                foreach (var apiItem in apiItems)
                {
                    Items.Add(new ToDoClass
                    {
                        id = apiItem.item_id,
                        item_name = apiItem.item_name ?? "",
                        item_description = apiItem.item_description ?? "",
                        status = apiItem.status ?? "active",
                        user_id = apiItem.user_id
                    });
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"API Load Error (To Do): {ex}");
            Items.Clear();
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task AddItemAsync()
    {
        await Shell.Current.GoToAsync("AddToDoPage");
    }
}
