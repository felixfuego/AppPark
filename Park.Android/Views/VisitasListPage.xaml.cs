using Park.Android.ViewModels;

namespace Park.Android.Views;

public partial class VisitasListPage : ContentPage
{
    private readonly VisitasListViewModel _viewModel;

    public VisitasListPage(VisitasListViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.InitializeAsync();
    }
}
