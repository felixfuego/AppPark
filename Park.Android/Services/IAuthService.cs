using Park.Comun.DTOs;

namespace Park.Android.Services;

public interface IAuthService
{
    Task<AuthResponseDto?> LoginAsync(string username, string password);
    Task LogoutAsync();
    Task<bool> IsAuthenticatedAsync();
    Task<UserDto?> GetCurrentUserAsync();
    string? GetToken();
}
