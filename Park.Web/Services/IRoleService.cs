using Park.Comun.DTOs;

namespace Park.Web.Services
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDto>> GetAllRolesAsync();
        Task<RoleDto?> GetRoleByIdAsync(int id);
        Task<RoleDto?> GetRoleByNameAsync(string name);
        Task<RoleDto> CreateRoleAsync(CreateRoleDto role);
        Task<RoleDto> UpdateRoleAsync(int id, UpdateRoleDto role);
        Task<bool> DeleteRoleAsync(int id);
        Task<IEnumerable<RoleDto>> GetUserRolesAsync(int userId);
        Task<bool> AssignRoleToUserAsync(int userId, int roleId);
        Task<bool> RemoveRoleFromUserAsync(int userId, int roleId);
    }
}
