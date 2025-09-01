using Park.Comun.Models;

namespace Park.Api.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
        string GenerateRefreshToken();
        bool ValidateToken(string token);
        int? GetUserIdFromToken(string token);
        string? GetUsernameFromToken(string token);
        string? GetRoleFromToken(string token);
    }
}
