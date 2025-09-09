namespace Park.Comun.Models
{
    public class Zona : BaseEntity
    {
        public int IdSitio { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public new bool IsActive { get; set; } = true;
        
        // Navigation properties
        public virtual Sitio Sitio { get; set; } = null!;
        public virtual ICollection<Centro> Centros { get; set; } = new List<Centro>();
    }
}
