namespace Park.Comun.Models
{
    public class CompanyCentro : BaseEntity
    {
        public int IdCompania { get; set; }
        public int IdCentro { get; set; }
        public new bool IsActive { get; set; } = true;
        
        // Navigation properties
        public virtual Company Compania { get; set; } = null!;
        public virtual Centro Centro { get; set; } = null!;
    }
}
