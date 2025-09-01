using Newtonsoft.Json;

namespace Park.Maui.Models
{
    public class LoginRequest
    {
        [JsonProperty("username")]
        public string Username { get; set; } = string.Empty;

        [JsonProperty("password")]
        public string Password { get; set; } = string.Empty;
    }
}
