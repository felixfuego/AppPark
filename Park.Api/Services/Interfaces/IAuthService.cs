using Park.Comun.DTOs;

namespace Park.Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto refreshTokenDto);
        Task<bool> ValidateTokenAsync(string token);
        Task<bool> LogoutAsync(string token);
        Task<UserDto?> GetUserByIdAsync(int id);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
    }
}
