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
                .Include(u => u.Colaborador)
                    .ThenInclude(c => c.Compania)
                .Include(u => u.Colaborador)
                    .ThenInclude(c => c.ColaboradorByCentros)
                        .ThenInclude(cbc => cbc.Centro)
                .Include(u => u.ZonaAsignada)
                    .ThenInclude(z => z.Sitio)
                .Where(u => u.IsActive)
                .ToListAsync();

            return users.Select(MapToDto);
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Include(u => u.Colaborador)
                    .ThenInclude(c => c.Compania)
                .Include(u => u.Colaborador)
                    .ThenInclude(c => c.ColaboradorByCentros)
                        .ThenInclude(cbc => cbc.Centro)
                .Include(u => u.ZonaAsignada)
                    .ThenInclude(z => z.Sitio)
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
            // Obtener las empresas a través del colaborador asignado al usuario
            var user = await _context.Users
                .Include(u => u.Colaborador)
                    .ThenInclude(c => c.Compania)
                .FirstOrDefaultAsync(u => u.Id == userId && u.IsActive);

            if (user?.Colaborador?.Compania != null)
            {
                return new List<CompanyDto>
                {
                    new CompanyDto
                    {
                        Id = user.Colaborador.Compania.Id,
                        Name = user.Colaborador.Compania.Name,
                        Description = user.Colaborador.Compania.Description,
                        IsActive = user.Colaborador.Compania.IsActive,
                        CreatedAt = user.Colaborador.Compania.CreatedAt,
                        VisitsCount = 0, // TODO: Calcular desde visitas
                        IdSitio = user.Colaborador.Compania.IdSitio
                    }
                };
            }

            return new List<CompanyDto>();
        }

        public async Task<bool> AssignColaboradorToUserAsync(int userId, int colaboradorId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new InvalidOperationException("El usuario no existe.");
            }

            var colaborador = await _context.Colaboradores
                .Include(c => c.Compania)
                .FirstOrDefaultAsync(c => c.Id == colaboradorId && c.IsActive);

            if (colaborador == null)
            {
                throw new InvalidOperationException("El colaborador no existe o no está activo.");
            }

            // Verificar que el colaborador no esté ya asignado a otro usuario
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.IdColaborador == colaboradorId && u.Id != userId);

            if (existingUser != null)
            {
                throw new InvalidOperationException($"El colaborador '{colaborador.Nombre}' ya está asignado al usuario '{existingUser.Username}'.");
            }

            user.IdColaborador = colaboradorId;
            user.IdCompania = colaborador.IdCompania; // Heredar la empresa del colaborador
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveColaboradorFromUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return false;
            }

            user.IdColaborador = null;
            user.IdCompania = null;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AssignZoneToUserAsync(AssignZoneToUserDto assignZoneDto)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.UserRoles)
                        .ThenInclude(ur => ur.Role)
                    .FirstOrDefaultAsync(u => u.Id == assignZoneDto.UserId && u.IsActive);

                if (user == null)
                {
                    return false;
                }

                // Verificar que el usuario tenga rol de Guardia
                var isGuardia = user.UserRoles.Any(ur => ur.Role.Name == "Guardia");
                if (!isGuardia)
                {
                    return false;
                }

                // Asignar o quitar la zona
                user.IdZonaAsignada = assignZoneDto.ZonaId;
                user.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
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
                IdColaborador = user.IdColaborador,
                IdCompania = user.IdCompania,
                IdZonaAsignada = user.IdZonaAsignada,
                Roles = user.UserRoles?.Select(ur => new RoleDto
                {
                    Id = ur.Role.Id,
                    Name = ur.Role.Name,
                    Description = ur.Role.Description,
                    IsActive = ur.Role.IsActive,
                    CreatedAt = ur.Role.CreatedAt
                }).ToList() ?? new List<RoleDto>(),
                Colaborador = user.Colaborador != null ? new ColaboradorDto
                {
                    Id = user.Colaborador.Id,
                    IdCompania = user.Colaborador.IdCompania,
                    Identidad = user.Colaborador.Identidad,
                    Nombre = user.Colaborador.Nombre,
                    Puesto = user.Colaborador.Puesto,
                    Email = user.Colaborador.Email,
                    Tel1 = user.Colaborador.Tel1,
                    Tel2 = user.Colaborador.Tel2,
                    Tel3 = user.Colaborador.Tel3,
                    PlacaVehiculo = user.Colaborador.PlacaVehiculo,
                    Comentario = user.Colaborador.Comentario,
                    IsActive = user.Colaborador.IsActive,
                    IsBlackList = user.Colaborador.IsBlackList,
                    Compania = user.Colaborador.Compania != null ? new CompanyDto
                    {
                        Id = user.Colaborador.Compania.Id,
                        Name = user.Colaborador.Compania.Name,
                        Description = user.Colaborador.Compania.Description,
                        IsActive = user.Colaborador.Compania.IsActive,
                        CreatedAt = user.Colaborador.Compania.CreatedAt,
                        VisitsCount = 0, // TODO: Calcular desde visitas
                        IdSitio = user.Colaborador.Compania.IdSitio
                    } : null,
                    ColaboradorByCentros = user.Colaborador.ColaboradorByCentros?.Select(cbc => new ColaboradorByCentroDto
                    {
                        Id = cbc.Id,
                        IdColaborador = cbc.IdColaborador,
                        IdCentro = cbc.IdCentro,
                        Centro = cbc.Centro != null ? new CentroDto
                        {
                            Id = cbc.Centro.Id,
                            IdZona = cbc.Centro.IdZona,
                            Nombre = cbc.Centro.Nombre,
                            Localidad = cbc.Centro.Localidad,
                            IsActive = cbc.Centro.IsActive,
                            CreatedAt = cbc.Centro.CreatedAt
                        } : null
                    }).ToList() ?? new List<ColaboradorByCentroDto>()
                } : null,
                Compania = user.Compania != null ? new CompanyDto
                {
                    Id = user.Compania.Id,
                    Name = user.Compania.Name,
                    Description = user.Compania.Description,
                    IsActive = user.Compania.IsActive,
                    CreatedAt = user.Compania.CreatedAt,
                    VisitsCount = 0, // TODO: Calcular desde visitas
                    IdSitio = user.Compania.IdSitio
                } : null,
                ZonaAsignada = user.ZonaAsignada != null ? new ZonaDto
                {
                    Id = user.ZonaAsignada.Id,
                    IdSitio = user.ZonaAsignada.IdSitio,
                    Nombre = user.ZonaAsignada.Nombre,
                    Descripcion = user.ZonaAsignada.Descripcion,
                    IsActive = user.ZonaAsignada.IsActive,
                    CreatedAt = user.ZonaAsignada.CreatedAt,
                    Sitio = user.ZonaAsignada.Sitio != null ? new SitioDto
                    {
                        Id = user.ZonaAsignada.Sitio.Id,
                        Nombre = user.ZonaAsignada.Sitio.Nombre,
                        Descripcion = user.ZonaAsignada.Sitio.Descripcion,
                        IsActive = user.ZonaAsignada.Sitio.IsActive,
                        CreatedAt = user.ZonaAsignada.Sitio.CreatedAt
                    } : null
                } : null,
                AssignedCompanies = new List<CompanyDto>() // Mantener para compatibilidad
            };
        }
    }
}