using DataAnnotationsExtensions;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Trinity.Application.DTOs.Products
{
    public class ProductsInput
    {
        [JsonPropertyName("name")]
        [JsonRequired]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        [JsonRequired]
        [StringLength(200, MinimumLength = 3)]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("image")]
        public string Image { get; set; } = string.Empty;

        [JsonPropertyName("quantity")]
        [JsonRequired]
        [Range(1, 999)]
        public int Quantity { get; set; }

        [JsonPropertyName("price")]
        [JsonRequired]
        [Min(1)]
        public decimal Price { get; set; }

        [JsonPropertyName("discount")]
        [Range(0, 1)]
        public decimal Discount { get; set; }
    }
}
