using Park.Web.Models;

namespace Park.Web.Services;

public interface IAuthService
{
    Task<AuthResponse?> LoginAsync(LoginModel loginModel);
    Task<bool> LogoutAsync();
    Task<AuthResponse?> RefreshTokenAsync();
    Task<UserInfo?> GetCurrentUserAsync();
    Task InitializeAsync();
    bool IsAuthenticated { get; }
    event Action<bool>? AuthenticationStateChanged;
}
