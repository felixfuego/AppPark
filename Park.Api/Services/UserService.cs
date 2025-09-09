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

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Where(u => u.IsActive)
                .ToListAsync();

            return users.Select(MapToDto);
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
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

        public async Task<UserDto> CreateUserAsync(RegisterDto registerDto)
        {
            // Verificar si el usuario ya existe
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == registerDto.Username || u.Email == registerDto.Email);

            if (existingUser != null)
            {
                throw new InvalidOperationException($"El usuario '{registerDto.Username}' o email '{registerDto.Email}' ya existe.");
            }

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

            // Asignar roles al usuario
            if (registerDto.RoleIds.Any())
            {
                foreach (var roleId in registerDto.RoleIds)
                {
                    var role = await _context.Roles.FindAsync(roleId);
                    if (role != null)
                    {
                        var userRole = new UserRole
                        {
                            UserId = user.Id,
                            RoleId = roleId,
                            CreatedAt = DateTime.UtcNow
                        };
                        _context.UserRoles.Add(userRole);
                    }
                }
                await _context.SaveChangesAsync();
            }

            return MapToDto(user);
        }

        public async Task<UserDto> UpdateUserAsync(int id, UpdateUserDto updateUserDto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                throw new InvalidOperationException("El usuario no existe.");
            }

            // Verificar si el nuevo username o email ya existe en otro usuario
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => (u.Username == updateUserDto.Username || u.Email == updateUserDto.Email) && u.Id != id);

            if (existingUser != null)
            {
                throw new InvalidOperationException($"El username '{updateUserDto.Username}' o email '{updateUserDto.Email}' ya existe en otro usuario.");
            }

            user.Username = updateUserDto.Username;
            user.Email = updateUserDto.Email;
            user.FirstName = updateUserDto.FirstName;
            user.LastName = updateUserDto.LastName;
            user.IsActive = updateUserDto.IsActive;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return MapToDto(user);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return false;
            }

            user.IsActive = false;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return false;
            }

            // Verificar la contraseña actual
            var currentPasswordHash = HashPassword(changePasswordDto.CurrentPassword);
            if (user.PasswordHash != currentPasswordHash)
            {
                throw new InvalidOperationException("La contraseña actual es incorrecta.");
            }

            // Actualizar la contraseña
            user.PasswordHash = HashPassword(changePasswordDto.NewPassword);
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> LockUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return false;
            }

            user.LockoutEnd = DateTime.UtcNow.AddYears(100); // Lock for 100 years
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UnlockUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return false;
            }

            user.LockoutEnd = null;
            user.LoginAttempts = 0;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AssignUserToCompanyAsync(int userId, int companyId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new InvalidOperationException("El usuario no existe.");
            }

            var company = await _context.Companies.FindAsync(companyId);
            if (company == null || !company.IsActive)
            {
                throw new InvalidOperationException("La empresa no existe o no está activa.");
            }

            // TODO: Implementar cuando se cree la nueva estructura de UserCompany
            // var existingAssignment = await _context.UserCompanies
            //     .FirstOrDefaultAsync(uc => uc.UserId == userId && uc.CompanyId == companyId);

            // if (existingAssignment != null)
            // {
            //     throw new InvalidOperationException("El usuario ya está asignado a esta empresa.");
            // }

            // var userCompany = new UserCompany
            // {
            //     UserId = userId,
            //     CompanyId = companyId,
            //     IsActive = true,
            //     CreatedAt = DateTime.UtcNow
            // };

            // _context.UserCompanies.Add(userCompany);
            // await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveUserFromCompanyAsync(int userId, int companyId)
        {
            // TODO: Implementar cuando se cree la nueva estructura de UserCompany
            // var userCompany = await _context.UserCompanies
            //     .FirstOrDefaultAsync(uc => uc.UserId == userId && uc.CompanyId == companyId);

            // if (userCompany == null)
            // {
            //     return false;
            // }

            // userCompany.IsActive = false;
            // userCompany.UpdatedAt = DateTime.UtcNow;

            // await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<CompanyDto>> GetUserCompaniesAsync(int userId)
        {
            // TODO: Implementar cuando se cree la nueva estructura de UserCompany
            // var userCompanies = await _context.UserCompanies
            //     .Include(uc => uc.Company)
            //     .Where(uc => uc.UserId == userId && uc.IsActive && uc.Company.IsActive)
            //     .ToListAsync();

            // return userCompanies.Select(uc => new CompanyDto
            // {
            //     Id = uc.Company.Id,
            //     Name = uc.Company.Name,
            //     Description = uc.Company.Description,
            //     Address = uc.Company.Address,
            //     Phone = uc.Company.Phone,
            //     Email = uc.Company.Email,
            //     ContactPerson = uc.Company.ContactPerson,
            //     ContactPhone = uc.Company.ContactPhone,
            //     ContactEmail = uc.Company.ContactEmail,
            //     IsActive = uc.Company.IsActive,
            //     CreatedAt = uc.Company.CreatedAt,
            //     VisitsCount = 0 // TODO: Implementar cuando se creen las nuevas visitas
            // });

            return new List<CompanyDto>();
        }

        // Métodos auxiliares
        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
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
                IsActive = user.IsActive,
                IsLocked = user.LockoutEnd.HasValue && user.LockoutEnd > DateTime.UtcNow,
                LastLogin = user.LastLogin,
                LastLoginDate = user.LastLogin,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                Roles = user.UserRoles?.Select(ur => new RoleDto
                {
                    Id = ur.Role.Id,
                    Name = ur.Role.Name,
                    Description = ur.Role.Description,
                    IsActive = ur.Role.IsActive,
                    CreatedAt = ur.Role.CreatedAt
                }).ToList() ?? new List<RoleDto>(),
                AssignedCompanies = new List<CompanyDto>() // TODO: Implementar cuando se cree la nueva estructura
            };
        }
    }
}