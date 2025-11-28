using Microsoft.EntityFrameworkCore;
using Park.Api.Data;
using Park.Api.Services.Interfaces;
using Park.Comun.DTOs;
using Park.Comun.Models;
using System.Security.Cryptography;
using System.Text;

namespace Park.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly ParkDbContext _context;
        private readonly IJwtService _jwtService;

        public AuthService(ParkDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Include(u => u.Colaborador)
                    .ThenInclude(c => c.Compania)
                .Include(u => u.Colaborador)
                    .ThenInclude(c => c.ColaboradorByCentros)
                        .ThenInclude(cbc => cbc.Centro)
                .FirstOrDefaultAsync(u => u.Username == loginDto.Username && u.IsActive);

            if (user == null)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Usuario o contraseña incorrectos"
                };
            }

            // Verificar si el usuario está bloqueado
            if (user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTime.UtcNow)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Cuenta bloqueada temporalmente"
                };
            }

            // Verificar contraseña
            if (!VerifyPassword(loginDto.Password, user.PasswordHash))
            {
                // Incrementar intentos de login
                user.LoginAttempts++;
                
                // Bloquear cuenta si excede 5 intentos
                if (user.LoginAttempts >= 5)
                {
                    user.LockoutEnd = DateTime.UtcNow.AddHours(1);
                    user.UpdatedAt = DateTime.UtcNow;
                    await _context.SaveChangesAsync();

                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "Cuenta bloqueada por múltiples intentos fallidos"
                    };
                }

                user.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Usuario o contraseña incorrectos"
                };
            }

            // Login exitoso
            user.LastLogin = DateTime.UtcNow;
            user.LoginAttempts = 0;
            user.LockoutEnd = null;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var token = _jwtService.GenerateToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();

            return new AuthResponseDto
            {
                Success = true,
                Token = token,
                RefreshToken = refreshToken,
                Expiration = DateTime.UtcNow.AddHours(24),
                Message = "Login exitoso",
                User = MapToUserDto(user)
            };
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            // Validar contraseñas
            if (registerDto.Password != registerDto.ConfirmPassword)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Las contraseñas no coinciden"
                };
            }

            // Verificar si el usuario ya existe
            if (await _context.Users.AnyAsync(u => u.Username == registerDto.Username))
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "El nombre de usuario ya existe"
                };
            }

            if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "El email ya está registrado"
                };
            }

            // Crear nuevo usuario
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

            // Generar token
            var token = _jwtService.GenerateToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();

            return new AuthResponseDto
            {
                Success = true,
                Token = token,
                RefreshToken = refreshToken,
                Expiration = DateTime.UtcNow.AddHours(24),
                Message = "Registro exitoso",
                User = MapToUserDto(user)
            };
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto refreshTokenDto)
        {
            // Validar token actual
            if (!_jwtService.ValidateToken(refreshTokenDto.Token))
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Token inválido"
                };
            }

            var userId = _jwtService.GetUserIdFromToken(refreshTokenDto.Token);
            if (!userId.HasValue)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Token inválido"
                };
            }

            var user = await _context.Users.FindAsync(userId.Value);
            if (user == null || !user.IsActive)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Usuario no encontrado o inactivo"
                };
            }

            // Generar nuevo token
            var newToken = _jwtService.GenerateToken(user);
            var newRefreshToken = _jwtService.GenerateRefreshToken();

            return new AuthResponseDto
            {
                Success = true,
                Token = newToken,
                RefreshToken = newRefreshToken,
                Expiration = DateTime.UtcNow.AddHours(24),
                Message = "Token renovado exitosamente",
                User = MapToUserDto(user)
            };
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            if (!_jwtService.ValidateToken(token))
                return false;

            var userId = _jwtService.GetUserIdFromToken(token);
            if (!userId.HasValue)
                return false;

            var user = await _context.Users.FindAsync(userId.Value);
            return user != null && user.IsActive;
        }

        public async Task<bool> LogoutAsync(string token)
        {
            // En una implementación real, podrías invalidar el token
            // Por ahora, simplemente retornamos true
            return await Task.FromResult(true);
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Include(u => u.ZonaAsignada)
                    .ThenInclude(z => z.Sitio)
                .Include(u => u.Colaborador)
                    .ThenInclude(c => c.Compania)
                .Include(u => u.Colaborador)
                    .ThenInclude(c => c.ColaboradorByCentros)
                        .ThenInclude(cbc => cbc.Centro)
                .FirstOrDefaultAsync(u => u.Id == id && u.IsActive);

            return user != null ? MapToUserDto(user) : null;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Where(u => u.IsActive)
                .ToListAsync();

            return users.Select(MapToUserDto);
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

        private static UserDto MapToUserDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IdZonaAsignada = user.IdZonaAsignada,
                IdColaborador = user.IdColaborador,
                IdCompania = user.IdCompania,
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
                    IsBlackList = user.Colaborador.IsBlackList,
                    IsActive = user.Colaborador.IsActive,
                    CreatedAt = user.Colaborador.CreatedAt,
                    UpdatedAt = user.Colaborador.UpdatedAt,
                    Compania = user.Colaborador.Compania != null ? new CompanyDto
                    {
                        Id = user.Colaborador.Compania.Id,
                        Name = user.Colaborador.Compania.Name,
                        Description = user.Colaborador.Compania.Description,
                        IsActive = user.Colaborador.Compania.IsActive,
                        CreatedAt = user.Colaborador.Compania.CreatedAt,
                        IdSitio = user.Colaborador.Compania.IdSitio
                    } : null,
                    ColaboradorByCentros = user.Colaborador.ColaboradorByCentros?
                        .Where(cbc => cbc.IsActive)
                        .Select(cbc => new ColaboradorByCentroDto
                        {
                            Id = cbc.Id,
                            IdCentro = cbc.IdCentro,
                            IdColaborador = cbc.IdColaborador,
                            IsActive = cbc.IsActive,
                            CreatedAt = cbc.CreatedAt,
                            UpdatedAt = cbc.UpdatedAt
                        }).ToList() ?? new List<ColaboradorByCentroDto>()
                } : null,
                IsActive = user.IsActive,
                LastLogin = user.LastLogin,
                CreatedAt = user.CreatedAt
            };
        }
    }
}
