namespace Park.Comun.Models
{
    public class Zone : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public new bool IsActive { get; set; } = true;
        
        // Relaciones
        public virtual ICollection<Company> Companies { get; set; } = new List<Company>();
        public virtual ICollection<Gate> Gates { get; set; } = new List<Gate>();
        public virtual ICollection<UserZone> UserZones { get; set; } = new List<UserZone>();
    }
}
