using Newtonsoft.Json;

namespace Park.Maui.Models
{
    public class Company
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("address")]
        public string? Address { get; set; }

        [JsonProperty("phone")]
        public string? Phone { get; set; }

        [JsonProperty("email")]
        public string? Email { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
}
