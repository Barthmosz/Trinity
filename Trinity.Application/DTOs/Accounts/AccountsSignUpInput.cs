using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Trinity.Application.DTOs.Accounts
{
    public class AccountsSignUpInput
    {
        [JsonPropertyName("name")]
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be at least 3 characters.")]
        public string Name { get; set; } = string.Empty;

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
