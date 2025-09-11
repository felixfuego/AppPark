namespace Park.Comun.Models
{
    public class Company : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public new bool IsActive { get; set; } = true;
        
        // Propiedad para el sitio
        public int IdSitio { get; set; }
        
        // Navigation properties
        public virtual Sitio Sitio { get; set; } = null!;
        public virtual ICollection<CompanyZona> CompanyZonas { get; set; } = new List<CompanyZona>();
        public virtual ICollection<Colaborador> Colaboradores { get; set; } = new List<Colaborador>();
        public virtual ICollection<Visita> Visitas { get; set; } = new List<Visita>();
    }
}
