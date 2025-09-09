namespace Park.Comun.Models
{
    public class CompanyZona : BaseEntity
    {
        public int IdCompania { get; set; }
        public int IdZona { get; set; }
        public new bool IsActive { get; set; } = true;
        
        // Navigation properties
        public virtual Company Compania { get; set; } = null!;
        public virtual Zona Zona { get; set; } = null!;
    }
}
