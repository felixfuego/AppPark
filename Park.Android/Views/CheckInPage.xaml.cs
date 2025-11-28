using Park.Android.ViewModels;

namespace Park.Android.Views;

public partial class CheckInPage : ContentPage
{
    private readonly CheckInViewModel _viewModel;

    public CheckInPage(CheckInViewModel viewModel)
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
