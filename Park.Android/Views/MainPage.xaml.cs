using Park.Maui.Services;
using Park.Maui.Models;

namespace Park.Maui.Views;

public partial class MainPage : ContentPage
{
    private readonly IAuthService _authService;

    public MainPage()
    {
        InitializeComponent();
        _authService = new AuthService();
        LoadUserDataAsync();
    }

    private async void LoadUserDataAsync()
    {
        try
        {
            var currentUser = await _authService.GetCurrentUserAsync();
            if (currentUser != null)
            {
                WelcomeLabel.Text = $"Bienvenido, {currentUser.FullName}";
            }
            else
            {
                // Usuario no encontrado, volver al login
                await Shell.Current.GoToAsync("//LoginPage");
            }

            // TODO: Cargar estadísticas de visitas desde la API
            LoadVisitStatistics();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error cargando datos del usuario: {ex.Message}");
            await DisplayAlert("Error", "Error cargando datos del usuario", "OK");
        }
    }

    private void LoadVisitStatistics()
    {
        // TODO: Implementar carga de estadísticas desde la API
        // Por ahora, mostrar datos de ejemplo
        PendingVisitsLabel.Text = "0";
        InProgressVisitsLabel.Text = "0";
        CompletedVisitsLabel.Text = "0";
    }

    private async void OnViewVisitsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//VisitsPage");
    }

    private async void OnScanQrClicked(object sender, EventArgs e)
    {
        // TODO: Implementar escáner QR
        await DisplayAlert("Info", "Escáner QR - Próximamente", "OK");
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        try
        {
            // Mostrar diálogo de confirmación
            var result = await DisplayAlert("Confirmar", "¿Está seguro que desea cerrar sesión?", "Sí", "No");
            
            if (result)
            {
                await _authService.LogoutAsync();
                await Shell.Current.GoToAsync("//LoginPage");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error en logout: {ex.Message}");
            await DisplayAlert("Error", "Error al cerrar sesión", "OK");
        }
    }
}
