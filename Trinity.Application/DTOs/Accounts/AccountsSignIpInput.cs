using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Trinity.Application.DTOs.Users
{
    public class AccountsSignInInput
    {
        [JsonPropertyName("email")]
        [EmailAddress(ErrorMessage = "Email is not valid.")]
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("password")]
        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; } = string.Empty;
    }
}
