using Park.Comun.DTOs;
using System.Net.Http.Json;
using Blazored.LocalStorage;

namespace Park.Web.Services
{
    public class RoleService : IRoleService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public RoleService(HttpClient httpClient, ILocalStorageService localStorage)
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

        public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
        {
            await AddAuthHeaderAsync();
            var response = await _httpClient.GetAsync("api/Role");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<RoleDto>>() ?? new List<RoleDto>();
        }

        public async Task<RoleDto?> GetRoleByIdAsync(int id)
        {
            await AddAuthHeaderAsync();
            var response = await _httpClient.GetAsync($"api/Role/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<RoleDto>();
            }
            return null;
        }

        public async Task<RoleDto?> GetRoleByNameAsync(string name)
        {
            await AddAuthHeaderAsync();
            var response = await _httpClient.GetAsync($"api/Role/name/{name}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<RoleDto>();
            }
            return null;
        }

        public async Task<RoleDto> CreateRoleAsync(CreateRoleDto role)
        {
            await AddAuthHeaderAsync();
            var response = await _httpClient.PostAsJsonAsync("api/Role", role);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<RoleDto>() ?? new RoleDto();
        }

        public async Task<RoleDto> UpdateRoleAsync(int id, UpdateRoleDto role)
        {
            await AddAuthHeaderAsync();
            var response = await _httpClient.PutAsJsonAsync($"api/Role/{id}", role);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<RoleDto>() ?? new RoleDto();
        }

        public async Task<bool> DeleteRoleAsync(int id)
        {
            await AddAuthHeaderAsync();
            var response = await _httpClient.DeleteAsync($"api/Role/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<RoleDto>> GetUserRolesAsync(int userId)
        {
            await AddAuthHeaderAsync();
            var response = await _httpClient.GetAsync($"api/Role/user/{userId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<RoleDto>>() ?? new List<RoleDto>();
            }
            return new List<RoleDto>();
        }

        public async Task<bool> AssignRoleToUserAsync(int userId, int roleId)
        {
            await AddAuthHeaderAsync();
            var response = await _httpClient.PostAsync($"api/User/{userId}/roles/{roleId}", null);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveRoleFromUserAsync(int userId, int roleId)
        {
            await AddAuthHeaderAsync();
            var response = await _httpClient.DeleteAsync($"api/User/{userId}/roles/{roleId}");
            return response.IsSuccessStatusCode;
        }
    }
}
