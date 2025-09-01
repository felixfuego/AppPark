using System.ComponentModel.DataAnnotations;

namespace Park.Web.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}".Trim();
        public List<Role> Roles { get; set; } = new List<Role>();
        public List<Company> AssignedCompanies { get; set; } = new List<Company>();
        public List<Zone> AssignedZones { get; set; } = new List<Zone>();
        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class CreateUserModel
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public List<int> RoleIds { get; set; } = new List<int>();
    }

    public class EditUserModel
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public List<int> RoleIds { get; set; } = new List<int>();
        public bool IsActive { get; set; }
    }

    public class AssignCompanyModel
    {
        [Required(ErrorMessage = "Debe seleccionar una empresa")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una empresa válida")]
        public int CompanyId { get; set; }
    }

    public class UserCompanyAssignment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CompanyId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string CompanyDescription { get; set; } = string.Empty;
        public string ZoneName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class ChangePasswordModel
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }

    public class AssignZoneModel
    {
        [Required(ErrorMessage = "Debe seleccionar una zona")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una zona válida")]
        public int ZoneId { get; set; }
    }

    public class UserZoneAssignment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ZoneId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string ZoneName { get; set; } = string.Empty;
        public string ZoneDescription { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
