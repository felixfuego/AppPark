using Park.Android.ViewModels;

namespace Park.Android.Views;

public partial class CheckOutPage : ContentPage
{
    private readonly CheckOutViewModel _viewModel;

    public CheckOutPage(CheckOutViewModel viewModel)
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
