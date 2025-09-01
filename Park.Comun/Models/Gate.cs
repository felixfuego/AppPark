namespace Park.Comun.Models
{
    public class Gate : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string GateNumber { get; set; } = string.Empty;
        public new bool IsActive { get; set; } = true;
        
        // Relación con Zona
        public int ZoneId { get; set; }
        public virtual Zone Zone { get; set; } = null!;
        
        // Relaciones
        public virtual ICollection<Visit> Visits { get; set; } = new List<Visit>();
    }
}
