using System.Net.Http.Json;
using System.Text.Json;
using Blazored.LocalStorage;
using Park.Web.Models;

namespace Park.Web.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private readonly ILogger<AuthService> _logger;
    
    private const string TokenKey = "authToken";
    private const string RefreshTokenKey = "refreshToken";
    private const string UserKey = "currentUser";
    
    public bool IsAuthenticated { get; private set; }
    public event Action<bool>? AuthenticationStateChanged;

    public AuthService(HttpClient httpClient, ILocalStorageService localStorage, ILogger<AuthService> logger)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
        _logger = logger;
    }

    public async Task<AuthResponse?> LoginAsync(LoginModel loginModel)
    {
        try
        {
            // Agregar headers de autorización si existe un token
            await AddAuthHeader();
            
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginModel);
            
            if (response.IsSuccessStatusCode)
            {
                var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
                if (authResponse != null && authResponse.Success)
                {
                    await StoreAuthData(authResponse);
                    IsAuthenticated = true;
                    AuthenticationStateChanged?.Invoke(true);
                    return authResponse;
                }
            }
            
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login");
            return null;
        }
    }

    public async Task<bool> LogoutAsync()
    {
        try
        {
            await _localStorage.RemoveItemAsync(TokenKey);
            await _localStorage.RemoveItemAsync(RefreshTokenKey);
            await _localStorage.RemoveItemAsync(UserKey);
            
            IsAuthenticated = false;
            AuthenticationStateChanged?.Invoke(false);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during logout");
            return false;
        }
    }

    public async Task<AuthResponse?> RefreshTokenAsync()
    {
        try
        {
            var refreshToken = await _localStorage.GetItemAsync<string>(RefreshTokenKey);
            if (string.IsNullOrEmpty(refreshToken))
                return null;

            var response = await _httpClient.PostAsJsonAsync("api/auth/refresh", new { RefreshToken = refreshToken });
            
            if (response.IsSuccessStatusCode)
            {
                var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
                if (authResponse != null)
                {
                    await StoreAuthData(authResponse);
                    return authResponse;
                }
            }
            
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error refreshing token");
            return null;
        }
    }

    public async Task<UserInfo?> GetCurrentUserAsync()
    {
        try
        {
            var userJson = await _localStorage.GetItemAsync<string>(UserKey);
            if (!string.IsNullOrEmpty(userJson))
            {
                return JsonSerializer.Deserialize<UserInfo>(userJson);
            }
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting current user");
            return null;
        }
    }

    private async Task StoreAuthData(AuthResponse authResponse)
    {
        await _localStorage.SetItemAsync(TokenKey, authResponse.Token);
        await _localStorage.SetItemAsync(RefreshTokenKey, authResponse.RefreshToken);
        await _localStorage.SetItemAsync(UserKey, JsonSerializer.Serialize(authResponse.User));
    }

    private async Task AddAuthHeader()
    {
        try
        {
            var token = await _localStorage.GetItemAsync<string>(TokenKey);
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding auth header");
        }
    }

    public async Task InitializeAsync()
    {
        try
        {
            var token = await _localStorage.GetItemAsync<string>(TokenKey);
            if (!string.IsNullOrEmpty(token))
            {
                // Verificar si el token no ha expirado
                var user = await GetCurrentUserAsync();
                if (user != null)
                {
                    IsAuthenticated = true;
                    AuthenticationStateChanged?.Invoke(true);
                    _logger.LogInformation("Usuario autenticado: {Username}", user.Username);
                }
                else
                {
                    // Token inválido, limpiar datos
                    await LogoutAsync();
                }
            }
            else
            {
                IsAuthenticated = false;
                AuthenticationStateChanged?.Invoke(false);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error initializing auth service");
            IsAuthenticated = false;
            AuthenticationStateChanged?.Invoke(false);
        }
    }
}
