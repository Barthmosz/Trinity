using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Trinity.Application.DTOs.Products
{
    public class ProductsUpdateInput
    {
        [JsonPropertyName("name")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be at least 3 characters.")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Description must be at least 3 characters.")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("image")]
        public string Image { get; set; } = string.Empty;

        [JsonPropertyName("quantity")]
        [Range(1, 999, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }

        [JsonPropertyName("price")]
        [Range(1, 99.999, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [JsonPropertyName("discount")]
        [Range(0, 1, ErrorMessage = "Discount must be between 0 and 1")]
        public decimal Discount { get; set; }
    }
}
