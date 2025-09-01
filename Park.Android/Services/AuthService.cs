using Newtonsoft.Json;
using Park.Maui.Models;
using Park.Maui.Utils;

namespace Park.Maui.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public AuthService()
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(Constants.RequestTimeout);
            _baseUrl = Constants.BaseApiUrl;
        }

        public async Task<LoginResponse?> LoginAsync(string username, string password)
        {
            try
            {
                var loginRequest = new LoginRequest
                {
                    Username = username,
                    Password = password
                };

                var json = JsonConvert.SerializeObject(loginRequest);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_baseUrl}{Constants.AuthEndpoint}", content);
                
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(responseContent);
                    
                    if (loginResponse?.Success == true)
                    {
                        // Guardar tokens y información del usuario
                        await SaveAuthDataAsync(loginResponse);
                        return loginResponse;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en login: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            try
            {
                var token = await GetTokenAsync();
                if (string.IsNullOrEmpty(token))
                    return false;

                // Verificar si el token no ha expirado
                var expiryString = await SecureStorage.Default.GetAsync(Constants.TokenExpiryKey);
                if (string.IsNullOrEmpty(expiryString))
                    return false;

                if (DateTime.TryParse(expiryString, out var expiry) && DateTime.UtcNow < expiry)
                    return true;

                // Token expirado, intentar refresh
                return await RefreshTokenAsync();
            }
            catch
            {
                return false;
            }
        }

        public async Task<string?> GetTokenAsync()
        {
            return await SecureStorage.Default.GetAsync(Constants.TokenKey);
        }

        public async Task<UserInfo?> GetCurrentUserAsync()
        {
            try
            {
                var userJson = await SecureStorage.Default.GetAsync(Constants.UserInfoKey);
                if (!string.IsNullOrEmpty(userJson))
                {
                    return JsonConvert.DeserializeObject<UserInfo>(userJson);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task LogoutAsync()
        {
            try
            {
                await SecureStorage.Default.SetAsync(Constants.TokenKey, string.Empty);
                await SecureStorage.Default.SetAsync(Constants.RefreshTokenKey, string.Empty);
                await SecureStorage.Default.SetAsync(Constants.UserInfoKey, string.Empty);
                await SecureStorage.Default.SetAsync(Constants.TokenExpiryKey, string.Empty);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en logout: {ex.Message}");
            }
        }

        public async Task<bool> RefreshTokenAsync()
        {
            try
            {
                var refreshToken = await SecureStorage.Default.GetAsync(Constants.RefreshTokenKey);
                if (string.IsNullOrEmpty(refreshToken))
                    return false;

                // Implementar refresh token si es necesario
                // Por ahora, simplemente retornamos false para forzar re-login
                return false;
            }
            catch
            {
                return false;
            }
        }

        private async Task SaveAuthDataAsync(LoginResponse loginResponse)
        {
            try
            {
                await SecureStorage.Default.SetAsync(Constants.TokenKey, loginResponse.Token);
                await SecureStorage.Default.SetAsync(Constants.RefreshTokenKey, loginResponse.RefreshToken);
                
                var userJson = JsonConvert.SerializeObject(loginResponse.User);
                await SecureStorage.Default.SetAsync(Constants.UserInfoKey, userJson);

                // Calcular expiración del token
                var expiry = DateTime.UtcNow.AddSeconds(loginResponse.ExpiresIn);
                await SecureStorage.Default.SetAsync(Constants.TokenExpiryKey, expiry.ToString("O"));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error guardando datos de auth: {ex.Message}");
            }
        }
    }
}
