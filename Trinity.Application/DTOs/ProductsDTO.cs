using System.Text.Json.Serialization;

namespace Trinity.Application.DTOs
{
    public class ProductsDTO
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        [JsonRequired]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        [JsonRequired]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("image")]
        public string Image { get; set; } = string.Empty;

        [JsonPropertyName("quantity")]
        [JsonRequired]
        public int Quantity { get; set; }

        [JsonPropertyName("price")]
        [JsonRequired]
        public decimal Price { get; set; }

        [JsonPropertyName("discount")]
        public decimal Discount { get; set; }
    }
}
