using System.ComponentModel.DataAnnotations;

namespace Park.Comun.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        public string Username { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "La contraseña es requerida")]
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterDto
    {
        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre de usuario debe tener entre 3 y 50 caracteres")]
        public string Username { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        public string Email { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "La contraseña es requerida")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        public string Password { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Debe confirmar la contraseña")]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmPassword { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(50, ErrorMessage = "El nombre no puede exceder 50 caracteres")]
        public string FirstName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "El apellido es requerido")]
        [StringLength(50, ErrorMessage = "El apellido no puede exceder 50 caracteres")]
        public string LastName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Debe seleccionar al menos un rol")]
        public List<int> RoleIds { get; set; } = new List<int>();
    }

    public class AuthResponseDto
    {
        public bool Success { get; set; }
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
        public string Message { get; set; } = string.Empty;
        public UserDto? User { get; set; }
    }

    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}".Trim();
        public string Password { get; set; } = string.Empty;
        public List<RoleDto> Roles { get; set; } = new List<RoleDto>();
        public List<CompanyDto> AssignedCompanies { get; set; } = new List<CompanyDto>();
        public List<ZoneDto> AssignedZones { get; set; } = new List<ZoneDto>();
        public List<GateDto> AssignedGates { get; set; } = new List<GateDto>();
        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class RefreshTokenDto
    {
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }

    public class ChangePasswordDto
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }

    public class AssignUserToCompanyDto
    {
        public int UserId { get; set; }
        public int CompanyId { get; set; }
    }

    public class UserCompanyDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CompanyId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public CompanyDto Company { get; set; } = null!;
    }



    public class AssignUserToZoneDto
    {
        public int UserId { get; set; }
        public int ZoneId { get; set; }
    }

    public class UserZoneDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ZoneId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public ZoneDto Zone { get; set; } = null!;
    }
}
