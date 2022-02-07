using System.Text.Json.Serialization;

namespace API.V1.Features.Products.Request;

public class ProductUpdateRequest
{
    [JsonPropertyName("productId")]
    public int ProductId { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
}