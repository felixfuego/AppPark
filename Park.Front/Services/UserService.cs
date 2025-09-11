using System.Text;
using System.Text.Json;
using Park.Comun.DTOs;

namespace Park.Front.Services
{
    public class UserService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthService _authService;
        private readonly JsonSerializerOptions _jsonOptions;

        public UserService(HttpClient httpClient, AuthService authService)
        {
            _httpClient = httpClient;
            _authService = authService;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            try
            {
                var token = await _authService.GetValidTokenAsync();
                if (string.IsNullOrEmpty(token))
                    throw new UnauthorizedAccessException("No hay token válido");

                _httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync("/api/user");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<UserDto>>(content, _jsonOptions) ?? new List<UserDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error obteniendo usuarios: {ex.Message}");
                throw;
            }
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            try
            {
                var token = await _authService.GetValidTokenAsync();
                if (string.IsNullOrEmpty(token))
                    throw new UnauthorizedAccessException("No hay token válido");

                _httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync($"/api/user/{id}");
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return null;

                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<UserDto>(content, _jsonOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error obteniendo usuario: {ex.Message}");
                throw;
            }
        }

        public async Task<UserDto> CreateUserAsync(RegisterDto model)
        {
            try
            {
                var token = await _authService.GetValidTokenAsync();
                if (string.IsNullOrEmpty(token))
                    throw new UnauthorizedAccessException("No hay token válido");

                _httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

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

                var response = await _httpClient.PostAsync("/api/user", content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<UserDto>(responseContent, _jsonOptions) ?? 
                       throw new InvalidOperationException("No se pudo crear el usuario");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creando usuario: {ex.Message}");
                throw;
            }
        }

        public async Task<UserDto> UpdateUserAsync(int id, UpdateUserDto model)
        {
            try
            {
                var token = await _authService.GetValidTokenAsync();
                if (string.IsNullOrEmpty(token))
                    throw new UnauthorizedAccessException("No hay token válido");

                _httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var updateDto = new
                {
                    username = model.Username,
                    email = model.Email,
                    firstName = model.FirstName,
                    lastName = model.LastName,
                    isActive = model.IsActive
                };

                var json = JsonSerializer.Serialize(updateDto, _jsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"/api/user/{id}", content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<UserDto>(responseContent, _jsonOptions) ?? 
                       throw new InvalidOperationException("No se pudo actualizar el usuario");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error actualizando usuario: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            try
            {
                var token = await _authService.GetValidTokenAsync();
                if (string.IsNullOrEmpty(token))
                    throw new UnauthorizedAccessException("No hay token válido");

                _httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.DeleteAsync($"/api/user/{id}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<bool>(content, _jsonOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error eliminando usuario: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> LockUserAsync(int id)
        {
            try
            {
                var token = await _authService.GetValidTokenAsync();
                if (string.IsNullOrEmpty(token))
                    throw new UnauthorizedAccessException("No hay token válido");

                _httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PostAsync($"/api/user/{id}/lock", null);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<bool>(content, _jsonOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error bloqueando usuario: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UnlockUserAsync(int id)
        {
            try
            {
                var token = await _authService.GetValidTokenAsync();
                if (string.IsNullOrEmpty(token))
                    throw new UnauthorizedAccessException("No hay token válido");

                _httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PostAsync($"/api/user/{id}/unlock", null);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<bool>(content, _jsonOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error desbloqueando usuario: {ex.Message}");
                throw;
            }
        }

        public async Task<List<RoleDto>> GetRolesAsync()
        {
            try
            {
                var token = await _authService.GetValidTokenAsync();
                if (string.IsNullOrEmpty(token))
                    throw new UnauthorizedAccessException("No hay token válido");

                _httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync("/api/role");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<RoleDto>>(content, _jsonOptions) ?? new List<RoleDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error obteniendo roles: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto model)
        {
            try
            {
                var token = await _authService.GetValidTokenAsync();
                if (string.IsNullOrEmpty(token))
                    throw new UnauthorizedAccessException("No hay token válido");

                _httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var changePasswordDto = new
                {
                    currentPassword = model.CurrentPassword,
                    newPassword = model.NewPassword,
                    confirmNewPassword = model.ConfirmNewPassword
                };

                var json = JsonSerializer.Serialize(changePasswordDto, _jsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"/api/user/{userId}/change-password", content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<bool>(responseContent, _jsonOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error cambiando contraseña: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> AssignColaboradorToUserAsync(int userId, int colaboradorId)
        {
            try
            {
                var token = await _authService.GetValidTokenAsync();
                if (string.IsNullOrEmpty(token))
                    throw new UnauthorizedAccessException("No hay token válido");

                _httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PostAsync($"/api/user/{userId}/assign-colaborador/{colaboradorId}", null);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<bool>(content, _jsonOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error asignando colaborador: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> RemoveColaboradorFromUserAsync(int userId)
        {
            try
            {
                var token = await _authService.GetValidTokenAsync();
                if (string.IsNullOrEmpty(token))
                    throw new UnauthorizedAccessException("No hay token válido");

                _httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.DeleteAsync($"/api/user/{userId}/colaborador");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<bool>(content, _jsonOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removiendo colaborador: {ex.Message}");
                throw;
            }
        }
    }
}
