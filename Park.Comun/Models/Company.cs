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
        
        // Relación con Zona
        public int ZoneId { get; set; }
        public virtual Zone Zone { get; set; } = null!;
        
        // Relaciones
        public virtual ICollection<UserCompany> UserCompanies { get; set; } = new List<UserCompany>();
        public virtual ICollection<Visit> Visits { get; set; } = new List<Visit>();
    }
}
