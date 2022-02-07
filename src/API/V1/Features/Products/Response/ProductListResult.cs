using System.Text.Json.Serialization;

namespace API.V1.Features.Products.Response;

public class ProductListResult
{
    [JsonPropertyName("productId")]
    public int ProductId { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
}