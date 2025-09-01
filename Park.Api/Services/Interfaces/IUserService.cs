using Park.Comun.DTOs;
using Park.Comun.Models;

namespace Park.Api.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDto?> GetUserByIdAsync(int id);
        Task<UserDto?> GetUserByUsernameAsync(string username);
        Task<UserDto?> GetUserByEmailAsync(string email);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> CreateUserAsync(RegisterDto registerDto);
        Task<UserDto> UpdateUserAsync(int id, UserDto userDto);
        Task<bool> DeleteUserAsync(int id);
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
        Task<bool> ResetPasswordAsync(string email);
        Task<bool> LockUserAsync(int userId);
        Task<bool> UnlockUserAsync(int userId);
        Task<bool> AssignUserToCompanyAsync(int userId, int companyId);
        Task<bool> RemoveUserFromCompanyAsync(int userId, int companyId);
        Task<IEnumerable<CompanyDto>> GetUserCompaniesAsync(int userId);
        Task<bool> AssignUserToZoneAsync(int userId, int zoneId);
        Task<bool> RemoveUserFromZoneAsync(int userId, int zoneId);
        Task<IEnumerable<ZoneDto>> GetUserZonesAsync(int userId);
    }
}
