using Microsoft.EntityFrameworkCore;
using Park.Api.Data;
using Park.Api.Services.Interfaces;
using Park.Comun.DTOs;
using Park.Comun.Models;
using System.Security.Cryptography;
using System.Text;

namespace Park.Api.Services
{
    public class UserService : IUserService
    {
        private readonly ParkDbContext _context;

        public UserService(ParkDbContext context)
        {
            _context = context;
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Include(u => u.UserCompanies)
                    .ThenInclude(uc => uc.Company)
                        .ThenInclude(c => c.Zone)
                .Include(u => u.UserZones)
                    .ThenInclude(uz => uz.Zone)
                .FirstOrDefaultAsync(u => u.Id == id && u.IsActive);

            return user != null ? MapToDto(user) : null;
        }

        public async Task<UserDto?> GetUserByUsernameAsync(string username)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username && u.IsActive);

            return user != null ? MapToDto(user) : null;
        }

        public async Task<UserDto?> GetUserByEmailAsync(string email)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.IsActive);

            return user != null ? MapToDto(user) : null;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Include(u => u.UserCompanies)
                    .ThenInclude(uc => uc.Company)
                        .ThenInclude(c => c.Zone)
                .Include(u => u.UserZones)
                    .ThenInclude(uz => uz.Zone)
                .Where(u => u.IsActive)
                .ToListAsync();

            return users.Select(MapToDto);
        }

        public async Task<UserDto> CreateUserAsync(RegisterDto registerDto)
        {
            // Verificar si el usuario ya existe
            if (await _context.Users.AnyAsync(u => u.Username == registerDto.Username))
                throw new InvalidOperationException("El nombre de usuario ya existe");

            if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
                throw new InvalidOperationException("El email ya está registrado");

            var user = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                PasswordHash = HashPassword(registerDto.Password),
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Asignar roles si se especificaron
            if (registerDto.RoleIds != null && registerDto.RoleIds.Any())
            {
                foreach (var roleId in registerDto.RoleIds)
                {
                    var userRole = new UserRole
                    {
                        UserId = user.Id,
                        RoleId = roleId,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    };
                    _context.UserRoles.Add(userRole);
                }
                await _context.SaveChangesAsync();
            }

            return await GetUserByIdAsync(user.Id) ?? MapToDto(user);
        }

        public async Task<UserDto> UpdateUserAsync(int id, UserDto userDto)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.Id == id);
                
            if (user == null)
                throw new InvalidOperationException("Usuario no encontrado");

            // Verificar si el email ya existe en otro usuario
            if (await _context.Users.AnyAsync(u => u.Email == userDto.Email && u.Id != id))
                throw new InvalidOperationException("El email ya está registrado por otro usuario");

            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.Email = userDto.Email;
            user.IsActive = userDto.IsActive;
            user.UpdatedAt = DateTime.UtcNow;

            // Actualizar roles
            if (userDto.Roles != null)
            {
                // Obtener los IDs de roles del DTO
                var newRoleIds = userDto.Roles.Select(r => r.Id).ToList();
                
                // Obtener los roles actuales del usuario
                var currentRoleIds = user.UserRoles?.Where(ur => ur.IsActive).Select(ur => ur.RoleId).ToList() ?? new List<int>();
                
                // Roles a agregar
                var rolesToAdd = newRoleIds.Except(currentRoleIds);
                foreach (var roleId in rolesToAdd)
                {
                    var userRole = new UserRole
                    {
                        UserId = user.Id,
                        RoleId = roleId,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    };
                    _context.UserRoles.Add(userRole);
                }
                
                // Roles a desactivar
                var rolesToDeactivate = currentRoleIds.Except(newRoleIds);
                foreach (var roleId in rolesToDeactivate)
                {
                    var userRole = user.UserRoles?.FirstOrDefault(ur => ur.RoleId == roleId && ur.IsActive);
                    if (userRole != null)
                    {
                        userRole.IsActive = false;
                        userRole.UpdatedAt = DateTime.UtcNow;
                    }
                }
            }

            await _context.SaveChangesAsync();

            return await GetUserByIdAsync(id) ?? MapToDto(user);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return false;

            user.IsActive = false;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return false;

            if (!VerifyPassword(currentPassword, user.PasswordHash))
                return false;

            user.PasswordHash = HashPassword(newPassword);
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ResetPasswordAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return false;

            // Generar contraseña temporal
            var tempPassword = GenerateTempPassword();
            user.PasswordHash = HashPassword(tempPassword);
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // TODO: Enviar email con la contraseña temporal
            // Por ahora solo retornamos true
            return true;
        }

        public async Task<bool> LockUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return false;

            user.IsActive = false;
            user.LockoutEnd = DateTime.UtcNow.AddHours(24); // Bloquear por 24 horas
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UnlockUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return false;

            user.IsActive = true;
            user.LockoutEnd = null;
            user.LoginAttempts = 0;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AssignUserToCompanyAsync(int userId, int companyId)
        {
            // Verificar si el usuario y la empresa existen
            var user = await _context.Users.FindAsync(userId);
            var company = await _context.Companies.FindAsync(companyId);

            if (user == null || !user.IsActive || company == null || !company.IsActive)
            {
                return false;
            }

            // Verificar si ya existe la asignación
            var existingAssignment = await _context.UserCompanies
                .FirstOrDefaultAsync(uc => uc.UserId == userId && uc.CompanyId == companyId);

            if (existingAssignment != null)
            {
                if (existingAssignment.IsActive)
                {
                    throw new InvalidOperationException("El usuario ya está asignado a esta empresa.");
                }
                else
                {
                    // Reactivar la asignación existente
                    existingAssignment.IsActive = true;
                    existingAssignment.UpdatedAt = DateTime.UtcNow;
                }
            }
            else
            {
                var userCompany = new UserCompany
                {
                    UserId = userId,
                    CompanyId = companyId,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                _context.UserCompanies.Add(userCompany);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveUserFromCompanyAsync(int userId, int companyId)
        {
            var userCompany = await _context.UserCompanies
                .FirstOrDefaultAsync(uc => uc.UserId == userId && uc.CompanyId == companyId && uc.IsActive);

            if (userCompany == null)
            {
                return false;
            }

            userCompany.IsActive = false;
            userCompany.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<CompanyDto>> GetUserCompaniesAsync(int userId)
        {
            var userCompanies = await _context.UserCompanies
                .Include(uc => uc.Company)
                    .ThenInclude(c => c.Zone)
                .Where(uc => uc.UserId == userId && uc.IsActive && uc.Company.IsActive)
                .ToListAsync();

            return userCompanies.Select(uc => new CompanyDto
            {
                Id = uc.Company.Id,
                Name = uc.Company.Name,
                Description = uc.Company.Description,
                Address = uc.Company.Address,
                Phone = uc.Company.Phone,
                Email = uc.Company.Email,
                ContactPerson = uc.Company.ContactPerson,
                ContactPhone = uc.Company.ContactPhone,
                ContactEmail = uc.Company.ContactEmail,
                IsActive = uc.Company.IsActive,
                CreatedAt = uc.Company.CreatedAt,
                ZoneId = uc.Company.ZoneId,
                Zone = new ZoneDto
                {
                    Id = uc.Company.Zone.Id,
                    Name = uc.Company.Zone.Name,
                    Description = uc.Company.Zone.Description,
                    IsActive = uc.Company.Zone.IsActive,
                    CreatedAt = uc.Company.Zone.CreatedAt
                }
            });
        }



        // Métodos para UserZone
        public async Task<bool> AssignUserToZoneAsync(int userId, int zoneId)
        {
            // Verificar si el usuario y la zona existen
            var user = await _context.Users.FindAsync(userId);
            var zone = await _context.Zones.FindAsync(zoneId);

            if (user == null || !user.IsActive || zone == null || !zone.IsActive)
            {
                return false;
            }

            // Verificar si ya existe la asignación
            var existingAssignment = await _context.UserZones
                .FirstOrDefaultAsync(uz => uz.UserId == userId && uz.ZoneId == zoneId);

            if (existingAssignment != null)
            {
                if (existingAssignment.IsActive)
                {
                    throw new InvalidOperationException("El usuario ya está asignado a esta zona.");
                }
                else
                {
                    // Reactivar la asignación existente
                    existingAssignment.IsActive = true;
                    existingAssignment.UpdatedAt = DateTime.UtcNow;
                }
            }
            else
            {
                var userZone = new UserZone
                {
                    UserId = userId,
                    ZoneId = zoneId,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                _context.UserZones.Add(userZone);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveUserFromZoneAsync(int userId, int zoneId)
        {
            var userZone = await _context.UserZones
                .FirstOrDefaultAsync(uz => uz.UserId == userId && uz.ZoneId == zoneId && uz.IsActive);

            if (userZone == null)
            {
                return false;
            }

            userZone.IsActive = false;
            userZone.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ZoneDto>> GetUserZonesAsync(int userId)
        {
            var userZones = await _context.UserZones
                .Include(uz => uz.Zone)
                .Where(uz => uz.UserId == userId && uz.IsActive && uz.Zone.IsActive)
                .ToListAsync();

            return userZones.Select(uz => new ZoneDto
            {
                Id = uz.Zone.Id,
                Name = uz.Zone.Name,
                Description = uz.Zone.Description,
                IsActive = uz.Zone.IsActive,
                CreatedAt = uz.Zone.CreatedAt
            });
        }

        // Métodos auxiliares
        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private static bool VerifyPassword(string password, string hash)
        {
            return HashPassword(password) == hash;
        }

        private static string GenerateTempPassword()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private static UserDto MapToDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = user.UserRoles?
                    .Where(ur => ur.IsActive && ur.Role.IsActive)
                    .Select(ur => new RoleDto
                    {
                        Id = ur.Role.Id,
                        Name = ur.Role.Name,
                        Description = ur.Role.Description,
                        IsActive = ur.Role.IsActive,
                        CreatedAt = ur.Role.CreatedAt
                    })
                    .ToList() ?? new List<RoleDto>(),
                AssignedCompanies = user.UserCompanies?
                    .Where(uc => uc.IsActive && uc.Company.IsActive)
                    .Select(uc => new CompanyDto
                    {
                        Id = uc.Company.Id,
                        Name = uc.Company.Name,
                        Description = uc.Company.Description,
                        Address = uc.Company.Address,
                        Phone = uc.Company.Phone,
                        Email = uc.Company.Email,
                        ContactPerson = uc.Company.ContactPerson,
                        ContactPhone = uc.Company.ContactPhone,
                        ContactEmail = uc.Company.ContactEmail,
                        IsActive = uc.Company.IsActive,
                        CreatedAt = uc.Company.CreatedAt,
                        ZoneId = uc.Company.ZoneId,
                        Zone = new ZoneDto
                        {
                            Id = uc.Company.Zone.Id,
                            Name = uc.Company.Zone.Name,
                            Description = uc.Company.Zone.Description,
                            IsActive = uc.Company.Zone.IsActive,
                            CreatedAt = uc.Company.Zone.CreatedAt
                        }
                    })
                    .ToList() ?? new List<CompanyDto>(),
                AssignedZones = user.UserZones?
                    .Where(uz => uz.IsActive && uz.Zone.IsActive)
                    .Select(uz => new ZoneDto
                    {
                        Id = uz.Zone.Id,
                        Name = uz.Zone.Name,
                        Description = uz.Zone.Description,
                        IsActive = uz.Zone.IsActive,
                        CreatedAt = uz.Zone.CreatedAt
                    })
                    .ToList() ?? new List<ZoneDto>(),
                IsActive = user.IsActive,
                LastLogin = user.LastLogin,
                CreatedAt = user.CreatedAt
            };
        }
    }
}
