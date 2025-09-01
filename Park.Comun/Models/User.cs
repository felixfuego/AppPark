namespace Park.Comun.Models
{
    public class User : BaseEntity
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public new bool IsActive { get; set; } = true;
        public DateTime? LastLogin { get; set; }
        public int LoginAttempts { get; set; } = 0;
        public DateTime? LockoutEnd { get; set; }
        
        // Relación muchos a muchos con Role
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        
        // Propiedad de navegación para acceder directamente a los roles
        public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
        
        // Relaciones con empresas y zonas
        public virtual ICollection<UserCompany> UserCompanies { get; set; } = new List<UserCompany>();
        public virtual ICollection<UserZone> UserZones { get; set; } = new List<UserZone>();
        
        // Visitas creadas por el usuario
        public virtual ICollection<Visit> CreatedVisits { get; set; } = new List<Visit>();
    }
}
