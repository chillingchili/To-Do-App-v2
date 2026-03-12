using MauiApp1.ViewModels;

namespace MauiApp1.Views;

public partial class CompletedToDoPage : ContentPage
{
    private readonly CompletedToDoViewModel _vm;

    public CompletedToDoPage()
    {
        InitializeComponent();
        _vm = new CompletedToDoViewModel();
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
            AppServices.SelectedCompletedItem = item;
            await Shell.Current.GoToAsync("EditCompletedToDoPage");
        }
        CompletedListView.SelectedItem = null;
    }
}
