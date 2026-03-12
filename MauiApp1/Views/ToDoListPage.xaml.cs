using MauiApp1.ViewModels;

namespace MauiApp1.Views;

public partial class ToDoListPage : ContentPage
{
    private readonly ToDoListViewModel _vm;

    public ToDoListPage()
    {
        InitializeComponent();
        _vm = new ToDoListViewModel();
        BindingContext = _vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.LoadItemsAsync();
    }

    private async void OnItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item is ToDoClass item)
        {
            AppServices.SelectedToDoItem = item;
            await Shell.Current.GoToAsync("EditToDoPage");
        }
        TaskListView.SelectedItem = null;
    }
}
