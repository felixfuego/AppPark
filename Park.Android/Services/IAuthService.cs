using Park.Maui.Models;

namespace Park.Maui.Services
{
    public interface IAuthService
    {
        Task<LoginResponse?> LoginAsync(string username, string password);
        Task<bool> IsAuthenticatedAsync();
        Task<string?> GetTokenAsync();
        Task<UserInfo?> GetCurrentUserAsync();
        Task LogoutAsync();
        Task<bool> RefreshTokenAsync();
    }
}
