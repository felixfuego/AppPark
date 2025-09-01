namespace Park.Comun.Models
{
    public class UserZone : BaseEntity
    {
        public int UserId { get; set; }
        public int ZoneId { get; set; }
        public new bool IsActive { get; set; } = true;
        
        // Navegación
        public virtual User User { get; set; } = null!;
        public virtual Zone Zone { get; set; } = null!;
    }
}
