using Park.Comun.DTOs;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Blazored.LocalStorage;

namespace Park.Web.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public UserService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        private async Task AddAuthHeaderAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            await AddAuthHeaderAsync();
            var response = await _httpClient.GetAsync("api/User");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<UserDto>>() ?? new List<UserDto>();
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            await AddAuthHeaderAsync();
            var response = await _httpClient.GetAsync($"api/User/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UserDto>();
            }
            return null;
        }

        public async Task<UserDto?> GetUserByUsernameAsync(string username)
        {
            await AddAuthHeaderAsync();
            var response = await _httpClient.GetAsync($"api/User/username/{username}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UserDto>();
            }
            return null;
        }

        public async Task<UserDto?> GetUserByEmailAsync(string email)
        {
            await AddAuthHeaderAsync();
            var response = await _httpClient.GetAsync($"api/User/email/{email}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UserDto>();
            }
            return null;
        }

        public async Task<UserDto> CreateUserAsync(RegisterDto user)
        {
            await AddAuthHeaderAsync();
            var response = await _httpClient.PostAsJsonAsync("api/User", user);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error {response.StatusCode}: {errorContent}");
            }
            
            return await response.Content.ReadFromJsonAsync<UserDto>() ?? new UserDto();
        }

        public async Task<UserDto> UpdateUserAsync(int id, UserDto user)
        {
            await AddAuthHeaderAsync();
            var response = await _httpClient.PutAsJsonAsync($"api/User/{id}", user);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<UserDto>() ?? new UserDto();
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            await AddAuthHeaderAsync();
            var response = await _httpClient.DeleteAsync($"api/User/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            await AddAuthHeaderAsync();
            var changePasswordDto = new ChangePasswordDto
            {
                CurrentPassword = currentPassword,
                NewPassword = newPassword,
                ConfirmNewPassword = newPassword
            };

            var response = await _httpClient.PostAsJsonAsync($"api/User/{userId}/change-password", changePasswordDto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ResetPasswordAsync(string email)
        {
            await AddAuthHeaderAsync();
            var response = await _httpClient.PostAsJsonAsync("api/User/reset-password", email);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> LockUserAsync(int userId)
        {
            await AddAuthHeaderAsync();
            var response = await _httpClient.PostAsync($"api/User/{userId}/lock", null);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UnlockUserAsync(int userId)
        {
            await AddAuthHeaderAsync();
            var response = await _httpClient.PostAsync($"api/User/{userId}/unlock", null);
            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<UserDto>> GetUsersByCompanyAsync(int companyId)
        {
            await AddAuthHeaderAsync();
            var response = await _httpClient.GetAsync($"api/Company/{companyId}/users");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<UserDto>>() ?? new List<UserDto>();
            }
            return new List<UserDto>();
        }

        public async Task<bool> AssignUserToCompanyAsync(int userId, int companyId)
        {
            await AddAuthHeaderAsync();
            var assignmentDto = new AssignUserToCompanyDto
            {
                UserId = userId,
                CompanyId = companyId
            };

            var response = await _httpClient.PostAsJsonAsync($"api/User/{userId}/assign-company", assignmentDto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveUserFromCompanyAsync(int userId, int companyId)
        {
            await AddAuthHeaderAsync();
            var response = await _httpClient.DeleteAsync($"api/User/{userId}/companies/{companyId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<CompanyDto>> GetUserCompaniesAsync(int userId)
        {
            await AddAuthHeaderAsync();
            var response = await _httpClient.GetAsync($"api/User/{userId}/companies");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<CompanyDto>>() ?? new List<CompanyDto>();
            }
            return new List<CompanyDto>();
        }

        // Métodos para manejo de zonas
        public async Task<bool> AssignUserToZoneAsync(int userId, int zoneId)
        {
            await AddAuthHeaderAsync();
            var assignmentDto = new AssignUserToZoneDto
            {
                UserId = userId,
                ZoneId = zoneId
            };

            var response = await _httpClient.PostAsJsonAsync($"api/User/{userId}/assign-zone", assignmentDto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveUserFromZoneAsync(int userId, int zoneId)
        {
            await AddAuthHeaderAsync();
            var response = await _httpClient.DeleteAsync($"api/User/{userId}/zones/{zoneId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<ZoneDto>> GetUserZonesAsync(int userId)
        {
            await AddAuthHeaderAsync();
            var response = await _httpClient.GetAsync($"api/User/{userId}/zones");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<ZoneDto>>() ?? new List<ZoneDto>();
            }
            return new List<ZoneDto>();
        }
    }
}
