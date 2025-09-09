namespace Park.Comun.Models
{
    public class Centro : BaseEntity
    {
        public int IdZona { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Localidad { get; set; } = string.Empty;
        public new bool IsActive { get; set; } = true;
        
        // Navigation properties
        public virtual Zona Zona { get; set; } = null!;
        public virtual ICollection<CompanyCentro> CompanyCentros { get; set; } = new List<CompanyCentro>();
        public virtual ICollection<ColaboradorByCentro> ColaboradorByCentros { get; set; } = new List<ColaboradorByCentro>();
        public virtual ICollection<Visita> Visitas { get; set; } = new List<Visita>();
    }
}
