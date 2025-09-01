using Newtonsoft.Json;

namespace Park.Maui.Models
{
    public class LoginResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; } = string.Empty;

        [JsonProperty("token")]
        public string Token { get; set; } = string.Empty;

        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; } = string.Empty;

        [JsonProperty("expiresIn")]
        public int ExpiresIn { get; set; }

        [JsonProperty("user")]
        public UserInfo User { get; set; } = new();
    }

    public class UserInfo
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; } = string.Empty;

        [JsonProperty("fullName")]
        public string FullName { get; set; } = string.Empty;

        [JsonProperty("email")]
        public string Email { get; set; } = string.Empty;

        [JsonProperty("roles")]
        public List<Role> Roles { get; set; } = new();
    }

    public class Role
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;
    }
}
