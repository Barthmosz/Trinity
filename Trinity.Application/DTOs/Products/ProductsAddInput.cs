using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Trinity.Application.DTOs.Products
{
    public class ProductsAddInput
    {
        [JsonPropertyName("name")]
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be at least 3 characters.")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        [Required(ErrorMessage = "Description is required.")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Description must be at least 3 characters.")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("imageUrl")]
        public string ImageUrl { get; set; } = string.Empty;

        [JsonPropertyName("quantity")]
        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, 999, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }

        [JsonPropertyName("price")]
        [Required(ErrorMessage = "Price is required.")]
        [Range(1, 99.999, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }
    }
}
