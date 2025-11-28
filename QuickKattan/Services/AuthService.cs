using System.Text.Json;
using System.Text;
using Park.Comun.DTOs;

namespace QuickKattan.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenService _tokenService;

        public AuthService(HttpClient httpClient, ITokenService tokenService)
        {
            _httpClient = httpClient;
            _tokenService = tokenService;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            try
            {
                var json = JsonSerializer.Serialize(loginDto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("api/auth/login", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var authResponse = JsonSerializer.Deserialize<AuthResponseDto>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (authResponse?.Success == true && !string.IsNullOrEmpty(authResponse.Token))
                    {
                        await _tokenService.SetTokenAsync(authResponse.Token);
                        // Configurar el header de Authorization para futuras peticiones
                        _httpClient.DefaultRequestHeaders.Authorization = 
                            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authResponse.Token);
                    }

                    return authResponse ?? new AuthResponseDto { Success = false, Message = "Error en la respuesta del servidor" };
                }
                else
                {
                    var errorResponse = JsonSerializer.Deserialize<AuthResponseDto>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    return errorResponse ?? new AuthResponseDto { Success = false, Message = "Error de autenticación" };
                }
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

        public async Task LogoutAsync()
        {
            await _tokenService.RemoveTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            var token = await _tokenService.GetTokenAsync();
            if (string.IsNullOrEmpty(token))
                return false;

            return !_tokenService.IsTokenExpired(token);
        }

        public async Task<string?> GetCurrentUserAsync()
        {
            var token = await _tokenService.GetTokenAsync();
            if (string.IsNullOrEmpty(token))
                return null;

            // Aquí podrías decodificar el JWT para obtener el nombre del usuario
            // Por simplicidad, retornamos que está autenticado
            return "Usuario Autenticado";
        }
    }
}