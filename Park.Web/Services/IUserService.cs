using Park.Comun.DTOs;

namespace Park.Web.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto?> GetUserByIdAsync(int id);
        Task<UserDto?> GetUserByUsernameAsync(string username);
        Task<UserDto?> GetUserByEmailAsync(string email);
        Task<UserDto> CreateUserAsync(RegisterDto user);
        Task<UserDto> UpdateUserAsync(int id, UserDto user);
        Task<bool> DeleteUserAsync(int id);
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
        Task<bool> ResetPasswordAsync(string email);
        Task<bool> LockUserAsync(int userId);
        Task<bool> UnlockUserAsync(int userId);
        Task<IEnumerable<UserDto>> GetUsersByCompanyAsync(int companyId);
        Task<bool> AssignUserToCompanyAsync(int userId, int companyId);
        Task<bool> RemoveUserFromCompanyAsync(int userId, int companyId);
        Task<IEnumerable<CompanyDto>> GetUserCompaniesAsync(int userId);
        
        // Métodos para manejo de zonas
        Task<bool> AssignUserToZoneAsync(int userId, int zoneId);
        Task<bool> RemoveUserFromZoneAsync(int userId, int zoneId);
        Task<IEnumerable<ZoneDto>> GetUserZonesAsync(int userId);
    }
}
