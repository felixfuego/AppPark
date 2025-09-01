namespace Park.Comun.Models
{
    public class Visitor : BaseEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string DocumentType { get; set; } = string.Empty; // DNI, Pasaporte, etc.
        public string DocumentNumber { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty; // Empresa del visitante (externa)
        public new bool IsActive { get; set; } = true;
        
        // Relaciones
        public virtual ICollection<Visit> Visits { get; set; } = new List<Visit>();
    }
}
