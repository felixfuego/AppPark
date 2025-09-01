using Microsoft.EntityFrameworkCore;
using Park.Api.Data;
using Park.Api.Services.Interfaces;
using Park.Comun.DTOs;
using Park.Comun.Models;

namespace Park.Api.Services
{
    public class RoleService : IRoleService
    {
        private readonly ParkDbContext _context;

        public RoleService(ParkDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
        {
            var roles = await _context.Roles
                .Where(r => r.IsActive)
                .Select(r => new RoleDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    IsActive = r.IsActive,
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync();

            return roles;
        }

        public async Task<RoleDto?> GetRoleByIdAsync(int id)
        {
            var role = await _context.Roles
                .Where(r => r.Id == id && r.IsActive)
                .Select(r => new RoleDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    IsActive = r.IsActive,
                    CreatedAt = r.CreatedAt
                })
                .FirstOrDefaultAsync();

            return role;
        }

        public async Task<RoleDto?> GetRoleByNameAsync(string name)
        {
            var role = await _context.Roles
                .Where(r => r.Name == name && r.IsActive)
                .Select(r => new RoleDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    IsActive = r.IsActive,
                    CreatedAt = r.CreatedAt
                })
                .FirstOrDefaultAsync();

            return role;
        }

        public async Task<RoleDto> CreateRoleAsync(CreateRoleDto createRoleDto)
        {
            // Verificar si el rol ya existe
            var existingRole = await _context.Roles
                .FirstOrDefaultAsync(r => r.Name == createRoleDto.Name);

            if (existingRole != null)
            {
                throw new InvalidOperationException($"El rol '{createRoleDto.Name}' ya existe.");
            }

            var role = new Role
            {
                Name = createRoleDto.Name,
                Description = createRoleDto.Description,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Roles.Add(role);
            await _context.SaveChangesAsync();

            return new RoleDto
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description,
                IsActive = role.IsActive,
                CreatedAt = role.CreatedAt
            };
        }

        public async Task<RoleDto?> UpdateRoleAsync(int id, UpdateRoleDto updateRoleDto)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null || !role.IsActive)
            {
                return null;
            }

            // Verificar si el nuevo nombre ya existe en otro rol
            var existingRole = await _context.Roles
                .FirstOrDefaultAsync(r => r.Name == updateRoleDto.Name && r.Id != id);

            if (existingRole != null)
            {
                throw new InvalidOperationException($"El rol '{updateRoleDto.Name}' ya existe.");
            }

            role.Name = updateRoleDto.Name;
            role.Description = updateRoleDto.Description;
            role.IsActive = updateRoleDto.IsActive;
            role.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new RoleDto
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description,
                IsActive = role.IsActive,
                CreatedAt = role.CreatedAt
            };
        }

        public async Task<bool> DeleteRoleAsync(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null || !role.IsActive)
            {
                return false;
            }

            // Verificar si el rol está asignado a algún usuario
            var hasUsers = await _context.UserRoles
                .AnyAsync(ur => ur.RoleId == id && ur.IsActive);

            if (hasUsers)
            {
                throw new InvalidOperationException("No se puede eliminar un rol que está asignado a usuarios.");
            }

            role.IsActive = false;
            role.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AssignRoleToUserAsync(int userId, int roleId)
        {
            // Verificar que el usuario y el rol existen
            var user = await _context.Users.FindAsync(userId);
            var role = await _context.Roles.FindAsync(roleId);

            if (user == null || !user.IsActive || role == null || !role.IsActive)
            {
                return false;
            }

            // Verificar si ya tiene el rol asignado
            var existingUserRole = await _context.UserRoles
                .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);

            if (existingUserRole != null)
            {
                if (existingUserRole.IsActive)
                {
                    return true; // Ya tiene el rol asignado
                }
                else
                {
                    // Reactivar el rol
                    existingUserRole.IsActive = true;
                    existingUserRole.UpdatedAt = DateTime.UtcNow;
                }
            }
            else
            {
                // Crear nueva asignación
                var userRole = new UserRole
                {
                    UserId = userId,
                    RoleId = roleId,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                _context.UserRoles.Add(userRole);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveRoleFromUserAsync(int userId, int roleId)
        {
            var userRole = await _context.UserRoles
                .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId && ur.IsActive);

            if (userRole == null)
            {
                return false;
            }

            userRole.IsActive = false;
            userRole.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<RoleDto>> GetUserRolesAsync(int userId)
        {
            var roles = await _context.UserRoles
                .Where(ur => ur.UserId == userId && ur.IsActive)
                .Include(ur => ur.Role)
                .Where(ur => ur.Role.IsActive)
                .Select(ur => new RoleDto
                {
                    Id = ur.Role.Id,
                    Name = ur.Role.Name,
                    Description = ur.Role.Description,
                    IsActive = ur.Role.IsActive,
                    CreatedAt = ur.Role.CreatedAt
                })
                .ToListAsync();

            return roles;
        }

        public async Task<bool> UserHasRoleAsync(int userId, string roleName)
        {
            var hasRole = await _context.UserRoles
                .Where(ur => ur.UserId == userId && ur.IsActive)
                .Include(ur => ur.Role)
                .AnyAsync(ur => ur.Role.Name == roleName && ur.Role.IsActive);

            return hasRole;
        }

        public async Task<bool> UserHasAnyRoleAsync(int userId, params string[] roleNames)
        {
            var hasAnyRole = await _context.UserRoles
                .Where(ur => ur.UserId == userId && ur.IsActive)
                .Include(ur => ur.Role)
                .AnyAsync(ur => roleNames.Contains(ur.Role.Name) && ur.Role.IsActive);

            return hasAnyRole;
        }
    }
}
