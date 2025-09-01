namespace Park.Comun.Models
{
    public class Role : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public new bool IsActive { get; set; } = true;
        
        // Relación muchos a muchos con User
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
