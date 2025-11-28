using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Park.Android.Services;
using Park.Android.Views;

namespace Park.Android.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly IAuthService _authService;

    [ObservableProperty]
    private string username = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    public LoginViewModel(IAuthService authService)
    {
        _authService = authService;
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Por favor ingrese usuario y contraseña";
            return;
        }

        IsLoading = true;
        ErrorMessage = string.Empty;

        try
        {
            Console.WriteLine($"[LoginViewModel] Intentando login con usuario: {Username}");
            
            // Verificar conectividad
            var current = Connectivity.Current.NetworkAccess;
            if (current != NetworkAccess.Internet)
            {
                ErrorMessage = "No hay conexión a Internet. Verifica tu conexión WiFi o datos móviles.";
                Console.WriteLine("[LoginViewModel] Sin conexión a Internet");
                return;
            }

            Console.WriteLine($"[LoginViewModel] Conectividad OK. Estado: {current}");
            
            var result = await _authService.LoginAsync(Username, Password);

            if (result != null && result.Success && result.User != null)
            {
                Console.WriteLine($"[LoginViewModel] Login exitoso. Usuario: {result.User.Username}");
                Console.WriteLine($"[LoginViewModel] Roles del usuario: {string.Join(", ", result.User.Roles.Select(r => r.Name))}");
                
                // Verificar que el usuario sea Guardia
                if (!result.User.Roles.Any(r => r.Name == "Guardia"))
                {
                    ErrorMessage = "Esta aplicación es solo para guardias de seguridad";
                    Console.WriteLine("[LoginViewModel] Usuario no tiene rol de Guardia");
                    await _authService.LogoutAsync();
                    return;
                }

                Console.WriteLine("[LoginViewModel] Navegando al Dashboard");
                // Navegar al Dashboard
                await Shell.Current.GoToAsync($"//{nameof(DashboardPage)}");
            }
            else
            {
                var message = result?.Message ?? "Usuario o contraseña incorrectos";
                ErrorMessage = message;
                Console.WriteLine($"[LoginViewModel] Login fallido: {message}");
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"[LoginViewModel] HttpRequestException: {ex.Message}");
            
            if (ex.Message.Contains("timeout") || ex.Message.Contains("Timeout") || ex.Message.Contains("tardó demasiado"))
            {
                ErrorMessage = "La conexión está tardando mucho. Verifica:\n" +
                             "1. Tu conexión a Internet\n" +
                             "2. Que el servidor esté disponible\n" +
                             "3. Intenta nuevamente";
            }
            else if (ex.Message.Contains("No such host") || ex.Message.Contains("Name or service not known"))
            {
                ErrorMessage = "No se puede conectar al servidor. Verifica tu conexión a Internet.";
            }
            else
            {
                ErrorMessage = $"Error de conexión: {ex.Message}";
            }
        }
        catch (TaskCanceledException ex)
        {
            Console.WriteLine($"[LoginViewModel] TaskCanceledException: {ex.Message}");
            ErrorMessage = "La solicitud tardó demasiado tiempo. Verifica tu conexión a Internet y el estado del servidor.";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[LoginViewModel] Exception: {ex.GetType().Name} - {ex.Message}");
            Console.WriteLine($"[LoginViewModel] StackTrace: {ex.StackTrace}");
            ErrorMessage = $"Error inesperado: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
            Console.WriteLine("[LoginViewModel] Login proceso finalizado");
        }
    }
}
