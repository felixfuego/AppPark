namespace Park.Comun.Models
{
    public class Company : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ContactPerson { get; set; } = string.Empty;
        public string ContactPhone { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;
        public new bool IsActive { get; set; } = true;
        
        // Nueva propiedad para el nuevo modelo
        public int IdSitio { get; set; }
        
        // Navigation properties
        public virtual Sitio Sitio { get; set; } = null!;
        public virtual ICollection<CompanyCentro> CompanyCentros { get; set; } = new List<CompanyCentro>();
        public virtual ICollection<CompanyZona> CompanyZonas { get; set; } = new List<CompanyZona>();
        public virtual ICollection<Colaborador> Colaboradores { get; set; } = new List<Colaborador>();
        public virtual ICollection<Visita> Visitas { get; set; } = new List<Visita>();
    }
}
