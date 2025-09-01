using Newtonsoft.Json;

namespace Park.Maui.Models
{
    public class Visitor
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; } = string.Empty;

        [JsonProperty("lastName")]
        public string LastName { get; set; } = string.Empty;

        [JsonProperty("documentType")]
        public string DocumentType { get; set; } = string.Empty;

        [JsonProperty("documentNumber")]
        public string DocumentNumber { get; set; } = string.Empty;

        [JsonProperty("email")]
        public string? Email { get; set; }

        [JsonProperty("phone")]
        public string? Phone { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        // Propiedad calculada para el nombre completo
        public string FullName => $"{FirstName} {LastName}".Trim();
        
        // Propiedad para mostrar información del documento
        public string DocumentInfo => $"{DocumentType}: {DocumentNumber}";
    }
}
