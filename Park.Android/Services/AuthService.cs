using Park.Comun.DTOs;

namespace Park.Android.Services;

public class AuthService : IAuthService
{
    private readonly IApiService _apiService;
    private readonly IStorageService _storageService;
    private UserDto? _currentUser;
    private string? _token;

    private const string TOKEN_KEY = "auth_token";
    private const string USER_KEY = "current_user";

    public AuthService(IApiService apiService, IStorageService storageService)
    {
        _apiService = apiService;
        _storageService = storageService;
    }

    public async Task<AuthResponseDto?> LoginAsync(string username, string password)
    {
        try
        {
            var loginRequest = new LoginDto
            {
                Username = username,
                Password = password
            };

            var response = await _apiService.PostAsync<AuthResponseDto>("api/auth/login", loginRequest);

            if (response != null && !string.IsNullOrEmpty(response.Token))
            {
                // Guardar token y usuario
                _token = response.Token;
                _currentUser = response.User;

                await _storageService.SetAsync(TOKEN_KEY, response.Token);
                await _storageService.SetAsync(USER_KEY, response.User);

                // Configurar token en ApiService
                _apiService.SetAuthToken(response.Token);

                return response;
            }

            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en LoginAsync: {ex.Message}");
            throw;
        }
    }

    public async Task LogoutAsync()
    {
        try
        {
            // Limpiar token y usuario
            _token = null;
            _currentUser = null;

            await _storageService.RemoveAsync(TOKEN_KEY);
            await _storageService.RemoveAsync(USER_KEY);

            _apiService.ClearAuthToken();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en LogoutAsync: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        try
        {
            var token = await _storageService.GetAsync<string>(TOKEN_KEY);
            
            if (!string.IsNullOrEmpty(token))
            {
                _token = token;
                _apiService.SetAuthToken(token);
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en IsAuthenticatedAsync: {ex.Message}");
            return false;
        }
    }

    public async Task<UserDto?> GetCurrentUserAsync()
    {
        try
        {
            if (_currentUser != null)
                return _currentUser;

            _currentUser = await _storageService.GetAsync<UserDto>(USER_KEY);
            return _currentUser;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en GetCurrentUserAsync: {ex.Message}");
            return null;
        }
    }

    public string? GetToken()
    {
        return _token;
    }
}
