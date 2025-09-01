namespace Park.Comun.Models
{
    public class UserRole : BaseEntity
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public new bool IsActive { get; set; } = true;
        
        // Navegación
        public virtual User User { get; set; } = null!;
        public virtual Role Role { get; set; } = null!;
    }
}
