namespace Park.Comun.Models
{
    public class Visitor : BaseEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string DocumentType { get; set; } = string.Empty;
        public string DocumentNumber { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        
        // Propiedad calculada para nombre completo
        public string FullName => $"{FirstName} {LastName}".Trim();
    }
}
