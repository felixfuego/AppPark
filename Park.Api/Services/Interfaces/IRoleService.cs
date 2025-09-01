using Park.Comun.DTOs;

namespace Park.Api.Services.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDto>> GetAllRolesAsync();
        Task<RoleDto?> GetRoleByIdAsync(int id);
        Task<RoleDto?> GetRoleByNameAsync(string name);
        Task<RoleDto> CreateRoleAsync(CreateRoleDto createRoleDto);
        Task<RoleDto?> UpdateRoleAsync(int id, UpdateRoleDto updateRoleDto);
        Task<bool> DeleteRoleAsync(int id);
        Task<bool> AssignRoleToUserAsync(int userId, int roleId);
        Task<bool> RemoveRoleFromUserAsync(int userId, int roleId);
        Task<IEnumerable<RoleDto>> GetUserRolesAsync(int userId);
        Task<bool> UserHasRoleAsync(int userId, string roleName);
        Task<bool> UserHasAnyRoleAsync(int userId, params string[] roleNames);
    }
}
