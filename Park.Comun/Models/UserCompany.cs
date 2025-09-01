namespace Park.Comun.Models
{
    public class UserCompany : BaseEntity
    {
        public int UserId { get; set; }
        public int CompanyId { get; set; }
        public new bool IsActive { get; set; } = true;
        
        // Navegación
        public virtual User User { get; set; } = null!;
        public virtual Company Company { get; set; } = null!;
    }
}
