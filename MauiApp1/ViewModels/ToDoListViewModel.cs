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
        var userId = AuthService.Instance.CurrentUser!.id;
        var list = await AppServices.Database.GetToDoItemsAsync(userId, "pending");
        Items.Clear();
        foreach (var item in list)
            Items.Add(item);
    }

    [RelayCommand]
    private async Task AddItemAsync()
    {
        await Shell.Current.GoToAsync("AddToDoPage");
    }
}
