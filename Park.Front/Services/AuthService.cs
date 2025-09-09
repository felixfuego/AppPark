using System.Text;
using System.Text.Json;
using Park.Comun.DTOs;

namespace Park.Front.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly JsonSerializerOptions _jsonOptions;

        public AuthService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public event Action<bool>? AuthStateChanged;

        public async Task<AuthResponseDto> LoginAsync(string username, string password)
        {
            try
            {
                var loginDto = new { username, password };
                var json = JsonSerializer.Serialize(loginDto, _jsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/auth/login", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var authResponse = JsonSerializer.Deserialize<AuthResponseDto>(responseContent, _jsonOptions);
                    
                    if (authResponse?.Success == true)
                    {
                        // Guardar token en localStorage
                        await _localStorage.SetItemAsync("authToken", authResponse.Token);
                        await _localStorage.SetItemAsync("refreshToken", authResponse.RefreshToken);
                        await _localStorage.SetItemAsync("user", authResponse.User);
                        await _localStorage.SetItemAsync("tokenExpiration", authResponse.Expiration.ToString());

                        // Notificar cambio de estado
                        AuthStateChanged?.Invoke(true);

                        return authResponse;
                    }
                }

                var errorResponse = JsonSerializer.Deserialize<AuthResponseDto>(responseContent, _jsonOptions);
                return new AuthResponseDto
                {
                    Success = false,
                    Message = errorResponse?.Message ?? "Error de autenticación"
                };
            }
            catch (Exception ex)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = $"Error de conexión: {ex.Message}"
                };
            }
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto model)
        {
            try
            {
                var registerDto = new
                {
                    username = model.Username,
                    email = model.Email,
                    password = model.Password,
                    confirmPassword = model.ConfirmPassword,
                    firstName = model.FirstName,
                    lastName = model.LastName,
                    roleIds = model.RoleIds
                };

                var json = JsonSerializer.Serialize(registerDto, _jsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/auth/register", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var authResponse = JsonSerializer.Deserialize<AuthResponseDto>(responseContent, _jsonOptions);
                    
                    if (authResponse?.Success == true)
                    {
                        // Guardar token en localStorage
                        await _localStorage.SetItemAsync("authToken", authResponse.Token);
                        await _localStorage.SetItemAsync("refreshToken", authResponse.RefreshToken);
                        await _localStorage.SetItemAsync("user", authResponse.User);
                        await _localStorage.SetItemAsync("tokenExpiration", authResponse.Expiration.ToString());

                        // Notificar cambio de estado
                        AuthStateChanged?.Invoke(true);

                        return authResponse;
                    }
                }

                var errorResponse = JsonSerializer.Deserialize<AuthResponseDto>(responseContent, _jsonOptions);
                return new AuthResponseDto
                {
                    Success = false,
                    Message = errorResponse?.Message ?? "Error en el registro"
                };
            }
            catch (Exception ex)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = $"Error de conexión: {ex.Message}"
                };
            }
        }

        public async Task<bool> LogoutAsync()
        {
            try
            {
                var token = await _localStorage.GetItemAsync<string>("authToken");
                if (!string.IsNullOrEmpty(token))
                {
                    var content = new StringContent($"\"{token}\"", Encoding.UTF8, "application/json");
                    await _httpClient.PostAsync("/api/auth/logout", content);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en logout: {ex.Message}");
            }
            finally
            {
                // Limpiar localStorage
                await _localStorage.RemoveItemAsync("authToken");
                await _localStorage.RemoveItemAsync("refreshToken");
                await _localStorage.RemoveItemAsync("user");
                await _localStorage.RemoveItemAsync("tokenExpiration");

                // Notificar cambio de estado
                AuthStateChanged?.Invoke(false);
            }

            return true;
        }

        public async Task<UserDto?> GetCurrentUserAsync()
        {
            try
            {
                var token = await GetValidTokenAsync();
                if (string.IsNullOrEmpty(token))
                    return null;

                _httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync("/api/auth/me");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<UserDto>(content, _jsonOptions);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error obteniendo usuario actual: {ex.Message}");
            }

            return null;
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            var token = await GetValidTokenAsync();
            return !string.IsNullOrEmpty(token);
        }

        public async Task<string?> GetValidTokenAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            var expiration = await _localStorage.GetItemAsync<string>("tokenExpiration");

            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(expiration))
                return null;

            if (DateTime.TryParse(expiration, out var expDate) && expDate <= DateTime.UtcNow.AddMinutes(5))
            {
                // Token expirado o próximo a expirar, intentar renovar
                var refreshToken = await _localStorage.GetItemAsync<string>("refreshToken");
                if (!string.IsNullOrEmpty(refreshToken))
                {
                    var newToken = await RefreshTokenAsync(token, refreshToken);
                    if (!string.IsNullOrEmpty(newToken))
                        return newToken;
                }

                // No se pudo renovar, limpiar tokens
                await LogoutAsync();
                return null;
            }

            return token;
        }

        private async Task<string?> RefreshTokenAsync(string token, string refreshToken)
        {
            try
            {
                var refreshDto = new { token, refreshToken };
                var json = JsonSerializer.Serialize(refreshDto, _jsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/auth/refresh-token", content);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var authResponse = JsonSerializer.Deserialize<AuthResponseDto>(responseContent, _jsonOptions);
                    
                    if (authResponse?.Success == true)
                    {
                        await _localStorage.SetItemAsync("authToken", authResponse.Token);
                        await _localStorage.SetItemAsync("refreshToken", authResponse.RefreshToken);
                        await _localStorage.SetItemAsync("tokenExpiration", authResponse.Expiration.ToString());
                        
                        return authResponse.Token;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error renovando token: {ex.Message}");
            }

            return null;
        }

        public async Task<UserDto?> GetStoredUserAsync()
        {
            return await _localStorage.GetItemAsync<UserDto>("user");
        }
    }
}
