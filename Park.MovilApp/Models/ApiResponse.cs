using Newtonsoft.Json;

namespace Park.MovilApp.Models
{
    public class ApiResponse<T>
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; } = string.Empty;

        [JsonProperty("data")]
        public T? Data { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; } = string.Empty;

        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; } = string.Empty;

        [JsonProperty("expiration")]
        public DateTime Expiration { get; set; }

        [JsonProperty("user")]
        public UserInfo? User { get; set; }
    }

    public class UserInfo
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; } = string.Empty;

        [JsonProperty("email")]
        public string Email { get; set; } = string.Empty;

        [JsonProperty("firstName")]
        public string FirstName { get; set; } = string.Empty;

        [JsonProperty("lastName")]
        public string LastName { get; set; } = string.Empty;

        [JsonProperty("fullName")]
        public string FullName { get; set; } = string.Empty;

        [JsonProperty("roles")]
        public List<RoleInfo> Roles { get; set; } = new List<RoleInfo>();

        [JsonProperty("assignedGates")]
        public List<GateInfo> AssignedGates { get; set; } = new List<GateInfo>();

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }
    }

    public class RoleInfo
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;
    }

    public class GateInfo
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("gateNumber")]
        public string GateNumber { get; set; } = string.Empty;

        [JsonProperty("zoneName")]
        public string ZoneName { get; set; } = string.Empty;
    }
}
