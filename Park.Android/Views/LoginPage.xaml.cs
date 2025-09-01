using Park.Maui.Services;
using Park.Maui.Models;

namespace Park.Maui.Views;

public partial class LoginPage : ContentPage
{
    private readonly IAuthService _authService;

    public LoginPage()
    {
        InitializeComponent();
        _authService = new AuthService();
        BindingContext = this;
    }

    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool IsBusy { get; set; }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        await PerformLoginAsync();
    }

    private async Task PerformLoginAsync()
    {
        try
        {
            // Validar campos
            if (string.IsNullOrWhiteSpace(Username))
            {
                await DisplayAlert("Error", "Por favor ingrese su usuario", "OK");
                UsernameEntry.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                await DisplayAlert("Error", "Por favor ingrese su contraseña", "OK");
                PasswordEntry.Focus();
                return;
            }

            // Mostrar progreso
            IsBusy = true;
            OnPropertyChanged(nameof(IsBusy));

            // Realizar login
            var loginResponse = await _authService.LoginAsync(Username.Trim(), Password);

            if (loginResponse?.Success == true)
            {
                // Login exitoso
                await DisplayAlert("Éxito", "Login exitoso", "OK");
                
                // Verificar que sea un guardia
                var user = loginResponse.User;
                if (user?.Roles.Any(r => r.Name == "Guardia") == true)
                {
                    // Es un guardia, ir al panel principal
                    await Shell.Current.GoToAsync("//MainPage");
                }
                else
                {
                    await DisplayAlert("Error", "Acceso denegado. Solo guardias pueden usar esta aplicación.", "OK");
                    await _authService.LogoutAsync();
                }
            }
            else
            {
                await DisplayAlert("Error", "Usuario o contraseña incorrectos", "OK");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error en login: {ex.Message}");
            await DisplayAlert("Error", "Error de conexión. Verifique su internet.", "OK");
        }
        finally
        {
            IsBusy = false;
            OnPropertyChanged(nameof(IsBusy));
        }
    }
}
