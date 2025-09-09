namespace Park.Comun.Models
{
    public class Sitio : BaseEntity
    {
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public new bool IsActive { get; set; } = true;
        
        // Navigation properties
        public virtual ICollection<Zona> Zonas { get; set; } = new List<Zona>();
        public virtual ICollection<Company> Companias { get; set; } = new List<Company>();
    }
}
