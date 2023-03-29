using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Trinity.Application.DTOs.Products
{
    public class ProductsInput
    {
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
    }
}
