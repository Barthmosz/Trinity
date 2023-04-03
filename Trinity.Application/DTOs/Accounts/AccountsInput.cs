using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Trinity.Application.DTOs.Users
{
    public class AccountsInput
    {
        [JsonPropertyName("name")]
        [JsonRequired]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        [JsonRequired]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("password")]
        [JsonRequired]
        public string Password { get; set; } = string.Empty;
    }
}
